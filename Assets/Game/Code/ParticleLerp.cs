using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Xunity.LerpEffects;
using Xunity.Sets;
using Particle = UnityEngine.ParticleSystem.Particle;

namespace Game.Code
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleLerp : LerpEffect
    {
        const int INIT_ARRAY_SIZE = 100;
        const float SCALE = .5f;

        [SerializeField] SetCollection target;

        [FormerlySerializedAs("origin")] [SerializeField]
        ParticleSystem originPs;

        [FormerlySerializedAs("target")] [SerializeField]
        ParticleSystem targetPs;

        ParticleSystem myPs;
        readonly Particle[] myParticles = new Particle[INIT_ARRAY_SIZE];
        readonly Particle[] originParticles = new Particle[INIT_ARRAY_SIZE];
        readonly Particle[] targetParticles = new Particle[INIT_ARRAY_SIZE];

        Vector3 originOffset;
        Vector3 targetOffset;
        float targetMultiplier;

        public Transform Target
        {
            get { return target.Items.FirstOrDefault(); }
        }

        public override void Refresh()
        {
            GetComponentIfNull(ref myPs);
            int count = GetParticles(myPs, myParticles);
            int originCount = GetParticles(originPs, originParticles);
            int targetCount = GetParticles(targetPs, targetParticles);

            originOffset = GetOffset(originPs);
            if (Target)
            {
                targetOffset = Target.position;
                targetMultiplier = SCALE * Target.lossyScale.x;
            }

            for (var i = 0; i < count; i++)
            {
                myParticles[i].position = Vector3.LerpUnclamped(
                    originParticles[i].position + originOffset,
                    targetParticles[i].position * targetMultiplier + targetOffset,
                    Lerp);
            }

            myPs.SetParticles(myParticles, count);
        }

        void Update()
        {
            Refresh();
        }

        int GetParticles(ParticleSystem ps, Particle[] array)
        {
            int amount = ps.GetParticles(array);
            if (array.Length < amount)
                Array.Resize(ref array, amount);
            return amount;
        }

        Vector3 GetOffset(ParticleSystem ps)
        {
            return ps.main.simulationSpace == ParticleSystemSimulationSpace.Local
                ? ps.transform.position
                : Vector3.zero;
        }
    }
}