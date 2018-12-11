using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Xunity.Behaviours;
using Xunity.ScriptableReferences;

namespace Game.Code.StageCreation
{
    public class InitializeSaveSlots : GameBehaviour
    {
        [SerializeField] IntReference slotCount;

        Dropdown dropdown;

        protected override void Awake()
        {
            base.Awake();
            GetComponentIfNull(ref dropdown);

            dropdown.ClearOptions();
            dropdown.AddOptions(IterateOptions().ToList());
        }

        IEnumerable<string> IterateOptions()
        {
            for (var i = 0; i < slotCount; i++)
                yield return "Slot " + i;
        }
    }
}