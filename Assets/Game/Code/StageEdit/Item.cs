using UnityEngine;
using Xunity.Behaviours;

namespace Game
{
    public class Item : GameBehaviour
    {
        [SerializeField] string id;

        public string Id
        {
            get { return id; }
        }
    }
}