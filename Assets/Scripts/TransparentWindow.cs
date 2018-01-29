using System;
using Assets.Scripts.Interop;
using UnityEngine;

namespace Assets.Scripts
{
    public class TransparentWindow : MonoBehaviour
    {
        public Material Material;

        public void Start()
        {
            #if !UNITY_EDITOR // You really don't want to enable this in the editor..

            bool dmwEnabled;
            NativeMethods.DwmIsCompositionEnabled(out dmwEnabled);

            var hwnd = NativeMethods.GetActiveWindow();

            if (dmwEnabled)
            {
                int fWidth = Screen.width;
                int fHeight = Screen.height;
                var margins = new WinApi.MARGINS {cxLeftWidth = -1};

                // Transparent windows with click through
                NativeMethods.SetWindowLong(hwnd, WinApi.GWL_EXSTYLE,
                    WinApi.WS_EX_LAYERED | WinApi.WS_EX_TRANSPARENT);
                NativeMethods.SetLayeredWindowAttributes(hwnd, 0, 255, WinApi.LWA_ALPHA);
                NativeMethods.SetWindowPos(hwnd, WinApi.HWND_TOPMOST, 0, 0, fWidth, fHeight,
                    WinApi.SWP_FRAMECHANGED | WinApi.SWP_SHOWWINDOW);
                NativeMethods.DwmExtendFrameIntoClientArea(hwnd, ref margins);
            }
            else
            {
                NativeMethods.MessageBoxW(hwnd, "You must have an Aero compatible theme enabled.", "Error", WinApi.MESSAGEBOXTYPE_OK);
                Application.Quit();
            }
                
            #endif
        }

        public void OnRenderImage(RenderTexture from, RenderTexture to)
        {
            if (Material.shader.isSupported)
            {
                Graphics.Blit(from, to, Material);
            }
            else
            {
                Graphics.Blit(from, to);
            }
        }
    }
}