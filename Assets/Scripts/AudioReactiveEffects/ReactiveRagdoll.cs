using System;
using Assets.Scripts.Audio;
using Assets.Scripts.AudioReactiveEffects.Base;
using Assets.Scripts.Rigidbody;
using UnityEngine;

namespace Assets.Scripts.AudioReactiveEffects
{
    public enum ReactiveRagdollPoseType
    {
        Forward
    }

    [Serializable]
    public struct ReactiveRagdollPose
    {
        public ReactiveRagdollPoseType Pose;
        public Vector3 FacingDirection;
        public ReactiveRagdollPart[] RagdollParts;
    }

    [Serializable]
    public struct ReactiveRagdollPart
    {
        public GameObject GameObject;
        public Vector3 BumpDirection;
        public int SpectrumIndex;
        public float BumpAmountScale;
        public float PullDown;
        public float PullUp;
        public bool DoEffect;
    }

    public class ReactiveRagdoll : VisualizationEffectBase
    {
        private ReactiveRagdollPose _currentPose;

        public GameObject RagdollGameObject;
        public ReactiveRagdollPose[] RagdollPoses;

        public override void Start()
        {
            base.Start();

            pickPose();
        }

        public void Update()
        {
            var spectrumData = LoopbackAudio.GetAllSpectrumData(AudioVisualizationStrategy);

            for (int i = 0; i < _currentPose.RagdollParts.Length; i++)
            {
                var ragdollPart = _currentPose.RagdollParts[i];
                var maintainPos = ragdollPart.GameObject.GetComponent<RigidbodyMaintainPosition>();

                if (!maintainPos.IsInitialized)
                {
                    continue;
                }

                float min = -RealtimeAudio.MaxAudioValue * ragdollPart.PullDown;
                float max = RealtimeAudio.MaxAudioValue * ragdollPart.PullUp;

                if (ragdollPart.DoEffect)
                {
                    // Direction
                    float amount = normalizeToRange(spectrumData[ragdollPart.SpectrumIndex], min, max) * ragdollPart.BumpAmountScale;

                    maintainPos.DesiredPosition = maintainPos.OriginalDesiredPosition + ragdollPart.BumpDirection * amount;
                }
                else
                {
                    if (maintainPos.DesiredPosition != maintainPos.OriginalDesiredPosition)
                    {
                        maintainPos.DesiredPosition = maintainPos.OriginalDesiredPosition;
                        maintainPos.PullForce = maintainPos.OriginalPullForce;
                    }
                }
            }
        }

        private void pickPose()
        {
            _currentPose = RagdollPoses[0];
            RagdollGameObject.GetComponent<RigidbodiesFaceDirection>().FacingDirection = _currentPose.FacingDirection;
        }

        private float normalizeToRange(float value, float min, float max)
        {
            // newvalue = (max'-min')/ (max - min) * (value - max) + max'
            return (max - min) / RealtimeAudio.MaxAudioValue * (value - RealtimeAudio.MaxAudioValue) + max;
        }
    }
}
