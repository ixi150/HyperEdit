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
        [SerializeField] [TextArea(5, 50)] string json;
        [SerializeField] StageData data;

        readonly Dictionary<Vector2Int, Item> items = new Dictionary<Vector2Int, Item>();
        GraphicRaycaster raycaster;
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

        public GraphicRaycaster Raycaster
        {
            get
            {
                if (raycaster == null)
                    raycaster = FindObjectOfType<GraphicRaycaster>();
                return raycaster;
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
            return items.TryGetValue(coord, out item);
        }

        public void RemoveItem(Vector2Int coord)
        {
            if (items.ContainsKey(coord))
            {
                items.Remove(coord);
            }
        }
        
        public void AddItem(Item item)
        {
            var coord = SnapPosition(item.Position);
            if (items.ContainsKey(coord))
                Destroy(items[coord].gameObject);

            items[coord] = item;
        }

        public void ClearMap()
        {
            foreach (var item in items.Values)
            {
                Destroy(item.gameObject);
            }

            items.Clear();
        }

        public void SaveStage()
        {
            Data.ClearItems();
            foreach (var pair in items)
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
                AddItem(Instantiate(prefab, item.coord.ToVector2(), Quaternion.identity));
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
        
        public bool IsMouseOverUi()
        {
            return IsPointOverUi(Input.mousePosition);
        }

        public bool IsPointOverUi(Vector3 screenPoint)
        {
            var pointerData = new PointerEventData(EventSystem.current);
            var results = new List<RaycastResult>();
            pointerData.position = screenPoint;
            Raycaster.Raycast(pointerData, results);
            return results.Count > 0;
        }

        int SnapWorldAxis(Vector2 worldPos, int axis)
        {
            float pos = worldPos[axis];
            int snap = pixelSize[axis];
            return Mathf.RoundToInt(pos / snap) * snap;
        }
    }
}