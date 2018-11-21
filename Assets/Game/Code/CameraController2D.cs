using UnityEngine;
using Xunity.Behaviours;
using Xunity.ReferenceVariables;

namespace Game
{
    public class CameraController2D : CameraController
    {
        [SerializeField] FloatReference rbVelocityMultiplier = FloatReference.New(true, 1);

        Rigidbody2D rb2D;

        protected override Vector3 TargetOffset
        {
            get { return base.TargetOffset + VelocityOffset; }
        }

        Vector3 VelocityOffset
        {
            get
            {
                if (rb2D == null && Target)
                    rb2D = Target.GetComponent<Rigidbody2D>();

                return rb2D ? (Vector3) rb2D.velocity * rbVelocityMultiplier : default(Vector3);
            }
        }

        protected override void Awake()
        {
            base.Awake();

            camera.opaqueSortMode = UnityEngine.Rendering.OpaqueSortMode.NoDistanceSort;
            camera.transparencySortMode = TransparencySortMode.Orthographic;
        }
    }
}