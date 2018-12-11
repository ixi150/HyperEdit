using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Xunity.Extensions;
using Xunity.ScriptableEvents;

namespace Game.Code.StageCreation
{
    [CreateAssetMenu]
    public class ItemDragger : ScriptableObject
    {
        [FormerlySerializedAs("stageEditManager")] [SerializeField] StageCreationManager stageCreationManager;
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
            var draggedObjectSortingLayer = 0;
            var draggedObjectRenderer = draggedItem.GetComponentInChildren<SpriteRenderer>();
            if (draggedObjectRenderer)
            {
                draggedObjectSortingLayer = draggedObjectRenderer.sortingLayerID;
                draggedObjectRenderer.sortingLayerID = layerWhileDragging.id;
            }

            foreach (var mono in draggedItem.GetComponentsInChildren<PingPongMove>())
                mono.enabled = false;

            while (Input.GetMouseButton(0))
            {
                draggedItem.Position = stageCreationManager.GetWorldMousePosition();
                if (stageCreationManager.IsItemInPlacablePosition(draggedItem))
                    draggedItem.Position = stageCreationManager.GetWorldMouseSnapPosition().ToVector2();
                yield return null;
            }

            if (stageCreationManager.IsItemInPlacablePosition(draggedItem))
            {
                if (draggedObjectRenderer)
                    draggedObjectRenderer.sortingLayerID = draggedObjectSortingLayer;
                foreach (var mono in draggedItem.GetComponentsInChildren<PingPongMove>())
                    mono.enabled = true;
                stageCreationManager.AddItem(draggedItem);
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