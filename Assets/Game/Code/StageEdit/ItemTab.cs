using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Xunity.Behaviours;

namespace Game.StageEdit
{
    public class ItemTab : GameBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] Image icon;
        [SerializeField] Color activeColor, inactiveColor;
        
        bool isActive;
        Image bg;
        ItemCategory itemCategory;

        public event Action<ItemTab> OnClicked = delegate(ItemTab tab) { };

        public ItemCategory ItemCategory
        {
            get { return itemCategory; }
            set
            {
                itemCategory = value;
                icon.sprite = value.Icon;
            }
        }

        public bool IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;
                bg.color = value ? activeColor : inactiveColor;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            GetComponentIfNull(ref bg);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnClicked(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            
        }
    }
}