using System;
using Assets.Scripts.Extensions;
using Assets.Scripts.Interop;
using UnityEngine;

namespace Assets.Scripts
{
    public class WindowsOverlay : MonoBehaviour
    {
        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_LAYERED = 0x80000;
        private const int LWA_ALPHA = 0x2;
        private const int LWA_COLORKEY = 0x1;

        private IntPtr _windowHandle;
        private IntPtr _originalWindowStyle;
        private bool _isOverlay;

        public Color ColorKey;

        public void Start()
        {
            _windowHandle = NativeMethods.GetActiveWindow();
            _originalWindowStyle = NativeMethods.GetWindowLong(_windowHandle, GWL_EXSTYLE);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (!_isOverlay)
                {
                    // Make this a layered window
                    NativeMethods.SetWindowLong(_windowHandle, GWL_EXSTYLE,
                        new IntPtr(_originalWindowStyle.ToInt32() ^ WS_EX_LAYERED));

                    // Make window transparent with color key
                    NativeMethods.SetLayeredWindowAttributes(_windowHandle, ((Color32)ColorKey).ToUintRgb(), 0, LWA_COLORKEY);

                    // Topmost
                    NativeMethods.SetWindowPos(_windowHandle, NativeMethods.Hwnd.TopMost, 0, 0, 0, 0,
                        NativeMethods.SetWindowPosFlags.IgnoreMove | NativeMethods.SetWindowPosFlags.IgnoreResize);
                }
                else
                {
                    // Restore original style
                    NativeMethods.SetWindowLong(_windowHandle, GWL_EXSTYLE, _originalWindowStyle);
                }

                _isOverlay = !_isOverlay;
            }
        }
    }
}