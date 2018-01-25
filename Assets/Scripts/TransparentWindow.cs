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

            int fWidth = Screen.width;
            int fHeight = Screen.height;
            var margins = new WinApi.MARGINS { cxLeftWidth = -1 };
            var hwnd = NativeMethods.GetActiveWindow();

            // Transparent windows with click through
            NativeMethods.SetWindowLong(hwnd, WinApi.GWL_EXSTYLE, WinApi.WS_EX_LAYERED | WinApi.WS_EX_TRANSPARENT);
            NativeMethods.SetLayeredWindowAttributes(hwnd, 0, 255, WinApi.LWA_ALPHA);
            NativeMethods.SetWindowPos(hwnd, WinApi.HWND_TOPMOST, 0, 0, fWidth, fHeight, WinApi.SWP_FRAMECHANGED | WinApi.SWP_SHOWWINDOW);
            NativeMethods.DwmExtendFrameIntoClientArea(hwnd, ref margins);

            #endif
        }

        public void OnRenderImage(RenderTexture from, RenderTexture to)
        {
            Graphics.Blit(from, to, Material);
        }
    }
}