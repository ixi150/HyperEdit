using System.Collections.Generic;
using UnityEngine;

namespace Game.Code.StageCreation
{
    [CreateAssetMenu]
    public class StageData : ScriptableObject
    {
        [SerializeField] RectInt bounds;
        [SerializeField] int width = 10;
        [SerializeField] List<StageItemData> items = new List<StageItemData>();

        public RectInt Bounds
        {
            get { return bounds; }
        }

        public IEnumerable<StageItemData> Items
        {
            get { return items; }
        }
        
        public void ClearItems()
        {
            items.Clear();
        }

        public void AddItem(Vector2Int coord, string id)
        {
            items.Add(new StageItemData(){coord = coord, id = id});
        }
        
        [System.Serializable]
        public class StageItemData
        {
            public Vector2Int coord;
            public string id;
        }
    }
}