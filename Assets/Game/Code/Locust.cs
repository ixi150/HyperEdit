using System.Collections;
using UnityEngine;
using Xunity.Behaviours;
using Xunity.ScriptableReferences;
using Xunity.ScriptableVariables;

namespace Game
{
    public class Locust : GameBehaviour
    {
        const string TAG = "Player";

        [SerializeField] ParticleLerp particleLerp;
        [SerializeField] AnimationCurve animationCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] FloatReference duration;
        [SerializeField] FloatReference planktonReducePerSecond;
        [SerializeField] FloatVariable planktonValue;

        bool isChasing;
        float elapsed;

        void Update()
        {
            elapsed += Time.deltaTime * (isChasing ? 1 : -1);
            elapsed = Mathf.Clamp(elapsed, 0, duration);
            particleLerp.Lerp = animationCurve.Evaluate(elapsed / duration);
            if (particleLerp.Lerp >= 1)
            {
                planktonValue.Set(planktonValue - planktonReducePerSecond * Time.deltaTime, this);
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(TAG))
            {
                isChasing = true;
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(TAG))
            {
                isChasing = false;
            }
        }
    }
}