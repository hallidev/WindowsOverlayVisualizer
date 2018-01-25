using UnityEngine;

namespace Assets.Scripts.Character
{
    public class CharacterUpright : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        public bool KeepUpright = true;
        public float UprightForce = 100;
        public float UprightOffset = 1.45f;
        public float AdditionalUpwardForce = 0;
        public float DampenAngularForce = 0;
    
        public void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.maxAngularVelocity = 40; // **** CANNOT APPLY HIGH ANGULAR FORCES UNLESS THE MAXANGULAR VELOCITY IS INCREASED ****
        }

        public void FixedUpdate()
        {
            if (KeepUpright)
            {
                // ***** USE TWO FORCES PULLING UP AND DOWN AT THE TOP AND BOTTOM OF THE OBJECT RESPECTIVELY TO PULL IT UPRIGHT ***
                //
                //  *** THIS TECHNIQUE CAN BE USED FOR PULLING AN OBJECT TO FACE ANY VECTOR ***
                //
                _rigidbody.AddForceAtPosition(new Vector3(0, (UprightForce + AdditionalUpwardForce), 0),
                    transform.position + transform.TransformPoint(new Vector3(0, UprightOffset, 0)), ForceMode.Force);

                _rigidbody.AddForceAtPosition(new Vector3(0, -UprightForce, 0),
                    transform.position + transform.TransformPoint(new Vector3(0, -UprightOffset, 0)), ForceMode.Force);
            }

            if (DampenAngularForce > 0)
            {
                _rigidbody.angularVelocity *= (1 - Time.deltaTime * DampenAngularForce);
            }
        }
    }
}
