using UnityEngine;

namespace Assets.Scripts.Rigidbody
{
    public class RigidbodiesFaceDirection : MonoBehaviour
    {
        private RigidbodyFaceDirection[] _rigidbodyFaceDirections;

        public Vector3 FacingDirection = Vector3.zero;

        public void Start()
        {
            _rigidbodyFaceDirections = GetComponentsInChildren<RigidbodyFaceDirection>();
        }

        public void FixedUpdate()
        {
            for (int i = 0; i < _rigidbodyFaceDirections.Length; i++)
            {
                _rigidbodyFaceDirections[i].FacingDirection = FacingDirection;
            }
        }
    }
}
