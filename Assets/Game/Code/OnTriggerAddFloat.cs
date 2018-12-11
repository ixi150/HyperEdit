using UnityEngine;
using Xunity.Behaviours;
using Xunity.ScriptableReferences;
using Xunity.ScriptableVariables;

namespace Game.Code
{
    public class OnTriggerAddFloat : GameBehaviour
    {
        [SerializeField] string triggerTag = "Player";
        [SerializeField] FloatVariable target;
        [SerializeField] FloatReference amount;
        [SerializeField] BoolReference multipleUses = BoolReference.New(true, false);

        void OnTriggerEnter(Collider other)
        {
            OnTrigger(other.transform);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            OnTrigger(other.transform);
        }

        void OnTrigger(Component other)
        {
            if (!other.CompareTag(triggerTag)) 
                return;
            
            target.Set(target + amount, this);
            enabled = multipleUses;
        }
    }
}