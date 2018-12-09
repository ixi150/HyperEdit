using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Xunity.Extensions;

namespace Game.StageEdit
{
    public class StageEditManager : ScriptableObject
    {
        [SerializeField] Vector2Int pixelSize = new Vector2Int(2, 2);
        [SerializeField] StageData data;
        [SerializeField] [TextArea(5, 50)] string json;

        readonly Dictionary<Vector2Int, Item> items = new Dictionary<Vector2Int, Item>();
        readonly Dictionary<Vector2Int, Item> itemRoots = new Dictionary<Vector2Int, Item>();
        
        Camera camera;

        StageData Data
        {
            get
            {
                if (data == null)
                    data = CreateInstance<StageData>();
                return data;
            }
        }

        public Vector2Int PixelSize
        {
            get { return pixelSize; }
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
            bool success = items.TryGetValue(coord, out item);
            if (success && item == null)
                items.Remove(coord);
            return item;
        }

        public void RemoveItem(Vector2Int coord)
        {
            Item item;
            bool success = itemRoots.TryGetValue(coord, out item);
            if (success)
            {
                itemRoots.Remove(coord);
            }

            foreach (var key in items.Keys.ToArray())
            {
                if (items[key] == item)
                    items.Remove(key);
            }
        }

        public void AddItem(Item item)
        {
            var coord = SnapPosition(item.Position);
            itemRoots[coord] = item;
            
            for (int y = item.Size.yMin; y < item.Size.yMax; y++)
            for (int x = item.Size.xMin; x < item.Size.xMax; x++)
            {
                var offset = new Vector2Int
                (
                    Mathf.FloorToInt(Mathf.Sign(item.transform.localScale.x) * PixelSize.x * x),
                    Mathf.FloorToInt(Mathf.Sign(item.transform.localScale.y) * PixelSize.y * y)
                );
                AddItem(item, coord + offset);
            }
        }

        void AddItem(Item item, Vector2Int coord)
        {
            if (items.ContainsKey(coord))
            {
                if (items[coord])
                Destroy(items[coord].gameObject);
                RemoveItem(coord);
            }

            items[coord] = item;
        }

        public void ClearMap()
        {
            foreach (var item in itemRoots.Values)
                if (item && item.gameObject)
                    Destroy(item.gameObject);

            items.Clear();
            itemRoots.Clear();
        }

        public void SaveStage()
        {
            Data.ClearItems();
            foreach (var pair in itemRoots)
            {
                Data.AddItem(pair.Key, pair.Value.Id);
            }

            json = JsonUtility.ToJson(Data, true);
            PlayerPrefs.SetString("1", json);
        }

        public void LoadStage()
        {
            ClearMap();
            Data.ClearItems();
            json = PlayerPrefs.GetString("1", "");
            JsonUtility.FromJsonOverwrite(json, data);
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
    }
}