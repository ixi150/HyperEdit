using UnityEngine;
using Xunity.Behaviours;

namespace Game
{
    public class FlipScale : GameBehaviour
    {
        Vector3 initialScale;

        protected override void Awake()
        {
            base.Awake();
            initialScale = transform.localScale;
        }

        void Update()
        {
            var scale = initialScale;
            if (Position.y > 0)
                scale.y *= -1;
            transform.localScale = scale;
        }
    }
}