using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Harmless.Texture
{
    internal class DrawTexture
    {
        public static void DrawTextureRounded(Rect Rect, UnityEngine.Texture texture, Vector4 Roundness) => GUI.DrawTexture(Rect, texture, ScaleMode.StretchToFill, true, 1.0f, Color.white, Vector4.zero, Roundness);
        public static void DrawTextureRounded(Rect position, UnityEngine.Texture texture, ScaleMode scaleMode, bool alphaBlend, float imageAspect, Color color, Vector4 borderWidths, Vector4 borderRadiuses)
        => GUI.DrawTexture(position, texture, scaleMode, alphaBlend, imageAspect, color, borderWidths, borderRadiuses);
        public static Rect GetRect(float width, float height, params GUILayoutOption[] options) => GUILayoutUtility.GetRect(width, height, GUIStyle.none, options);

        public static Func<int, int, Color, Texture2D> CreateTexture = (width, height, col) =>
        {
            Func<int, Color[]> Pixels = size => Enumerable.Repeat(col, size).ToArray();
            Texture2D tex = new Texture2D(width, height);
            tex.SetPixels(Pixels(width * height));
            tex.Apply();
            return tex;
        };
        public static Color32 HexToColor32(string hex, byte value = 255)
        {
            hex = hex.StartsWith("#") ? hex.Substring(1) : hex;
            byte r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
            if (hex.Length == 8)
            {
                byte a = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
                return new Color32(r, g, b, a);
            }
            else
            {
                return new Color32(r, g, b, value);
            }
        }
        public static Color32 Color32(byte R, byte G, byte B, byte A = byte.MaxValue)
        {
            return new Color32(R, G, B, A);
        }
        public static Color HexToColor(string hex)
        {
            hex = hex.StartsWith("#") ? hex.Substring(1) : hex;
            float r = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber) / 255f;
            float g = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber) / 255f;
            float b = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber) / 255f;

            if (hex.Length == 8)
            {
                float a = int.Parse(hex.Substring(6, 2), NumberStyles.HexNumber) / 255f;
                return new Color(r, g, b, a);
            }
            else
            {
                return new Color(r, g, b, 1f);
            }
        }
        public static Func<string, Texture2D> BaseToTexture = s =>
        {
            var texture = new Texture2D(1, 1, TextureFormat.ARGB32, true, true)
            {
                hideFlags = HideFlags.HideAndDontSave,
                filterMode = FilterMode.Bilinear
            };
            texture.LoadImage(System.Convert.FromBase64String(s), true);
            return texture;
        };
    }
}
