using System.Collections;
using UnityEngine;
using Xunity.Extensions;
using Xunity.ScriptableEvents;

namespace Game.StageEdit
{
    [CreateAssetMenu]
    public class ItemDragger : ScriptableObject
    {
        [SerializeField] StageEditManager stageEditManager;
        [SerializeField] GameEvent onItemDragBegin;

        SortingLayer layerWhileDragging;

        public bool IsDragging { get; private set; }

        public void DragItem(Item itemToDrag)
        {
            if (IsDragging)
                return;
            
            onItemDragBegin.Raise();
            itemToDrag.StartCoroutine(DragYourself(itemToDrag));
        }

        IEnumerator DragYourself(Item draggedItem)
        {
            //IsDragging = true;
            var draggedObjectRenderer = draggedItem.GetComponentInChildren<SpriteRenderer>();
            int draggedObjectSortingLayer = draggedObjectRenderer.sortingLayerID;
            draggedObjectRenderer.sortingLayerID = layerWhileDragging.id;
            foreach (var mono in draggedItem.GetComponentsInChildren<PingPongMove>())
                mono.enabled = false;

            while (Input.GetMouseButton(0))
            {
                draggedItem.Position = stageEditManager.GetWorldMousePosition();
                if (stageEditManager.IsItemInPlacablePosition(draggedItem))
                    draggedItem.Position = stageEditManager.GetWorldMouseSnapPosition().ToVector2();
                yield return null;
            }

            if (stageEditManager.IsItemInPlacablePosition(draggedItem))
            {
                draggedObjectRenderer.sortingLayerID = draggedObjectSortingLayer;
                foreach (var mono in draggedItem.GetComponentsInChildren<PingPongMove>())
                    mono.enabled = true;
                stageEditManager.AddItem(draggedItem);
            }
            else
            {
                Destroy(draggedItem.gameObject);
            }
            IsDragging = false;
        }

        void UpdateWaitingForDrag()
        {
            if (!Input.GetMouseButtonDown(0))
                return;

            
        }
    }
}