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
        private const int SpectrumSize = 16;

        private RigidbodyMaintainPosition _headHeight;
        private RigidbodyMaintainPosition _leftArmHeight;
        private RigidbodyMaintainPosition _leftElbowHeight;
        private RigidbodyMaintainPosition _rightArmHeight;
        private RigidbodyMaintainPosition _rightElbowHeight;
        private RigidbodyMaintainPosition _leftLegHeight;
        private RigidbodyMaintainPosition _leftKneeHeight;
        private RigidbodyMaintainPosition _rightLegHeight;
        private RigidbodyMaintainPosition _rightKneeHeight;
        private RigidbodyMaintainPosition _midSpineHeight;

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

        [Range(0, SpectrumSize - 1)]
        public int HeadSpectrumIndex;
        [Range(0, SpectrumSize - 1)]
        public int LeftArmSpectrumIndex;
        [Range(0, SpectrumSize - 1)]
        public int LeftElbowSpectrumIndex;
        [Range(0, SpectrumSize - 1)]
        public int RightArmSpectrumIndex;
        [Range(0, SpectrumSize - 1)]
        public int RightElbowSpectrumIndex;
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

        public float PullDown = 0.2f;
        public float PullUp = 0.5f;

        public override void Start()
        {
            base.Start();

            _headHeight = Head.GetComponent<RigidbodyMaintainPosition>();
            _leftArmHeight = LeftArm.GetComponent<RigidbodyMaintainPosition>();
            _leftElbowHeight = LeftElbow.GetComponent<RigidbodyMaintainPosition>();
            _rightArmHeight = RightArm.GetComponent<RigidbodyMaintainPosition>();
            _rightElbowHeight = RightElbow.GetComponent<RigidbodyMaintainPosition>();
            _leftLegHeight = LeftLeg.GetComponent<RigidbodyMaintainPosition>();
            _leftKneeHeight = LeftKnee.GetComponent<RigidbodyMaintainPosition>();
            _rightLegHeight = RightLeg.GetComponent<RigidbodyMaintainPosition>();
            _rightKneeHeight = RightKnee.GetComponent<RigidbodyMaintainPosition>();
            _midSpineHeight = MidSpine.GetComponent<RigidbodyMaintainPosition>();
        }

        public void Update()
        {
            var spectrumData = LoopbackAudio.GetAllSpectrumData(AudioVisualizationStrategy);

            // This will get the 10 largest values from the spectrum data, then sort it by
            // where it came from in the array
            //var sortedMaxSpectrumData = spectrumData.Select((value, index) => new { index, value })
            //    .OrderByDescending(kvp => kvp.value)
            //    .Take(RagdollPartCount)
            //    .OrderBy(kvp => kvp.index)
            //    .Select(kvp => kvp.value)
            //    .ToArray();

            //_headHeight.DesiredHeight = _headHeight.OriginalDesiredHeight + sortedMaxSpectrumData[HeadSpectrumIndex] * PrimaryScaleFactor;
            float min = -RealtimeAudio.MaxAudioValue * PullDown;
            float max = RealtimeAudio.MaxAudioValue * PullUp;

            _leftArmHeight.DesiredPosition.y = _leftArmHeight.OriginalDesiredPosition.y + normalizeToRange(spectrumData[LeftArmSpectrumIndex], min, max) * PrimaryScaleFactor;
            _leftElbowHeight.DesiredPosition.y = _leftElbowHeight.OriginalDesiredPosition.y + normalizeToRange(spectrumData[LeftElbowSpectrumIndex], min, max) * PrimaryScaleFactor;
            _rightArmHeight.DesiredPosition.y = _rightArmHeight.OriginalDesiredPosition.y + normalizeToRange(spectrumData[RightArmSpectrumIndex], min, max) * PrimaryScaleFactor;
            _rightElbowHeight.DesiredPosition.y = _rightElbowHeight.OriginalDesiredPosition.y + normalizeToRange(spectrumData[RightElbowSpectrumIndex], min, max) * PrimaryScaleFactor;
            _leftLegHeight.DesiredPosition.y = _leftLegHeight.OriginalDesiredPosition.y + normalizeToRange(spectrumData[LeftLegSpectrumIndex], min, max) * PrimaryScaleFactor;
            _leftKneeHeight.DesiredPosition.y = _leftKneeHeight.OriginalDesiredPosition.y + normalizeToRange(spectrumData[LeftKneeSpectrumIndex], min, max) * PrimaryScaleFactor;
            _rightLegHeight.DesiredPosition.y = _rightLegHeight.OriginalDesiredPosition.y + normalizeToRange(spectrumData[RightLegSpectrumIndex], min, max) * PrimaryScaleFactor;
            _rightKneeHeight.DesiredPosition.y = _rightKneeHeight.OriginalDesiredPosition.y + normalizeToRange(spectrumData[RightKneeSpectrumIndex], min, max) * PrimaryScaleFactor;
            _midSpineHeight.DesiredPosition.y = _midSpineHeight.OriginalDesiredPosition.y + normalizeToRange(spectrumData[MidSpineSpectrumIndex], min, max) * PrimaryScaleFactor;
        }

        private float normalizeToRange(float value, float min, float max)
        {
            // newvalue = (max'-min')/ (max - min) * (value - max) + max'
            return (max - min) / (10) * (value - 10) + max;
        }
    }
}
