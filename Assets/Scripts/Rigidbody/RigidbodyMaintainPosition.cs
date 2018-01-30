using UnityEngine;

namespace Assets.Scripts.Rigidbody
{
    public class RigidbodyMaintainPosition : MonoBehaviour
    {
        private UnityEngine.Rigidbody _rigidbody;

        public Vector3 DesiredPosition;
        public float PullForce = 10;
        public float LeadTime = 0.3f; // *** THIS IS USED TO SLOW DOWN WHEN APPROACHING THE DESIRED HEIGHT, INSTEAD OF OVERSHOOTING BACK AND FORTH **
        public float InitDelay = 0.0f;

        public Vector3 OriginalDesiredPosition { get; private set; }
        public float OriginalPullForce { get; private set; }
        public bool IsInitialized { get; private set; }
        public bool IsCooling { get; set; }

        public void Start()
        {
            _rigidbody = GetComponent<UnityEngine.Rigidbody>();

            OriginalPullForce = PullForce;
        }

        public void FixedUpdate()
        {
            // Wait until the ragdoll is at rest before recording positions
            if (Time.time > InitDelay && !IsInitialized)
            {
                init();
            }

            if (IsInitialized)
            {
                var diff = DesiredPosition - (transform.position + _rigidbody.velocity * LeadTime);

                var dist = new Vector3(Mathf.Abs(diff.x), Mathf.Abs(diff.y), Mathf.Abs(diff.z));

                var pullM = new Vector3(Mathf.Clamp01(dist.x / 0.3f), Mathf.Clamp01(dist.y / 0.3f),
                    Mathf.Clamp01(dist.z / 0.3f));

                var force = new Vector3(Mathf.Sign(diff.x) * PullForce * pullM.x * Time.deltaTime,
                    Mathf.Sign(diff.y) * PullForce * pullM.y * Time.deltaTime,
                    Mathf.Sign(diff.z) * PullForce * pullM.z * Time.deltaTime);

                _rigidbody.AddForce(force, ForceMode.Impulse);
            }
        }

        public void OnDrawGizmos()
        {
            if (enabled)
            {
                var color = Color.green;

                if (IsCooling)
                {
                    color = Color.red;
                }

                Gizmos.color = color;
                Gizmos.DrawLine(transform.position, DesiredPosition);
                Gizmos.DrawWireSphere(DesiredPosition, 0.1f);
            }
        }

        private void init()
        {
            DesiredPosition += transform.position;
            OriginalDesiredPosition = DesiredPosition;
            IsInitialized = true;
        }
    }
}
