﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    public class LoopbackAudio : MonoBehaviour
    {
        #region Constants

        private const int EnergyAverageCount = 100;

        #endregion

        #region Private Member Variables

        private RealtimeAudio _realtimeAudio;
        private readonly List<float> _postScaleAverages = new List<float>();

        #endregion

        #region Public Properties

        public int SpectrumSize;
        public ScalingStrategy ScalingStrategy;
        public AnimationCurve PostScaleCurve;
        [Range(0.0f, 40.0f)]
        public float PostScaleMultiplier = 2.0f;
        public float[] SpectrumData;
        public float[] PostScaledSpectrumData;
        public float[] PostScaledMinMaxSpectrumData;
        public float PostScaledMax;
        public float PostScaledEnergy;
        public bool IsIdle;

        public float ThresholdToMin = 1.5f;
        public float MinAmount = 0.0f;
        public float ThresholdToMax = 1.5f;
        public float MaxAmount = 1.0f;

        #endregion

        #region Startup / Shutdown

        public void Awake()
        {
            SpectrumData = new float[SpectrumSize];
            PostScaledSpectrumData = new float[SpectrumSize];
            PostScaledMinMaxSpectrumData = new float[SpectrumSize];

            // Used for post scaling
            float postScaleStep = 1.0f / SpectrumSize;

            // Setup loopback audio and start listening
            _realtimeAudio = new RealtimeAudio(SpectrumSize, ScalingStrategy,(spectrumData) =>
            {
                // Raw
                SpectrumData = spectrumData;

                float postScaledMax = 0.0f;
                float postScaleAverage = 0.0f;
                float totalPostScaledValue = 0.0f;

                bool isIdle = true;

                // Pass 1: Scaled. Scales progressively as moving up the spectrum
                for (int i = 0; i < SpectrumSize; i++)
                {
                    float postScaleValue = PostScaleCurve.Evaluate(postScaleStep * i) * SpectrumData[i] * PostScaleMultiplier;
                    PostScaledSpectrumData[i] = Mathf.Clamp(postScaleValue, 0, RealtimeAudio.MaxAudioValue);

                    if (PostScaledSpectrumData[i] > postScaledMax)
                    {
                        postScaledMax = PostScaledSpectrumData[i];
                    }

                    totalPostScaledValue += PostScaledSpectrumData[i];

                    if (spectrumData[i] > 0)
                    {
                        isIdle = false;
                    }
                }

                PostScaledMax = postScaledMax;

                // Calculate "energy" using the post scale average
                postScaleAverage = totalPostScaledValue / SpectrumSize;
                _postScaleAverages.Add(postScaleAverage);

                // We only want to track EnergyAverageCount averages.
                // With a value of 1000, this will happen every couple seconds
                if (_postScaleAverages.Count == EnergyAverageCount)
                {
                    _postScaleAverages.RemoveAt(0);
                }

                // Average the averages to get the energy.
                PostScaledEnergy = _postScaleAverages.Average();

                // Pass 2: MinMax spectrum. Here we use the average.
                // If a given band falls below the average, reduce it 50%
                // otherwise boost it 50%
                for (int i = 0; i < SpectrumSize; i++)
                {
                    float minMaxed = PostScaledSpectrumData[i];

                    if(minMaxed <= postScaleAverage * ThresholdToMin)
                    {
                        minMaxed *= MinAmount;
                    }
                    else if(minMaxed >= postScaleAverage * ThresholdToMax)
                    {
                        minMaxed *= MaxAmount;
                    }

                    PostScaledMinMaxSpectrumData[i] = minMaxed;
                }

                IsIdle = isIdle;
            });
            _realtimeAudio.StartListen();
        }

        public void Update()
        {
        
        }

        public void OnApplicationQuit()
        {
            _realtimeAudio.StopListen();
        }

        #endregion

        #region Public Methods

        public float[] GetAllSpectrumData(AudioVisualizationStrategy strategy)
        {
            float[] spectrumData;

            switch (strategy)
            {
                case AudioVisualizationStrategy.Raw:
                    spectrumData = SpectrumData;
                    break;
                case AudioVisualizationStrategy.PostScaled:
                    spectrumData = PostScaledSpectrumData;
                    break;
                case AudioVisualizationStrategy.PostScaledMinMax:
                    spectrumData = PostScaledMinMaxSpectrumData;
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Invalid for GetAllSpectrumData: {0}", strategy));
            }

            return spectrumData;
        }

        public float GetSpectrumData(AudioVisualizationStrategy strategy, int index = 0)
        {
            float spectrumData = 0.0f;

            switch (strategy)
            {
                case AudioVisualizationStrategy.Raw:
                    spectrumData = SpectrumData[index];
                    break;
                case AudioVisualizationStrategy.PostScaled:
                    spectrumData = PostScaledSpectrumData[index];
                    break;
                case AudioVisualizationStrategy.PostScaledMinMax:
                    spectrumData = PostScaledMinMaxSpectrumData[index];
                    break;
                case AudioVisualizationStrategy.PostScaledMax:
                    spectrumData = PostScaledMax;
                    break;
                case AudioVisualizationStrategy.PostScaledEnergy:
                    spectrumData = PostScaledEnergy;
                    break;
            }

            return spectrumData;
        }

        #endregion
    }
}
