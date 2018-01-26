using System;
using Assets.Scripts.Audio;
using Assets.Scripts.AudioReactiveEffects.Base;
using Assets.Scripts.Rigidbody;
using UnityEngine;

namespace Assets.Scripts.AudioReactiveEffects
{
    [Serializable]
    public struct ReactiveRagdollPart
    {
        public GameObject GameObject;
        public BumpDirection BumpDirection;
        public int SpectrumIndex;
        public float BumpAmountScale;
        public float PullDown;
        public float PullUp;
        public bool DoEffect;
    }

    public enum BumpDirection
    {
        UpDown,
        LeftRight,
        InOut
    }

    public class ReactiveRagdoll : VisualizationEffectBase
    {
        public ReactiveRagdollPart[] RagdollParts;

        public void Update()
        {
            var spectrumData = LoopbackAudio.GetAllSpectrumData(AudioVisualizationStrategy);

            for (int i = 0; i < RagdollParts.Length; i++)
            {
                var ragdollPart = RagdollParts[i];
                var maintainPos = ragdollPart.GameObject.GetComponent<RigidbodyMaintainPosition>();

                float min = -RealtimeAudio.MaxAudioValue * ragdollPart.PullDown;
                float max = RealtimeAudio.MaxAudioValue * ragdollPart.PullUp;

                if (ragdollPart.DoEffect)
                {
                    // Direction
                    float amount = normalizeToRange(spectrumData[ragdollPart.SpectrumIndex], min, max) * ragdollPart.BumpAmountScale;

                    switch (ragdollPart.BumpDirection)
                    {
                        case BumpDirection.UpDown:
                            maintainPos.DesiredPosition.y = maintainPos.OriginalDesiredPosition.y + amount;
                            break;
                        case BumpDirection.LeftRight:
                            maintainPos.DesiredPosition.x = maintainPos.OriginalDesiredPosition.x + amount;
                            break;
                        case BumpDirection.InOut:
                            maintainPos.DesiredPosition.z = maintainPos.OriginalDesiredPosition.z + amount;
                            break;
                    }
                }
                else
                {
                    maintainPos.DesiredPosition.y = maintainPos.OriginalDesiredPosition.y;
                    maintainPos.PullForce = maintainPos.OriginalPullForce;
                }
            }
        }

        private float normalizeToRange(float value, float min, float max)
        {
            // newvalue = (max'-min')/ (max - min) * (value - max) + max'
            return (max - min) / (10) * (value - 10) + max;
        }
    }
}
