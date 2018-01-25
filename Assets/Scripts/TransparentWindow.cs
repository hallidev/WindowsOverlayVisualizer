using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Assets.Scripts
{
    public class TransparentWindow : MonoBehaviour
    {
        private struct MARGINS
        {
            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyBottomHeight;
        }

        // Define function signatures to import from Windows APIs
        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        [DllImport("Dwmapi.dll")]
        private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

        // Definitions of window styles
        private const int GWL_STYLE = -16;
        private const uint WS_POPUP = 0x80000000;
        private const uint WS_VISIBLE = 0x10000000;

        private bool _isOverlayActive;

        public Material Material;

        public void Start()
        {
//#if !UNITY_EDITOR
//        var margins = new MARGINS() { cxLeftWidth = -1 };
 
//        // Get a handle to the window
//        var hwnd = GetActiveWindow();
 
//        // Set properties of the window
//        // See: https://msdn.microsoft.com/en-us/library/windows/desktop/ms633591%28v=vs.85%29.aspx
//        SetWindowLong(hwnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);
         
//        // Extend the window into the client area
//        // See: https://msdn.microsoft.com/en-us/library/windows/desktop/aa969512%28v=vs.85%29.aspx 
//        DwmExtendFrameIntoClientArea(hwnd, ref margins);
//#endif
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (!_isOverlayActive)
                {
                    var margins = new MARGINS() { cxLeftWidth = -1 };

                    // Get a handle to the window
                    var hwnd = GetActiveWindow();

                    // Set properties of the window
                    // See: https://msdn.microsoft.com/en-us/library/windows/desktop/ms633591%28v=vs.85%29.aspx
                    SetWindowLong(hwnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);

                    // Extend the window into the client area
                    // See: https://msdn.microsoft.com/en-us/library/windows/desktop/aa969512%28v=vs.85%29.aspx 
                    DwmExtendFrameIntoClientArea(hwnd, ref margins);
                }
                else
                {
                    
                }

                _isOverlayActive = !_isOverlayActive;
            }
        }

        // Pass the output of the camera to the custom material
        // for chroma replacement
        public void OnRenderImage(RenderTexture from, RenderTexture to)
        {
            Graphics.Blit(from, to, Material);
        }
    }
}