using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Xunity.Behaviours;

namespace Game.Code.StageCreation
{
    public class ItemButton : GameBehaviour, IPointerDownHandler
    {
        [SerializeField] Image icon;
        [FormerlySerializedAs("stageEditManager")] [SerializeField] StageCreationManager stageCreationManager;
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