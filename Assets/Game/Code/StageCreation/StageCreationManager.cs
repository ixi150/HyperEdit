using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Xunity.Extensions;
using Xunity.ScriptableReferences;
using Xunity.ScriptableVariables;

namespace Game.Code.StageCreation
{
    public class StageCreationManager : ScriptableObject
    {
        const string SAVE_PREFIX = "LocalMapSlot";

        [SerializeField] Vector2Int pixelSize = new Vector2Int(2, 2);
        [SerializeField] StageData data;
        [SerializeField] StringVariable[] examples;
        [SerializeField] [TextArea(5, 30)] string json;

        readonly List<ItemCoordinates> itemsOnMap = new List<ItemCoordinates>();

        string saveWip;
        Camera camera;

        public int CurrentSaveSlot { get; set; }

        public Vector2Int PixelSize
        {
            get { return pixelSize; }
        }

        public string Json
        {
            get { return json; }
        }


        StageData Data
        {
            get
            {
                if (data == null)
                    data = CreateInstance<StageData>();
                return data;
            }
        }

        Camera Camera
        {
            get
            {
                if (camera == null)
                    camera = Camera.main;
                return camera;
            }
        }

        public bool TryGetItem(Vector2Int coord, out Item item)
        {
            item = itemsOnMap
                .Where(i => i.Coords.Any(c => c == coord))
                .Select(i => i.Item)
                .FirstOrDefault();
            return item;
        }

        public void RemoveItem(Vector2Int coord)
        {
            itemsOnMap.RemoveAll(i => i.Coords.Any(c => c == coord));
        }

        public void RemoveItems(params Vector2Int[] coords)
        {
            coords.ForEach(RemoveItem);
        }

        public void AddItem(Item item)
        {
            var position = SnapPosition(item.Position);
            var coords = IterateAllCoords(item, position).ToArray();
            DestroyItems(coords);
            RemoveItems(coords);
            itemsOnMap.Add(new ItemCoordinates(item, position, coords));
        }

        IEnumerable<Vector2Int> IterateAllCoords(Item item, Vector2Int coord)
        {
            for (int y = item.Size.yMin; y < item.Size.yMax; y++)
            for (int x = item.Size.xMin; x < item.Size.xMax; x++)
            {
                yield return coord + new Vector2Int
                             (
                                 Mathf.FloorToInt(Mathf.Sign(item.transform.localScale.x) * PixelSize.x * x),
                                 Mathf.FloorToInt(Mathf.Sign(item.transform.localScale.y) * PixelSize.y * y)
                             );
            }
        }

        void DestroyItems(params Vector2Int[] coords)
        {
            itemsOnMap.Where(i => i.Coords.Any(coords.Contains))
                .ForEach(i => i.DestroyItem());
        }

        public void ClearMap()
        {
            Data.ClearItems();
            itemsOnMap.ForEach(i => i.DestroyItem());
            itemsOnMap.Clear();
        }

        public void SaveWip()
        {
            saveWip = GetSaveString();
        }

        public void LoadWip()
        {
            LoadStageFromString(saveWip);
        }

        public void SaveStage()
        {
            PlayerPrefs.SetString(SAVE_PREFIX + CurrentSaveSlot, GetSaveString());
        }

        string GetSaveString()
        {
            Data.ClearItems();
            foreach (var itemOnMap in itemsOnMap)
                Data.AddItem(itemOnMap.Position, itemOnMap.Item.Id);
            return json = JsonUtility.ToJson(Data, true);
        }

        public void LoadStage()
        {
            json = PlayerPrefs.GetString(SAVE_PREFIX + CurrentSaveSlot);
            LoadStageFromString(json);
        }

        public void LoadExample(int index)
        {
            LoadStageFromString(examples[index]);
        }

        public void SaveExample(int index)
        {
            examples[index].Set(GetSaveString(), this);
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(examples[index]);
#endif
        }

        public void LoadStageFromString(string saveString)
        {
            ClearMap();
            Data.ClearItems();
            JsonUtility.FromJsonOverwrite(saveString, Data);
            var prefabDictionary = Resources.LoadAll<ItemData>("")
                .ToDictionary(itemData => itemData.prefab.Id, itemData => itemData.prefab);

            foreach (var item in Data.Items)
            {
                Item prefab;
                if (!prefabDictionary.TryGetValue(item.id, out prefab))
                    continue;
                AddItem(Instantiate(prefab, item.coord.ToVector2(), prefab.transform.rotation));
            }
        }

        public Vector3 GetWorldMousePosition()
        {
            var mousePos = Input.mousePosition.Modified(z: -Camera.transform.position.z);
            return Camera.ScreenToWorldPoint(mousePos);
        }

        public Vector2Int GetWorldMouseSnapPosition()
        {
            return SnapPosition(GetWorldMousePosition());
        }

        public Vector2Int SnapPosition(Vector2 pos)
        {
            return new Vector2Int(
                SnapWorldAxis(pos, 0),
                SnapWorldAxis(pos, 1)
            );
        }

        public bool IsMouseInPlacablePosition()
        {
            if (IsMouseOverUi())
                return false;

            var pos = GetWorldMouseSnapPosition();
            return Data.Bounds.Contains(pos);
        }

        public bool IsPointInPlacablePosition(Vector3 worldPoint)
        {
            if (IsPointOverUi(worldPoint))
                return false;

            var pos = SnapPosition(worldPoint);
            return Data.Bounds.Contains(pos);
        }

        public bool IsItemInPlacablePosition(Item item)
        {
            var pos = item.Position;
            if (!IsPointInPlacablePosition(pos))
                return false;

            int y = SnapPosition(pos).y;
            return item.Placement.ContainsFlag(Item.PlacementRule.Ceil) && Data.Bounds.yMax - 2 == y
                   || item.Placement.ContainsFlag(Item.PlacementRule.Floor) && Data.Bounds.yMin == y
                   || item.Placement.ContainsFlag(Item.PlacementRule.Mid);
        }

        public bool IsMouseOverUi()
        {
            return IsPointOverUi(Input.mousePosition);
        }

        public bool IsPointOverUi(Vector3 screenPoint)
        {
            if (EventSystem.current == null)
                return false;
            var eventData = new PointerEventData(EventSystem.current) {position = screenPoint};
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            return results.Count > 0; // || !Screen.safeArea.Contains(screenPoint);
        }

        int SnapWorldAxis(Vector2 worldPos, int axis)
        {
            float pos = worldPos[axis];
            int snap = pixelSize[axis];
            return Mathf.RoundToInt(pos / snap) * snap;
        }

        void OnDisable()
        {
            CurrentSaveSlot = 0;
        }

        class ItemCoordinates
        {
            Item item;
            Vector2Int position;
            Vector2Int[] coords;

            public ItemCoordinates(Item item, Vector2Int position, params Vector2Int[] coords)
            {
                this.item = item;
                this.position = position;
                this.coords = coords;
            }

            public Item Item
            {
                get { return item; }
            }

            public Vector2Int Position
            {
                get { return position; }
            }

            public IEnumerable<Vector2Int> Coords
            {
                get { return coords; }
            }

            public void DestroyItem()
            {
                Destroy(item.gameObject);
            }
        }
    }
}