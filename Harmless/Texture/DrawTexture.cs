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
        public static Color32 HexToColor32(string hex)
        {   
            if (ColorUtility.TryParseHtmlString(hex, out Color color))
             return (Color32)color;
              return new Color32(255, 255, 255, 255);
        }
        public static Color32 Color32(byte R, byte G, byte B, byte A = byte.MaxValue)
        {
            return new Color32(R, G, B, A);
        }
        public static Color HexToColor(string hex)
        {       
              if (ColorUtility.TryParseHtmlString(hex, out Color color))
               return color;
                 return Color.white;
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
