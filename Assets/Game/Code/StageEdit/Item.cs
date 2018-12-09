using System;
using System.Collections;
using UnityEngine;
using Xunity.Attributes;
using Xunity.Behaviours;

namespace Game.StageEdit
{
    public class Item : GameBehaviour
    {
        [System.Flags]
        public enum PlacementRule
        {
            Ceil = 1,
            Mid = 2,
            Floor = 4,
        }

        [SerializeField] string id;
        [SerializeField] [EnumFlags] PlacementRule placement = (PlacementRule) (-1);
        [SerializeField] RectInt size = new RectInt(0,0,1,1);

        public string Id
        {
            get { return id; }
        }

        public PlacementRule Placement
        {
            get { return placement; }
        }

        public RectInt Size
        {
            get { return size; }
        }
    }
}