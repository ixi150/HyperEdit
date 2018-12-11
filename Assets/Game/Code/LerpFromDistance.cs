using UnityEngine;
using Xunity.Behaviours;
using Xunity.LerpEffects;
using Xunity.ScriptableReferences;

namespace Game.Code
{
	public class LerpFromDistance :GameBehaviour
	{
		[SerializeField] LerpEffect lerpEffect;
		[SerializeField] FloatReference min, max;
		
		Transform cameraTransform;

		void Start ()
		{
			cameraTransform = Camera.main.transform;
		}
	
		void Update ()
		{
			lerpEffect.Lerp = Mathf.InverseLerp(min, max, Vector3.SqrMagnitude(Position - cameraTransform.position));
		}
	}
}
