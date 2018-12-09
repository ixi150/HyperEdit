using UnityEngine;
using Xunity.Behaviours;

namespace Game
{
	public class CopyPosition : GameBehaviour
	{
		[SerializeField] Transform target;

		void Update()
		{
			Position = target.position;
		}
	}
}
