using System.Collections.Generic;
using UnityEngine;
using Xunity.Behaviours;

namespace Game.Code
{
    public class GameModeManager : GameBehaviour
    {
        [SerializeField] GameObject[] editModeObjects;
        [SerializeField] GameObject[] playModeObjects;

        IEnumerable<GameObject[]> AllContainers
        {
            get
            {
                yield return editModeObjects;
                yield return playModeObjects;
            }
        }


        public void EntedPlayMode()
        {
            EnterMode(playModeObjects);
        }

        public void EnterEditMode()
        {
            EnterMode(editModeObjects);
        }


        void EnterMode(GameObject[] activeContainer)
        {
            foreach (var container in AllContainers)
            foreach (var go in container)
                go.SetActive(container == activeContainer);
        }
    }
}