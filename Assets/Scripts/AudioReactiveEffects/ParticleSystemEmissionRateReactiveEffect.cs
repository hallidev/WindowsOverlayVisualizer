using Assets.Scripts.AudioReactiveEffects.Base;
using UnityEngine;

namespace Assets.Scripts.AudioReactiveEffects
{
    public class ParticleSystemEmissionRateReactiveEffect : VisualizationEffectBase
    {
        private ParticleSystem _particleSystem;
        private ParticleSystem.EmissionModule _emissionModule;
        private ParticleSystem.MinMaxCurve _emissionRate;

        public override void Start()
        {
            base.Start();

            _particleSystem = GetComponent<ParticleSystem>();
            _emissionModule = _particleSystem.emission;
            _emissionRate = _particleSystem.emission.rateOverTime;
        }

        public void Update()
        {
            int rate = (int)(GetAudioData() * PrimaryScaleFactor);
            _emissionRate.constant = rate;
            _emissionRate.constantMax = rate;
            _emissionRate.constantMin = rate;
            _emissionModule.rateOverTime = _emissionRate;
        }
    }
}