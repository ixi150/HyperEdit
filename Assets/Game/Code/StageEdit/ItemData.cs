using UnityEngine;

namespace Game
{
	[CreateAssetMenu]
	public class ItemData : ScriptableObject
	{
		public ItemCategory category;
		public Sprite icon;
		public Item prefab;
	}
}
