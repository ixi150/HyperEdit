using System.Collections.Generic;
using Game.StageEdit;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Xunity.Behaviours;
using Xunity.Extensions;

namespace Game
{
    public class ItemButton : GameBehaviour, IPointerDownHandler
    {
        [SerializeField] Image icon;
        [SerializeField] StageEditManager stageEditManager;
        [SerializeField] ItemDragger itemDragger;

        ItemData itemData;
        SpriteRenderer draggedObjectRenderer;

        public ItemData ItemData
        {
            get { return itemData; }
            set
            {
                itemData = value;
                icon.sprite = itemData.icon;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (itemDragger.IsDragging)
                return;
            itemDragger.DragItem(Instantiate(itemData.prefab));
        }
    }
}