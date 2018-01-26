using System.Collections.Generic;
using Assets.Scripts.Audio;
using Assets.Scripts.AudioReactiveEffects.Base;
using Assets.Scripts.Rigidbody;
using UnityEngine;

namespace Assets.Scripts.AudioReactiveEffects
{
    public class ReactiveRagdoll : VisualizationEffectBase
    {
        private const int SpectrumSize = 64;

        private RagdollPart[] _ragdollParts;

        public GameObject Head;
        public GameObject LeftArm;
        public GameObject LeftElbow;
        public GameObject LeftWrist;
        public GameObject RightArm;
        public GameObject RightElbow;
        public GameObject RightWrist;
        public GameObject LeftLeg;
        public GameObject LeftKnee;
        public GameObject RightLeg;
        public GameObject RightKnee;
        public GameObject MidSpine;

        public bool DoHead;
        public bool DoLeftArm;
        public bool DoLeftElbow;
        public bool DoLeftWrist;
        public bool DoRightArm;
        public bool DoRightElbow;
        public bool DoRightWrist;
        public bool DoLeftLeg;
        public bool DoLeftKnee;
        public bool DoRightLeg;
        public bool DoRightKnee;
        public bool DoMidSpine;

        [Range(0, SpectrumSize - 1)]
        public int HeadSpectrumIndex;
        [Range(0, SpectrumSize - 1)]
        public int LeftArmSpectrumIndex;
        [Range(0, SpectrumSize - 1)]
        public int LeftElbowSpectrumIndex;
        [Range(0, SpectrumSize - 1)]
        public int LeftWristSpectrumIndex;
        [Range(0, SpectrumSize - 1)]
        public int RightArmSpectrumIndex;
        [Range(0, SpectrumSize - 1)]
        public int RightElbowSpectrumIndex;
        [Range(0, SpectrumSize - 1)]
        public int RightWristSpectrumIndex;
        [Range(0, SpectrumSize - 1)]
        public int LeftLegSpectrumIndex;
        [Range(0, SpectrumSize - 1)]
        public int LeftKneeSpectrumIndex;
        [Range(0, SpectrumSize - 1)]
        public int RightLegSpectrumIndex;
        [Range(0, SpectrumSize - 1)]
        public int RightKneeSpectrumIndex;
        [Range(0, SpectrumSize - 1)]
        public int MidSpineSpectrumIndex;

        public float HeightAudioScale = 0.2f;
        public float ForceAudioScale = 0.2f;
        public float PullDown = 0.5f;
        public float PullUp = 0.5f;

        public override void Start()
        {
            base.Start();

            hookupRagdoll();
        }

        public void Update()
        {
            var spectrumData = LoopbackAudio.GetAllSpectrumData(AudioVisualizationStrategy);

            if (Input.GetKeyDown(KeyCode.A))
            {
                hookupRagdoll();
            }

            float min = -RealtimeAudio.MaxAudioValue * PullDown;
            float max = RealtimeAudio.MaxAudioValue * PullUp;

            for (int i = 0; i < _ragdollParts.Length; i++)
            {
                var maintainPos = _ragdollParts[i].RigidbodyMaintainPosition;

                if (_ragdollParts[i].DoEffect)
                {
                    // Height
                    maintainPos.DesiredPosition.y = maintainPos.OriginalDesiredPosition.y
                                                    + normalizeToRange(spectrumData[_ragdollParts[i].SpectrumIndex],
                                                        min, max)
                                                    * HeightAudioScale;

                    // Pull force
                    //maintainPos.PullForce = maintainPos.OriginalPullForce +
                    //                        spectrumData[_ragdollParts[i].SpectrumIndex] * ForceAudioScale;
                }
                else
                {
                    maintainPos.DesiredPosition.y = maintainPos.OriginalDesiredPosition.y;
                    maintainPos.PullForce = maintainPos.OriginalPullForce;
                }
            }
        }

        private void hookupRagdoll()
        {
            var ragdollParts = new List<RagdollPart>();

            ragdollParts.Add(new RagdollPart
            {
                GameObject = Head,
                RigidbodyMaintainPosition = Head.GetComponent<RigidbodyMaintainPosition>(),
                SpectrumIndex = HeadSpectrumIndex,
                DoEffect = DoHead
            });

            ragdollParts.Add(new RagdollPart
            {
                GameObject = LeftArm,
                RigidbodyMaintainPosition = LeftArm.GetComponent<RigidbodyMaintainPosition>(),
                SpectrumIndex = LeftArmSpectrumIndex,
                DoEffect = DoLeftArm
            });

            ragdollParts.Add(new RagdollPart
            {
                GameObject = LeftElbow,
                RigidbodyMaintainPosition = LeftElbow.GetComponent<RigidbodyMaintainPosition>(),
                SpectrumIndex = LeftElbowSpectrumIndex,
                DoEffect = DoLeftElbow
            });

            ragdollParts.Add(new RagdollPart
            {
                GameObject = LeftWrist,
                RigidbodyMaintainPosition = LeftWrist.GetComponent<RigidbodyMaintainPosition>(),
                SpectrumIndex = LeftWristSpectrumIndex,
                DoEffect = DoLeftWrist
            });

            ragdollParts.Add(new RagdollPart
            {
                GameObject = RightArm,
                RigidbodyMaintainPosition = RightArm.GetComponent<RigidbodyMaintainPosition>(),
                SpectrumIndex = RightArmSpectrumIndex,
                DoEffect = DoRightArm
            });

            ragdollParts.Add(new RagdollPart
            {
                GameObject = RightElbow,
                RigidbodyMaintainPosition = RightElbow.GetComponent<RigidbodyMaintainPosition>(),
                SpectrumIndex = RightElbowSpectrumIndex,
                DoEffect = DoRightElbow
            });

            ragdollParts.Add(new RagdollPart
            {
                GameObject = RightWrist,
                RigidbodyMaintainPosition = RightWrist.GetComponent<RigidbodyMaintainPosition>(),
                SpectrumIndex = RightWristSpectrumIndex,
                DoEffect = DoRightWrist
            });

            ragdollParts.Add(new RagdollPart
            {
                GameObject = LeftLeg,
                RigidbodyMaintainPosition = LeftLeg.GetComponent<RigidbodyMaintainPosition>(),
                SpectrumIndex = LeftLegSpectrumIndex,
                DoEffect = DoLeftLeg
            });

            ragdollParts.Add(new RagdollPart
            {
                GameObject = LeftKnee,
                RigidbodyMaintainPosition = LeftKnee.GetComponent<RigidbodyMaintainPosition>(),
                SpectrumIndex = LeftKneeSpectrumIndex,
                DoEffect = DoLeftKnee
            });

            ragdollParts.Add(new RagdollPart
            {
                GameObject = RightLeg,
                RigidbodyMaintainPosition = RightLeg.GetComponent<RigidbodyMaintainPosition>(),
                SpectrumIndex = RightLegSpectrumIndex,
                DoEffect = DoRightLeg
            });

            ragdollParts.Add(new RagdollPart
            {
                GameObject = RightKnee,
                RigidbodyMaintainPosition = RightKnee.GetComponent<RigidbodyMaintainPosition>(),
                SpectrumIndex = RightKneeSpectrumIndex,
                DoEffect = DoRightKnee
            });

            ragdollParts.Add(new RagdollPart
            {
                GameObject = MidSpine,
                RigidbodyMaintainPosition = MidSpine.GetComponent<RigidbodyMaintainPosition>(),
                SpectrumIndex = MidSpineSpectrumIndex,
                DoEffect = DoMidSpine
            });

            _ragdollParts = ragdollParts.ToArray();
        }

        private float normalizeToRange(float value, float min, float max)
        {
            // newvalue = (max'-min')/ (max - min) * (value - max) + max'
            return (max - min) / (10) * (value - 10) + max;
        }

        private struct RagdollPart
        {
            public GameObject GameObject { get; set; }
            public RigidbodyMaintainPosition RigidbodyMaintainPosition { get; set; }
            public int SpectrumIndex { get; set; }
            public bool DoEffect { get; set; }
        }
    }
}
