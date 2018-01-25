using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Audio;
using Assets.Scripts.AudioReactiveEffects.Base;
using Assets.Scripts.Rigidbody;
using UnityEngine;

namespace Assets.Scripts.AudioReactiveEffects
{
    public class ReactiveRagdoll : VisualizationEffectBase
    {
        private const int RagdollPartCount = 10;

        private RigidbodyMaintainHeight _headHeight;
        private RigidbodyMaintainHeight _leftArmHeight;
        private RigidbodyMaintainHeight _leftElbowHeight;
        private RigidbodyMaintainHeight _rightArmHeight;
        private RigidbodyMaintainHeight _rightElbowHeight;
        private RigidbodyMaintainHeight _leftLegHeight;
        private RigidbodyMaintainHeight _leftKneeHeight;
        private RigidbodyMaintainHeight _rightLegHeight;
        private RigidbodyMaintainHeight _rightKneeHeight;
        private RigidbodyMaintainHeight _midSpineHeight;

        public GameObject Head;
        public GameObject LeftArm;
        public GameObject LeftElbow;
        public GameObject RightArm;
        public GameObject RightElbow;
        public GameObject LeftLeg;
        public GameObject LeftKnee;
        public GameObject RightLeg;
        public GameObject RightKnee;
        public GameObject MidSpine;

        [Range(0, RagdollPartCount - 1)]
        public int HeadSpectrumIndex;
        [Range(0, RagdollPartCount - 1)]
        public int LeftArmSpectrumIndex;
        [Range(0, RagdollPartCount - 1)]
        public int LeftElbowSpectrumIndex;
        [Range(0, RagdollPartCount - 1)]
        public int RightArmSpectrumIndex;
        [Range(0, RagdollPartCount - 1)]
        public int RightElbowSpectrumIndex;
        [Range(0, RagdollPartCount - 1)]
        public int LeftLegSpectrumIndex;
        [Range(0, RagdollPartCount - 1)]
        public int LeftKneeSpectrumIndex;
        [Range(0, RagdollPartCount - 1)]
        public int RightLegSpectrumIndex;
        [Range(0, RagdollPartCount - 1)]
        public int RightKneeSpectrumIndex;
        [Range(0, RagdollPartCount - 1)]
        public int MidSpineSpectrumIndex;

        public override void Start()
        {
            base.Start();

            _headHeight = Head.GetComponent<RigidbodyMaintainHeight>();
            _leftArmHeight = LeftArm.GetComponent<RigidbodyMaintainHeight>();
            _leftElbowHeight = LeftElbow.GetComponent<RigidbodyMaintainHeight>();
            _rightArmHeight = RightArm.GetComponent<RigidbodyMaintainHeight>();
            _rightElbowHeight = RightElbow.GetComponent<RigidbodyMaintainHeight>();
            _leftLegHeight = LeftLeg.GetComponent<RigidbodyMaintainHeight>();
            _leftKneeHeight = LeftKnee.GetComponent<RigidbodyMaintainHeight>();
            _rightLegHeight = RightLeg.GetComponent<RigidbodyMaintainHeight>();
            _rightKneeHeight = RightKnee.GetComponent<RigidbodyMaintainHeight>();
            _midSpineHeight = MidSpine.GetComponent<RigidbodyMaintainHeight>();
        }

        public void Update()
        {
            var spectrumData = LoopbackAudio.GetAllSpectrumData(AudioVisualizationStrategy);

            // This will get the 10 largest values from the spectrum data, then sort it by
            // where it came from in the array
            var sortedMaxSpectrumData = spectrumData.Select((value, index) => new { index, value })
                .OrderByDescending(kvp => kvp.value)
                .Take(RagdollPartCount)
                .OrderBy(kvp => kvp.index)
                .Select(kvp => kvp.value)
                .ToArray();

            //_headHeight.PullUpForce = sortedMaxSpectrumData[HeadSpectrumIndex] * PrimaryScaleFactor;
            //_leftArmHeight.PullUpForce = sortedMaxSpectrumData[LeftArmSpectrumIndex] * PrimaryScaleFactor;
            //_leftElbowHeight.PullUpForce = sortedMaxSpectrumData[LeftElbowSpectrumIndex] * PrimaryScaleFactor;
            //_rightArmHeight.PullUpForce = sortedMaxSpectrumData[RightArmSpectrumIndex] * PrimaryScaleFactor;
            //_rightElbowHeight.PullUpForce = sortedMaxSpectrumData[RightElbowSpectrumIndex] * PrimaryScaleFactor;
            //_leftLegHeight.PullUpForce = sortedMaxSpectrumData[LeftLegSpectrumIndex] * PrimaryScaleFactor;
            //_leftKneeHeight.PullUpForce = sortedMaxSpectrumData[LeftKneeSpectrumIndex] * PrimaryScaleFactor;
            //_rightLegHeight.PullUpForce = sortedMaxSpectrumData[RightLegSpectrumIndex] * PrimaryScaleFactor;
            //_rightKneeHeight.PullUpForce = sortedMaxSpectrumData[RightKneeSpectrumIndex] * PrimaryScaleFactor;
            //_midSpineHeight.PullUpForce = sortedMaxSpectrumData[MidSpineSpectrumIndex] * PrimaryScaleFactor;

            //_headHeight.DesiredHeight = _headHeight.OriginalDesiredHeight + sortedMaxSpectrumData[HeadSpectrumIndex] * PrimaryScaleFactor;
            float min = -RealtimeAudio.MaxAudioValue * 0.5f;
            float max = RealtimeAudio.MaxAudioValue * 0.5f;

            _leftArmHeight.DesiredHeight = _leftArmHeight.OriginalDesiredHeight + normalizeToRange(sortedMaxSpectrumData[LeftArmSpectrumIndex], min, max) * PrimaryScaleFactor;
            //_leftElbowHeight.DesiredHeight = _leftElbowHeight.OriginalDesiredHeight + sortedMaxSpectrumData[LeftElbowSpectrumIndex] * PrimaryScaleFactor;
            _rightArmHeight.DesiredHeight = _rightArmHeight.OriginalDesiredHeight + normalizeToRange(sortedMaxSpectrumData[RightArmSpectrumIndex], min, max) * PrimaryScaleFactor;
            //_rightElbowHeight.DesiredHeight = _rightElbowHeight.OriginalDesiredHeight + sortedMaxSpectrumData[RightElbowSpectrumIndex] * PrimaryScaleFactor;
            //_leftLegHeight.DesiredHeight = _leftLegHeight.OriginalDesiredHeight + sortedMaxSpectrumData[LeftLegSpectrumIndex] * PrimaryScaleFactor;
            //_leftKneeHeight.DesiredHeight = _leftKneeHeight.OriginalDesiredHeight + sortedMaxSpectrumData[LeftKneeSpectrumIndex] * PrimaryScaleFactor;
            //_rightLegHeight.DesiredHeight = _rightLegHeight.OriginalDesiredHeight + sortedMaxSpectrumData[RightLegSpectrumIndex] * PrimaryScaleFactor;
            //_rightKneeHeight.DesiredHeight = _rightKneeHeight.OriginalDesiredHeight + sortedMaxSpectrumData[RightKneeSpectrumIndex] * PrimaryScaleFactor;
            //_midSpineHeight.DesiredHeight = _midSpineHeight.OriginalDesiredHeight + sortedMaxSpectrumData[MidSpineSpectrumIndex] * PrimaryScaleFactor;
        }

        private float normalizeToRange(float value, float min, float max)
        {
            // newvalue = (max'-min')/ (max - min) * (value - max) + max'
            return (max - min) / (10) * (value - 10) + max;
        }
    }
}
