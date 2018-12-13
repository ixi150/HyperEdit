using System.Collections.Generic;
using UnityEngine;
using Xunity.Behaviours;

namespace Game.Code.StageCreation
{
    public class ItemButtonsManager : GameBehaviour
    {
        [SerializeField] ItemCategory activeCategory;
        [SerializeField] ItemButton buttonPrefab;

        readonly List<ItemButton> buttons = new List<ItemButton>();
        readonly List<ItemTab> tabs = new List<ItemTab>();
        ItemCategory[] categories;
        int categoryIndex;

        public int CategoryIndex
        {
            get { return categoryIndex; }
            set
            {
                categoryIndex = (int)Mathf.Repeat(value,categories.Length);
            }
        }
        
        public void NextCategory()
        {
            CategoryIndex++;
            OnCategoryChange();
        }

        public void PrevCategory()
        {
            CategoryIndex--;
            OnCategoryChange();
        }


        
        void Start()
        {
            categories = Resources.LoadAll<ItemCategory>("");
            var itemDatas = Resources.LoadAll<ItemData>("");

            foreach (var itemData in itemDatas)
            {
                var button = Instantiate(buttonPrefab, transform);
                buttons.Add(button);
                button.ItemData = itemData;
                button.gameObject.SetActive(button.ItemData.category == activeCategory);
            }
            
            OnCategoryChange();
        }

        void OnCategoryChange()
        {
            activeCategory = categories[CategoryIndex];
            foreach (var button in buttons)
                button.gameObject.SetActive(button.ItemData.category == activeCategory);
            foreach (var tab in tabs)
                tab.IsActive = tab.ItemCategory == activeCategory;
        }
    }
}