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
        private const int ULW_ALPHA = 2;
        private const byte AC_SRC_OVER = 0;
        private const byte AC_SRC_ALPHA = 1;

        private IntPtr _windowHandle;
        private IntPtr _originalWindowStyle;
        private bool _isOverlayActive;
        private int _overlayWidth;
        private int _overlayHeight;
        private IntPtr _renderTextureHandle = IntPtr.Zero;

        public Color ColorKey;

        public void Start()
        {
            _windowHandle = NativeMethods.GetActiveWindow();
            _originalWindowStyle = NativeMethods.GetWindowLong(_windowHandle, GWL_EXSTYLE);
        }

        public void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            _overlayWidth = src.width;
            _overlayHeight = src.height;

            if (_renderTextureHandle == IntPtr.Zero)
            {
                _renderTextureHandle = src.GetNativeTexturePtr();
            }
            
            Graphics.Blit(src, dest);
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (!_isOverlayActive)
                {
                    // Make this a layered window
                    NativeMethods.SetWindowLong(_windowHandle, GWL_EXSTYLE, new IntPtr(_originalWindowStyle.ToInt32() ^ WS_EX_LAYERED));

                    // Topmost
                    NativeMethods.SetWindowPos(_windowHandle, NativeMethods.Hwnd.TopMost, 0, 0, 0, 0, NativeMethods.SetWindowPosFlags.IgnoreMove | NativeMethods.SetWindowPosFlags.IgnoreResize);

                    IntPtr screenDeviceContext = NativeMethods.GetDC(IntPtr.Zero);
                    IntPtr memoryDeviceContext = NativeMethods.CreateCompatibleDC(screenDeviceContext);
                    IntPtr bitmapHandle = IntPtr.Zero;
                    IntPtr oldBitmapHandle = IntPtr.Zero;

                    try
                    {
                        // Get dimensions
                        int width = _overlayWidth;
                        int height = _overlayHeight;

                        // Make mem DC + mem  bitmap
                        bitmapHandle = NativeMethods.CreateCompatibleBitmap(screenDeviceContext, width, height);
                        oldBitmapHandle = NativeMethods.SelectObject(memoryDeviceContext, bitmapHandle);

                        // Draw image to memory DC
                        // TODO: How to pull this off?
                        bool result = NativeMethods.BitBlt(memoryDeviceContext, 0, 0, _overlayWidth, _overlayHeight, _renderTextureHandle, 0, 0, NativeMethods.TernaryRasterOperations.SRCCOPY);
                        //img.Draw(memoryDeviceContext, 0, 0, iWidth, iHeight, 0, 0, iWidth, iHeight);

                        // Call UpdateLayeredWindow
                        NativeMethods.BlendFunction blend = new NativeMethods.BlendFunction
                        {
                            BlendFlags = 0,
                            BlendOp = AC_SRC_OVER,
                            SourceConstantAlpha = 255,
                            AlphaFormat = AC_SRC_ALPHA
                        };
                        NativeMethods.Point ptPos = new NativeMethods.Point(0, 0);
                        NativeMethods.Size sizeWnd = new NativeMethods.Size(width, height);
                        NativeMethods.Point ptSrc = new NativeMethods.Point(0, 0);

                        NativeMethods.UpdateLayeredWindow(_windowHandle, screenDeviceContext, ref ptPos, ref sizeWnd, memoryDeviceContext, ref ptSrc, 0, ref blend, ULW_ALPHA);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    finally
                    {
                        NativeMethods.ReleaseDC(IntPtr.Zero, screenDeviceContext);

                        if (bitmapHandle != IntPtr.Zero)
                        {
                            NativeMethods.SelectObject(memoryDeviceContext, oldBitmapHandle);
                            NativeMethods.DeleteObject(bitmapHandle);
                        }

                        NativeMethods.DeleteDC(memoryDeviceContext);
                    }
                }
                else
                {
                    // Restore original style
                    NativeMethods.SetWindowLong(_windowHandle, GWL_EXSTYLE, _originalWindowStyle);
                }

                _isOverlayActive = !_isOverlayActive;
            }
        }
    }
}