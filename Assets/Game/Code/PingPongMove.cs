using UnityEngine;
using Xunity.Behaviours;
using Xunity.ScriptableReferences;

namespace Game
{
    public class PingPongMove : GameBehaviour
    {
        [SerializeField] FloatReference speed;
        [SerializeField] FloatReference amplitude;

        Vector3 originalPosition;
        float randomOffset;
        float randomSpeed;

        void OnEnable()
        {
            originalPosition = Position;
            randomOffset = Random.Range(0, Mathf.PI * 2);
            randomSpeed = Random.Range(0.95f, 1.05f);
        }

        void FixedUpdate()
        {
            var pos = originalPosition;
            pos.y += Mathf.Sin(Mathf.PI * 2 * speed * randomSpeed * Time.time + randomOffset) * amplitude;
            Position = pos;
        }
    }
}