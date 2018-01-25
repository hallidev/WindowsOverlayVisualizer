using UnityEngine;

namespace Assets.Scripts
{
    public class ViewportOffsetObject : MonoBehaviour
    {
        public Vector3 ViewportOffset;

        private Vector3 _viewPortOffset;

        public void Awake()
        {
            #if !UNITY_EDITOR

            if (_viewPortOffset != ViewportOffset)
            {
                transform.position = Camera.main.ViewportToWorldPoint(ViewportOffset);
                _viewPortOffset = ViewportOffset;
            }

            #endif
        }
    }
}