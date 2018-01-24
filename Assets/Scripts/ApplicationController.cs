using UnityEngine;

namespace Assets.Scripts
{
    public class ApplicationController : MonoBehaviour
    {
        public void Start()
        {
            Application.runInBackground = true;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}