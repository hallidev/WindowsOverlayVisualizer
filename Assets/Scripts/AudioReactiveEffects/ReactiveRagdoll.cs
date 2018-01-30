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
        public Vector3 MaxPositiveImpulseDuration;
        public Vector3 MaxPositiveImpulseCooldown;
        public bool DoEffect;

        [HideInInspector]
        public Vector3 PositiveImpulseDuration;
        [HideInInspector]
        public Vector3 PositiveImpulseCooldown;
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
                    float amount = normalizeToRange(spectrumData[ragdollPart.SpectrumIndex], min, max) * ragdollPart.BumpAmountScale;
                    Vector3 amountWithDirection = ragdollPart.BumpDirection * amount;

                    // Record how long the amount has been positive
                    if (amountWithDirection.y > 0.0f)
                    {
                        // If there is a value for this axis, see how long we've been in the positives
                        if (ragdollPart.MaxPositiveImpulseDuration.y > 0.0f 
                            && ragdollPart.PositiveImpulseDuration.y > ragdollPart.MaxPositiveImpulseDuration.y)
                        {
                            // If too long, remove the amount on this axis
                            amountWithDirection.y = 0.0f;
                        }

                        ragdollPart.PositiveImpulseDuration.y += Time.deltaTime;
                    }
                    else
                    {
                        //ragdollPart.PositiveImpulseDuration.y = 0.0f;
                    }

                    // Direction

                    maintainPos.DesiredPosition = maintainPos.OriginalDesiredPosition + amountWithDirection;
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
