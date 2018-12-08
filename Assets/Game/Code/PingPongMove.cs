using UnityEngine;
using Xunity.Behaviours;
using Xunity.ScriptableReferences;

namespace Game
{
	public class PingPongMove : GameBehaviour
	{
		[SerializeField] FloatReference speed;
		[SerializeField] FloatReference amplitude;

		Vector3 originalPosition;

		void OnEnable()
		{
			originalPosition = Position;
		}

		void FixedUpdate()
		{
			var pos = originalPosition;
			pos.y += Mathf.Sin(Mathf.PI * 2 * speed * Time.time) * amplitude;
			Position = pos;
		}
	}
}
