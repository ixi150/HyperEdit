using UnityEngine;

namespace Game.Code.StageCreation
{
	[CreateAssetMenu]
	public class ItemData : ScriptableObject
	{
		public ItemCategory category;
		public Sprite icon;
		public Item prefab;
	}
}
