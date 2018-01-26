using Assets.Scripts.AudioReactiveEffects.Base;
using UnityEngine;

namespace Assets.Scripts.AudioReactiveEffects
{
    public class ParticleSystemStartSpeedReactiveEffect : VisualizationEffectBase
    {
        private ParticleSystem _particleSystem;
        private ParticleSystem.MainModule _mainModule;

        public override void Start()
        {
            base.Start();

            _particleSystem = GetComponent<ParticleSystem>();
            _mainModule = _particleSystem.main;
        }

        public void Update()
        {
            _mainModule.startSpeed = GetAudioData() * PrimaryScaleFactor;
        }
    }
}