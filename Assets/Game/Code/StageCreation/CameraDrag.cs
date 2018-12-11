using UnityEngine;
using UnityEngine.Serialization;
using Xunity.Behaviours;
using Xunity.Extensions;

namespace Game.Code.StageCreation
{
    public class CameraDrag : GameBehaviour
    {
        [SerializeField] Rect moveArea;
        [SerializeField] float damping = 1;
        [SerializeField] float dragAmount = 1;
        [FormerlySerializedAs("stageEditManager")] [SerializeField] StageCreationManager stageCreationManager;

        bool isMouseDown;
        new Camera camera;
        Transform cameraTransform;
        Vector3 mouseDownCameraPosition;
        Vector3 mouseDownMousePosition;
        Vector3 lastMousePosition;

        Vector2 dragVelocity;

        public void InteruptDragging()
        {
            isMouseDown = false;
        }
        
        protected override void Awake()
        {
            base.Awake();
            camera = GetComponentInChildren<Camera>();
            if (camera == null)
                camera = Camera.main;
            if (camera == null)
                return;
            cameraTransform = camera.transform;
        }

        Vector3 GetWorldMousePosition()
        {
            var mousePos = Input.mousePosition.Modified(z: -cameraTransform.position.z);
            return camera.ScreenToWorldPoint(mousePos);
        }

        Vector3 GetVieportMousePosition()
        {
            var mousePos = Input.mousePosition.Modified(z: -cameraTransform.position.z);
            return camera.ScreenToViewportPoint(mousePos);
        }

   
        void Update()
        {
            if (isMouseDown)
                UpdateHoldingMouse();
            else
                UpdateWaitingForMouse();

            LockCameraInBoundingBox();
        }

        void LockCameraInBoundingBox()
        {
            var pos = cameraTransform.position;
            pos.x = Mathf.Min(pos.x, moveArea.xMax);
            pos.x = Mathf.Max(pos.x, moveArea.xMin);
            pos.y = Mathf.Min(pos.y, moveArea.yMax);
            pos.y = Mathf.Max(pos.y, moveArea.yMin);
            cameraTransform.position = pos;
        }

        void UpdateHoldingMouse()
        {
            var currentMousePosition = GetVieportMousePosition();
            dragVelocity = (lastMousePosition - currentMousePosition) * dragAmount * 10;

            if (Input.GetMouseButtonUp(0))
            {
                isMouseDown = false;
                return;
            }

            lastMousePosition = currentMousePosition;

            Vector3 offset = mouseDownMousePosition - currentMousePosition;
            cameraTransform.position = mouseDownCameraPosition + offset * dragAmount;
        }

        void UpdateWaitingForMouse()
        {
            dragVelocity = Vector2.MoveTowards(dragVelocity, Vector2.zero, Time.deltaTime * damping);
            cameraTransform.position += (Vector3) (dragVelocity * Time.deltaTime);

            if (Input.GetMouseButtonDown(0) && !stageCreationManager.IsMouseOverUi())
            {
                isMouseDown = true;
                lastMousePosition = mouseDownMousePosition = GetVieportMousePosition();
                mouseDownCameraPosition = cameraTransform.position;
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(moveArea.center, moveArea.size);
        }
    }
}