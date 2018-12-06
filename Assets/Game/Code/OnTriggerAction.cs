using UnityEngine;
using UnityEngine.Events;
using Xunity.Behaviours;
using Xunity.ScriptableReferences;
using Xunity.ScriptableVariables;

namespace Game
{
    public class OnTriggerAction : GameBehaviour
    {
        [SerializeField] string triggerTag = "Player";
        [SerializeField] BoolReference multipleUses = BoolReference.New(true, false);
        [SerializeField] UnityEvent action;

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
            
            action.Invoke();
            enabled = multipleUses;
        }
    }
}