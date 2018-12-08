using System.Collections.Generic;
using UnityEngine;
using Xunity.Behaviours;
using Xunity.Extensions;

namespace Game
{
    [RequireComponent(typeof(MeshRenderer))]
    public class DynamicTexture : GameBehaviour
    {
        [SerializeField] Color color;
        [SerializeField] Color colorAdd;
        [SerializeField] Vector2Int size;
        [SerializeField] Vector2Int pixelSize = new Vector2Int(2, 2);
        List<Vector2Int> worldPositions = new List<Vector2Int>();

        MeshRenderer meshRenderer;
        Texture2D tex;

        protected override void Awake()
        {
            base.Awake();
            GetComponentIfNull(ref meshRenderer);
            CreateNewTexture();
        }

        void LateUpdate()
        {
            SetScale();

            if (Input.GetMouseButtonDown(0))
            {
                worldPositions.Add(GetWorldMouseSnapPosition());
            }

            RebuildTexture();
        }

        void SetScale()
        {
            Vector2 scale = size;
            scale.Scale(pixelSize);
            transform.localScale = scale;
        }

        void RebuildTexture()
        {
            var offcenter = new Vector2Int(Mathf.FloorToInt(size.x / 2f), Mathf.FloorToInt(size.y / 2f)) * -1;
            var positionInt = SnapPosition(Position);

            tex.Resize(size.x, size.y);
            var pixels = tex.GetPixels();
            for (var y = 0; y < tex.height; y++)
            for (var x = 0; x < tex.width; x++)
            {
                var pixelPos = new Vector2Int(x, y);
                var relativePos = pixelPos + offcenter;
                var offset = Vector2Int.Scale(relativePos, pixelSize);
                var worldPixelPos = offset + positionInt;

                int i = y * tex.width + x;
                pixels[i] = worldPositions.Contains(worldPixelPos)
                            || GetWorldMouseSnapPosition() == worldPixelPos
                    ? colorAdd
                    : color;
            }


            tex.SetPixels(pixels);
            tex.Apply();
        }

        void CreateNewTexture()
        {
            tex = new Texture2D(size.x, size.y) {filterMode = FilterMode.Point}; //, TextureFormat.RGBA32,  );
            meshRenderer.material.mainTexture = tex;
            meshRenderer.enabled = true;
        }


        Vector2Int GetWorldMouseSnapPosition()
        {
            return SnapPosition(GetWorldMousePosition());
        }

        Vector2Int SnapPosition(Vector2 pos)
        {
            return new Vector2Int(
                SnapWorldAxis(pos, pixelSize, 0),
                SnapWorldAxis(pos, pixelSize, 1)
            );
        }

        static int SnapWorldAxis(Vector2 worldPos, Vector2Int pixelSize, int axis)
        {
            float pos = worldPos[axis];
            int snap = pixelSize[axis];
            return Mathf.RoundToInt(pos / snap) * snap;
        }

        static Vector3 GetWorldMousePosition()
        {
            var cam = Camera.main;
            var mousePos = Input.mousePosition.Modified(z: -cam.transform.position.z);
            return cam.ScreenToWorldPoint(mousePos);
        }
    }
}