using Assets.Scripts.Audio;
using UnityEngine;

namespace Assets.Scripts.AudioReactiveEffects.Base
{
    public abstract class VisualizationEffectBase : MonoBehaviour
    {
        #region Private Member Variables

        private LoopbackAudio _loopbackAudio;

        #endregion

        #region Protected Properties

        protected LoopbackAudio LoopbackAudio { get { return _loopbackAudio; } }

        #endregion

        #region Public Properties

        public AudioVisualizationStrategy AudioVisualizationStrategy;
        public int AudioSampleIndex;
        public float PrimaryScaleFactor;

        #endregion

        #region Startup / Shutdown

        public virtual void Start()
        {
            // References and setup
            _loopbackAudio = FindObjectOfType<LoopbackAudio>();
        }

        #endregion

        #region Protected Methods

        protected float GetAudioData()
        {
            // Get audio data
            return GetAudioData(AudioSampleIndex);
        }

        protected float GetAudioData(int index)
        {
            // Get audio data
            return GetAudioData(AudioVisualizationStrategy, index);
        }

        protected float GetAudioData(AudioVisualizationStrategy audioVisualizationStrategy, int index)
        {
            // Get audio data
            return _loopbackAudio.GetSpectrumData(audioVisualizationStrategy, index);
        }

        #endregion
    }
}
