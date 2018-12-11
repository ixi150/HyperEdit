using System.Collections.Generic;
using UnityEngine;
using Xunity.Behaviours;

namespace Game.Code.StageCreation
{
    public class ItemButtonsManager : GameBehaviour
    {
        [SerializeField] ItemCategory activeCategory;
        [SerializeField] ItemButton buttonPrefab;
        [SerializeField] ItemTab tabPrefab;
        [SerializeField] Transform tabTransform;

        readonly List<ItemButton> buttons = new List<ItemButton>();
        readonly List<ItemTab> tabs = new List<ItemTab>();

        void Start()
        {
            var categories = Resources.LoadAll<ItemCategory>("");
            foreach (var cat in categories)
            {
                var tab = Instantiate(tabPrefab, tabTransform);
                tabs.Add(tab);
                tab.ItemCategory = cat;
                tab.IsActive = cat == activeCategory;
                tab.OnClicked += OnCategoryClicked;
            }
            
            var itemDatas = Resources.LoadAll<ItemData>("");
            foreach (var itemData in itemDatas)
            {
                var button = Instantiate(buttonPrefab, transform);
                buttons.Add(button);
                button.ItemData = itemData;
                button.gameObject.SetActive(button.ItemData.category == activeCategory);
            }
        }

        void OnCategoryClicked(ItemTab clickedTab)
        {
            activeCategory = clickedTab.ItemCategory;
            foreach (var button in buttons)
                button.gameObject.SetActive(button.ItemData.category == activeCategory);
            foreach (var tab in tabs)
                tab.IsActive = tab.ItemCategory == activeCategory;
        }
    }
}