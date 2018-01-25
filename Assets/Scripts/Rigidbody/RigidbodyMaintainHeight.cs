using UnityEngine;

namespace Assets.Scripts.Rigidbody
{
    public class RigidbodyMaintainHeight : MonoBehaviour
    {
        private UnityEngine.Rigidbody _rigidbody;
        private float _groundHeight;

        public float DesiredHeight;
        public float PullUpForce = 10;
        public float LeadTime = 0.3f; // *** THIS IS USED TO SLOW DOWN WHEN APPROACHING THE DESIRED HEIGHT, INSTEAD OF OVERSHOOTING BACK AND FORTH **
        public Transform InRelationTo = null;

        public float OriginalDesiredHeight { get; private set; }

        public void Start()
        {
            _rigidbody = GetComponent<UnityEngine.Rigidbody>();

            DesiredHeight += transform.position.y;
            OriginalDesiredHeight = DesiredHeight;
        }

        public void FixedUpdate()
        {
            // ***** TRY HOLD A OBJECT AT A SPECIFIC HEIGHT (optionally in relation to another object) ***
            //
            RaycastHit groundHit;

            if (Physics.Raycast(new Ray(transform.position, Vector3.down), out groundHit, 100, 1 << LayerMask.NameToLayer("Ground")))
            {
                _groundHeight = groundHit.point.y;
            }

            float diff = (_groundHeight + DesiredHeight) - (transform.position.y + _rigidbody.velocity.y * LeadTime);

            if (InRelationTo != null)
            {
                diff = InRelationTo.TransformPoint(Vector3.up * DesiredHeight).y - (transform.position.y + _rigidbody.velocity.y * LeadTime);
            }

            float dist = Mathf.Abs(diff);
            float pullM = Mathf.Clamp01(dist / 0.3f);
            _rigidbody.AddForce(new Vector3(0, Mathf.Sign(diff) * PullUpForce * pullM * Time.deltaTime, 0), ForceMode.Impulse);
        }
    }
}
