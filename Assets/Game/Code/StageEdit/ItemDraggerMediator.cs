using System.Collections;
using UnityEngine;
using Xunity.Behaviours;
using Xunity.ScriptableReferences;

namespace Game.StageEdit
{
    public class ItemDraggerMediator : GameBehaviour
    {
        [SerializeField] StageEditManager stageEditManager;
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

            var pos = stageEditManager.GetWorldMouseSnapPosition();
            if (!stageEditManager.TryGetItem(pos, out item))
                return;

            StartCoroutine(CheckHoldAfterDelay(pos));
        }

        IEnumerator CheckHoldAfterDelay(Vector2Int pos)
        {
            var mousePos = stageEditManager.GetWorldMousePosition();
            yield return new WaitForSeconds(holdDelay);
            if (Vector3.Magnitude(mousePos - stageEditManager.GetWorldMousePosition()) > mouseMoveTolerance)
                yield break;

            stageEditManager.RemoveItem(pos);
            itemDragger.DragItem(item);
        }
    }
}