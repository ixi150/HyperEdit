using System.Collections.Generic;
using Game.StageEdit;
using UnityEngine;
using Xunity.Behaviours;
using Xunity.Extensions;

namespace Game
{
    [RequireComponent(typeof(MeshRenderer))]
    public class DynamicTexture : GameBehaviour
    {
        [SerializeField] StageEditManager stageEditor;
        [SerializeField] Color color;
        [SerializeField] Color colorAdd;
        [SerializeField] Vector2Int size;


        //List<Vector2Int> worldPositions = new List<Vector2Int>();

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
                //worldPositions.Add(GetWorldMouseSnapPosition());
            }

            RebuildTexture();
        }

        void SetScale()
        {
            Vector2 scale = size;
            scale.Scale(stageEditor.PixelSize);
            transform.localScale = scale;
        }

        void RebuildTexture()
        {
            var offcenter = new Vector2Int(Mathf.FloorToInt(size.x / 2f), Mathf.FloorToInt(size.y / 2f)) * -1;
            var positionInt = stageEditor.SnapPosition(Position);

            tex.Resize(size.x, size.y);
            var pixels = tex.GetPixels();
            for (var y = 0; y < tex.height; y++)
            for (var x = 0; x < tex.width; x++)
            {
                var pixelPos = new Vector2Int(x, y);
                var relativePos = pixelPos + offcenter;
                var offset = Vector2Int.Scale(relativePos, stageEditor.PixelSize);
                var worldPixelPos = offset + positionInt;

                int i = y * tex.width + x;
                //pixels[i] = stageEditor.GetWorldMouseSnapPosition() == worldPixelPos ? colorAdd : color;
                pixels[i] = stageEditor.IsPointInPlacablePosition(worldPixelPos.ToVector2())
                    ? Color.green / 4
                    : Color.red   / 4;
            }

            tex.SetPixels(pixels);
            tex.Apply();
        }

        void CreateNewTexture()
        {
            tex = new Texture2D(size.x, size.y) {filterMode = FilterMode.Point};
            meshRenderer.material.mainTexture = tex;
            meshRenderer.enabled = true;
        }
    }
}