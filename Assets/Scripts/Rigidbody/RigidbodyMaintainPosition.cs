﻿using UnityEngine;

namespace Assets.Scripts.Rigidbody
{
    public class RigidbodyMaintainPosition : MonoBehaviour
    {
        private UnityEngine.Rigidbody _rigidbody;

        public Vector3 DesiredPosition;
        public float PullUpForce = 10;
        public float LeadTime = 0.3f; // *** THIS IS USED TO SLOW DOWN WHEN APPROACHING THE DESIRED HEIGHT, INSTEAD OF OVERSHOOTING BACK AND FORTH **

        public Vector3 OriginalDesiredPosition { get; private set; }

        public void Start()
        {
            _rigidbody = GetComponent<UnityEngine.Rigidbody>();

            DesiredPosition += transform.position;
            OriginalDesiredPosition = DesiredPosition;
        }

        public void FixedUpdate()
        {
            var diff = DesiredPosition - (transform.position + _rigidbody.velocity * LeadTime);

            var dist = new Vector3(Mathf.Abs(diff.x), Mathf.Abs(diff.y), Mathf.Abs(diff.z));

            var pullM = new Vector3(Mathf.Clamp01(dist.x / 0.3f), Mathf.Clamp01(dist.y / 0.3f), Mathf.Clamp01(dist.z / 0.3f));

            var force = new Vector3(Mathf.Sign(diff.x) * PullUpForce * pullM.x * Time.deltaTime,
                Mathf.Sign(diff.y) * PullUpForce * pullM.y * Time.deltaTime,
                Mathf.Sign(diff.z) * PullUpForce * pullM.z * Time.deltaTime);

            _rigidbody.AddForce(force, ForceMode.Impulse);
        }
    }
}