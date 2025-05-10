using static Harmless.Texture.TextureFields;
using UnityEngine;
using System;

namespace Harmless.Texture
{
    public static class MainTexture
    {
        public static bool CreateToggle(ref bool value, string text, string tooltip = "")
        {
            GUILayout.BeginHorizontal();
            Rect rect = DrawTexture.GetRect(28, 27, GUILayout.ExpandWidth(false));

            DrawTexture.DrawTextureRounded(rect, value ? NormalToggleTexture : ActiveToggleTexture, ScaleMode.StretchToFill, true, 1f, Color.white, Vector4.zero, new Vector4(4f, 4f, 4f, 4f));
            if (value) GUI.Label(new Rect(rect.x + 5, rect.y + 4, 20f, 20f), "X", new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 17 });
            if (GUI.Button(new Rect(rect.x + 5, rect.y + 6f, 25f, 22f), new GUIContent("", tooltip), GUIStyle.none))
            {
                value = !value;
            }
            GUI.Label(new Rect(rect.x + 45, rect.y + 2f, 1000, 24), new GUIContent($"<b>{text}</b>", tooltip), new GUIStyle(GUI.skin.label) { fontSize = 14, richText = true });

            GUILayout.EndHorizontal();
            GUILayout.Space(3f);

            if (!string.IsNullOrEmpty(GUI.tooltip))
            {
                Vector2 size = GUI.skin.box.CalcSize(new GUIContent(GUI.tooltip));
                GUI.Box(new Rect(Event.current.mousePosition.x + 15, Event.current.mousePosition.y, size.x + 8, size.y + 4), GUI.tooltip);
            }

            return value;
        }

                static float Scale = 1f;
        static bool Pressed = false;

        static Dictionary<int, (float Scale, bool Pressed)> Buttons = new(); /* gonna use ids so animations dont play on all */

        public static void CreateButton(string text, System.Action Act)
        {
            Rect GetRect = DrawTexture.GetRect(320, 40);
            Vector2 center = new(GetRect.x + 80, GetRect.y + 20);

            int hashid = $"{text}y{GetRect.y}".GetHashCode(); /* Hash Id */
            if (!Buttons.ContainsKey(hashid))
                Buttons[hashid] = (1f, false);

            var state = Buttons[hashid];
            state.Scale = Mathf.Lerp(state.Scale, state.Pressed ? 0.95f : 1f, 0.2f);

            Rect ButtonRect = new Rect(center.x - 80 * state.Scale, center.y - 12.5f * state.Scale, 160 * state.Scale, 25f * state.Scale);
            DrawTexture.DrawTextureRounded(ButtonRect, ButtonTexture, ScaleMode.StretchToFill, true, 1f, Color.white, Vector4.zero, new Vector4(6, 6, 6, 6));

            Event Event = Event.current; 
            if (Event.type == EventType.MouseDown && ButtonRect.Contains(Event.mousePosition))
                state.Pressed = true;
            if (Event.type == EventType.MouseUp)
            {
                if (state.Pressed && ButtonRect.Contains(Event.mousePosition))
                    Act?.Invoke();
                state.Pressed = false;
            }
            GUI.Label(ButtonRect, $"<b>{text}</b>", new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                richText = true,
                normal = { textColor = Color.white }
            });

            Buttons[hashid] = state;
            GUILayout.Space(3);
        }
        public static float CreateSlider(string Text, float value, float min, float max)
        {
            GUILayout.BeginHorizontal(GUILayout.Width(230));

            GUILayout.BeginVertical(GUILayout.Width(55));
            var TextStyle = new GUIStyle(GUI.skin.label) { fontSize = 13, normal = { textColor = Color.white } };
            GUILayout.Label(Text, TextStyle);
            GUILayout.Space(18);
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.Width(120));
            Rect SliderRect = DrawTexture.GetRect(1f, 30f);

            float Filler = Mathf.Lerp(0, SliderRect.width - 14f, Mathf.InverseLerp(min, max, value));

            GUI.color = Color.black;
            GUI.DrawTexture(new Rect(SliderRect.x, SliderRect.y + 14f, SliderRect.width, 4f), Texture2D.whiteTexture);

            GUI.color = new Color(0.3f, 0.5f, 1f);
            GUI.DrawTexture(new Rect(SliderRect.x, SliderRect.y + 14f, Filler, 4f), Texture2D.whiteTexture);

            GUI.color = Color.white;

            Rect BallRect = new Rect(SliderRect.x + Filler, SliderRect.y + 9f, 14f, 14f);
            DrawTexture.DrawTextureRounded(BallRect, SliderBallTexture, ScaleMode.StretchToFill, true, 1f, new Color(0.3f, 0.5f, 1f), Vector4.zero, new Vector4(7, 7, 7, 7));

            if ((Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag) && SliderRect.Contains(Event.current.mousePosition))
            {
                value = Mathf.Lerp(min, max, Mathf.Clamp01((Event.current.mousePosition.x - SliderRect.x) / (SliderRect.width - 14f)));
            }

            GUILayout.EndVertical();

            var ValueStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft, fontSize = 12, normal = { textColor = Color.gray } };
            GUILayout.Label($"{Mathf.RoundToInt(value)}°", ValueStyle, GUILayout.Width(30), GUILayout.Height(30));

            GUILayout.EndHorizontal();
            GUILayout.Space(6f);
            return value;
        }
        public static void CreateTextField(string label, ref string value)
        { /*  Would make this better but i just wanna rush this  */
            var r = GUILayoutUtility.GetRect(260, 20); r.y -= 12;
            var style = new GUIStyle(GUI.skin.label) { fontSize = 13 };
            GUI.Label(new Rect(r.x, r.y, 70f, 20), label, style);
            value = GUI.TextField(new Rect(r.x + 70f + 2f, r.y, 145, 20), value);
        }


        public static void CreateBox(string title, System.Action Act)
        {
            Rect BoxRect = new Rect(23f, 8f, 238f, 300f); 
            DrawTexture.DrawTextureRounded(BoxRect, BoxTexture, ScaleMode.StretchToFill, true, 1f, Color.white, Vector4.zero, new Vector4(6f, 6f, 6f, 6f));

            GUI.Label(new Rect(BoxRect.x + 10f, BoxRect.y + 2, BoxRect.width - 20f, 30f), "<size=16><b>" + title + "</b></size>");

            GUILayout.BeginArea(new Rect(BoxRect.x + 10f, BoxRect.y + 30f, BoxRect.width - 20f, BoxRect.height - 40f));
            Act();
            GUILayout.EndArea();
        }
        public static void CreateRightBox(string title, System.Action Act)
        {
            Rect RightBoxRect = new Rect(281f, 8f, 238f, 300f); 
            DrawTexture.DrawTextureRounded(RightBoxRect, BoxTexture, ScaleMode.StretchToFill, true, 1f, Color.white, Vector4.zero, new Vector4(4f, 4f, 4f, 4f));

            GUI.Label(new Rect(RightBoxRect.x + 10f, RightBoxRect.y + 2, RightBoxRect.width - 20f, 30f), "<size=16><b>" + title + "</b></size>");

            GUILayout.BeginArea(new Rect(RightBoxRect.x + 10f, RightBoxRect.y + 30f, RightBoxRect.width - 20f, RightBoxRect.height - 40f));
            Act();
            GUILayout.EndArea();
        }
    }
}
