using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Xunity.Behaviours;
using Xunity.ScriptableReferences;

namespace Game.Code.StageCreation
{
    public class ItemDraggerMediator : GameBehaviour
    {
        [FormerlySerializedAs("stageEditManager")] [SerializeField] StageCreationManager stageCreationManager;
        [SerializeField] ItemDragger itemDragger;
        [SerializeField] FloatReference holdDelay;
        [SerializeField] FloatReference mouseMoveTolerance;

        Item item;

        void LateUpdate()
        {
            if (Input.GetMouseButtonUp(0))
                StopAllCoroutines();
            if (itemDragger.IsDragging || !Input.GetMouseButtonDown(0))
                return;

            var pos = stageCreationManager.GetWorldMouseSnapPosition();
            if (!stageCreationManager.TryGetItem(pos, out item))
                return;

            StartCoroutine(CheckHoldAfterDelay(pos));
        }

        IEnumerator CheckHoldAfterDelay(Vector2Int pos)
        {
            var mousePos = stageCreationManager.GetWorldMousePosition();
            yield return new WaitForSeconds(holdDelay);
            if (Vector3.Magnitude(mousePos - stageCreationManager.GetWorldMousePosition()) > mouseMoveTolerance)
                yield break;

            stageCreationManager.RemoveItem(pos);
            itemDragger.DragItem(item);
        }
    }
}