using Assets.Scripts.AudioReactiveEffects.Base;
using UnityEngine;

namespace Assets.Scripts.AudioReactiveEffects
{
    public class MaterialColorIntensityReactiveEffect : VisualizationEffectBase
    {
        #region Private Member Variables

        private Material _material;
        private Color _initialColor;
        private Color _initialEmissionColor;

        #endregion

        #region Public Properties

        public float MinIntensity;
        public float IntensityScale;
        public float MinEmissionIntensity;
        public float EmissionIntensityScale;

        #endregion

        #region Startup / Shutdown

        public override void Start()
        {
            base.Start();

            _material = GetComponent<Renderer>().material;
            _initialColor = _material.GetColor("_Color");
            _initialEmissionColor = _material.GetColor("_EmissionColor");
        }

        #endregion

        #region Render

        public void Update()
        {
            float audioData = GetAudioData();
            float scaledAmount = Mathf.Clamp(MinIntensity + (audioData * IntensityScale), 0.0f, 1.0f);
            float scaledEmissionAmount = Mathf.Clamp(MinEmissionIntensity + (audioData * EmissionIntensityScale), 0.0f, 1.0f);
            Color scaledColor = _initialColor * scaledAmount;
            Color scaledEmissionColor = _initialEmissionColor * scaledEmissionAmount;

            _material.SetColor("_Color", scaledColor);
            _material.SetColor("_EmissionColor", scaledEmissionColor);
        }

        #endregion
    }
}