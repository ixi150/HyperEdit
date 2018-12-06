using UnityEngine;
using Xunity.Behaviours;
using Xunity.ScriptableReferences;
using Xunity.ScriptableVariables;

namespace Game
{
    public class OnEnableSetFloat : GameBehaviour
    {
        [SerializeField] FloatVariable target;
        [SerializeField] FloatReference value;

        void OnEnable()
        {
            target.Set(value, this);
        }
    }
}