using Assets.Scripts.AudioReactiveEffects.Base;
using UnityEngine;

namespace Assets.Scripts.AudioReactiveEffects
{
    public class ParticleSystemEnableEmissionReactiveEffect : VisualizationEffectBase
    {
        private ParticleSystem _particleSystem;
        private ParticleSystem.EmissionModule _emissionModule;

        public override void Start()
        {
            base.Start();

            _particleSystem = GetComponent<ParticleSystem>();
            _emissionModule = _particleSystem.emission;
        }

        public void Update()
        {
            _emissionModule.enabled = GetAudioData() > PrimaryScaleFactor;
        }
    }
}