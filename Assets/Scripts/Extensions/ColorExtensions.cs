using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class ColorExtensions
    {
        public static uint ToUint(this Color32 c)
        {
            return (uint)(((c.a << 24) | (c.r << 16) | (c.g << 8) | c.b) & 0xffffffffL);
        }

        public static uint ToUintRgb(this Color32 c)
        {
            return (uint)(((c.r << 16) | (c.g << 8) | c.b) & 0xffffffffL);
        }
    }
}
