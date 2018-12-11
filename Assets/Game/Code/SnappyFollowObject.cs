using System.Linq;
using UnityEngine;
using Xunity.Behaviours;
using Xunity.Extensions;
using Xunity.ScriptableReferences;
using Xunity.Sets;

namespace Game.Code
{
    public class SnappyFollowObject : GameBehaviour
    {
        [SerializeField] SetCollection targetReference;
        [SerializeField] Vector3Reference snap;

        Vector3 Snap
        {
            get { return snap; }
        }

        void Update()
        {
            var target = targetReference.Items.NotNull().FirstOrDefault();
            if (target == null)
                return;

            var pos = target.position;
            Position = new Vector3
            (
                SnapAxis(pos.x, 0),
                SnapAxis(pos.y, 1),
                SnapAxis(pos.z, 2)
            );
        }

        float SnapAxis(float value, int axisIndex)
        {
            float s = Snap[axisIndex];
            if (s == 0) 
                return value;
            if (s < 0)
                return Position[axisIndex];

            int count = Mathf.FloorToInt(value / s);
            return count * s;
        }
    }
}