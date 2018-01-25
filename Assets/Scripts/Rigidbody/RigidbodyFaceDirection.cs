using UnityEngine;

namespace Assets.Scripts.Rigidbody
{
    public class RigidbodyFaceDirection : MonoBehaviour
    {
        private UnityEngine.Rigidbody _rigidbody;

        public Vector3 BodyForward = new Vector3(0, 0, 2);
        public Vector3 FacingDirection = Vector3.zero;
        public float FacingForce = 800;
        public float LeadTime = 0; // *** THIS IS USED TO SLOW DOWN WHEN APPROACHING THE DESIRED DIRECTION, INSTEAD OF OVERSHOOTING BACK AND FORTH **

        public void Start()
        {
            _rigidbody = GetComponent<UnityEngine.Rigidbody>();
        }

        public void FixedUpdate()
        {
            if (LeadTime == 0)
            {
                // ****** JUST PULL WITH TWO STRINGS TO FACE DIRECTION *****
                //
                if (FacingDirection != Vector3.zero)
                {
                    // *********************  FACE CHEST TOWARDS THE INPUT DIRECTION *******
                    _rigidbody.AddForceAtPosition(FacingForce * FacingDirection * Time.deltaTime, _rigidbody.transform.TransformDirection(BodyForward), ForceMode.Impulse);
                    _rigidbody.AddForceAtPosition(-FacingForce * FacingDirection * Time.deltaTime, _rigidbody.transform.TransformDirection(-BodyForward), ForceMode.Impulse);
                }
            }
            else
            {
                // ******** TRY PULL TOWARDS DIRECTION FACTORING IN VELOCITY (ie. decelerate towards the target) ***********
                Vector3 targetPoint = transform.position + FacingDirection * BodyForward.magnitude;
                Vector3 currentPoint = transform.TransformPoint(BodyForward);
                Vector3 reversePoint = transform.TransformPoint(-BodyForward);
                Vector3 velocity = _rigidbody.GetPointVelocity(currentPoint);
                Vector3 diff = targetPoint - (currentPoint + velocity * LeadTime);

                _rigidbody.AddForceAtPosition(FacingForce * diff * Time.deltaTime, currentPoint, ForceMode.Impulse);
                _rigidbody.AddForceAtPosition(-FacingForce * diff * Time.deltaTime, reversePoint, ForceMode.Impulse);
            }
        }
    }
}
