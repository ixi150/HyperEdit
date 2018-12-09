using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Xunity.Behaviours;
using Xunity.Extensions;

namespace Game
{
    public class CameraDrag : GameBehaviour
    {
        [SerializeField] Rect moveArea;
        [SerializeField] float damping = 1;
        [SerializeField] float dragAmount = 1;

        bool isMouseDown;
        new Camera camera;
        Transform cameraTransform;
        Vector3 mouseDownCameraPosition;

        Vector3 mouseDownMousePosition;
        Vector3 lastMousePosition;

        Vector2 dragVelocity;
        GraphicRaycaster raycaster;

        public void InteruptDragging()
        {
            isMouseDown = false;
        }
        
        protected override void Awake()
        {
            base.Awake();
            camera = GetComponentInChildren<Camera>();
            cameraTransform = camera.transform;
        }

        void Start()
        {
            raycaster = FindObjectOfType<GraphicRaycaster>();
        }

        Vector3 GetWorldMousePosition()
        {
            var mousePos = Input.mousePosition.Modified(z: -camera.transform.position.z);
            return camera.ScreenToWorldPoint(mousePos);
        }

        Vector3 GetVieportMousePosition()
        {
            var mousePos = Input.mousePosition.Modified(z: -camera.transform.position.z);
            return camera.ScreenToViewportPoint(mousePos);
        }

        bool IsUiClicked()
        {
            var pointerData = new PointerEventData(EventSystem.current);
            var results = new List<RaycastResult>();
            pointerData.position = Input.mousePosition;
            raycaster.Raycast(pointerData, results);
            return results.Count > 0;
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
            camera.transform.position += (Vector3) (dragVelocity * Time.deltaTime);

            if (Input.GetMouseButtonDown(0) && !IsUiClicked())
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