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
        public Vector3 RestingOffset;
        public float PullForce;
        public float LeadTime;
        public float InitDelay;
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
        [HideInInspector]
        public bool IsInCooldownX;
        [HideInInspector]
        public bool IsInCooldownY;
        [HideInInspector]
        public bool IsInCooldownZ;
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

                    var cooledAmountWithDirection = doCooldown(ragdollPart, i, amountWithDirection);

                    maintainPos.IsCooling = ragdollPart.IsInCooldownX || ragdollPart.IsInCooldownY || ragdollPart.IsInCooldownZ;

                    // Direction
                    maintainPos.DesiredPosition = maintainPos.OriginalDesiredPosition + cooledAmountWithDirection;
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

        private Vector3 doCooldown(ReactiveRagdollPart ragdollPart, int i, Vector3 amountWithDirection)
        {
            // --------
            // X
            // --------
            if (ragdollPart.IsInCooldownX)
            {
                amountWithDirection.x = 0.0f;
                ragdollPart.PositiveImpulseCooldown.x += Time.deltaTime;

                if (ragdollPart.PositiveImpulseCooldown.x > ragdollPart.MaxPositiveImpulseCooldown.x)
                {
                    // Reset
                    ragdollPart.IsInCooldownX = false;
                    ragdollPart.PositiveImpulseCooldown.x = 0.0f;
                    ragdollPart.PositiveImpulseDuration.x = 0.0f;
                }
            }
            else
            {
                // Record how long the amount has been positive
                if (amountWithDirection.x > 0.0f)
                {
                    // If there is a value for this axis, see how long we've been in the positives
                    if (ragdollPart.MaxPositiveImpulseDuration.x > 0.0f
                        && ragdollPart.PositiveImpulseDuration.x > ragdollPart.MaxPositiveImpulseDuration.x)
                    {
                        // If too long, cool down
                        ragdollPart.IsInCooldownX = true;
                    }

                    ragdollPart.PositiveImpulseDuration.x += Time.deltaTime;
                }
                else
                {
                    ragdollPart.PositiveImpulseDuration.x = 0.0f;
                }
            }

            // --------
            // Y
            // --------
            if (ragdollPart.IsInCooldownY)
            {
                amountWithDirection.y = 0.0f;
                ragdollPart.PositiveImpulseCooldown.y += Time.deltaTime;

                if (ragdollPart.PositiveImpulseCooldown.y > ragdollPart.MaxPositiveImpulseCooldown.y)
                {
                    // Reset
                    ragdollPart.IsInCooldownY = false;
                    ragdollPart.PositiveImpulseCooldown.y = 0.0f;
                    ragdollPart.PositiveImpulseDuration.y = 0.0f;
                }
            }
            else
            {
                // Record how long the amount has been positive
                if (amountWithDirection.y > 0.0f)
                {
                    // If there is a value for this axis, see how long we've been in the positives
                    if (ragdollPart.MaxPositiveImpulseDuration.y > 0.0f
                        && ragdollPart.PositiveImpulseDuration.y > ragdollPart.MaxPositiveImpulseDuration.y)
                    {
                        // If too long, cool down
                        ragdollPart.IsInCooldownY = true;
                    }

                    ragdollPart.PositiveImpulseDuration.y += Time.deltaTime;
                }
                else
                {
                    ragdollPart.PositiveImpulseDuration.y = 0.0f;
                }
            }

            // --------
            // Z
            // --------
            if (ragdollPart.IsInCooldownZ)
            {
                amountWithDirection.z = 0.0f;
                ragdollPart.PositiveImpulseCooldown.z += Time.deltaTime;

                if (ragdollPart.PositiveImpulseCooldown.z > ragdollPart.MaxPositiveImpulseCooldown.z)
                {
                    // Reset
                    ragdollPart.IsInCooldownZ = false;
                    ragdollPart.PositiveImpulseCooldown.z = 0.0f;
                    ragdollPart.PositiveImpulseDuration.z = 0.0f;
                }
            }
            else
            {
                // Record how long the amount has been positive
                if (amountWithDirection.z > 0.0f)
                {
                    // If there is a value for this axis, see how long we've been in the positives
                    if (ragdollPart.MaxPositiveImpulseDuration.z > 0.0f
                        && ragdollPart.PositiveImpulseDuration.z > ragdollPart.MaxPositiveImpulseDuration.z)
                    {
                        // If too long, cool down
                        ragdollPart.IsInCooldownZ = true;
                    }

                    ragdollPart.PositiveImpulseDuration.z += Time.deltaTime;
                }
                else
                {
                    ragdollPart.PositiveImpulseDuration.z = 0.0f;
                }
            }

            // Make sure to update the struct!
            _currentPose.RagdollParts[i] = ragdollPart;

            return amountWithDirection;
        }
    }
}
