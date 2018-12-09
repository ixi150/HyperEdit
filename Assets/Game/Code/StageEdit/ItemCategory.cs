using UnityEngine;

namespace Game
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