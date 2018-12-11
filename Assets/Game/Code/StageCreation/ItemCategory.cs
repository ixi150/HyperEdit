using UnityEngine;

namespace Game.Code.StageCreation
{
    [CreateAssetMenu]
    public class ItemCategory : ScriptableObject
    {
        [SerializeField] Sprite icon;

        public Sprite Icon
        {
            get { return icon; }
        }
    }
}