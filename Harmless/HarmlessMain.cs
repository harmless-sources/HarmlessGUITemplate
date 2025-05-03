using static Harmless.Texture.TextureFields;
using static Harmless.Texture.DrawTexture;
using Harmless.Texture;
using UnityEngine;
using Harmless.TabHandle;
using Harmless.ToggleManager;
using System.Collections.Generic;

namespace Harmless
{
    public class HarmlessMain : MonoBehaviour
    {
        public static Rect windowRect = new Rect(50f, 50f, 700f, 450f);
        private int SelectedTab = 0;
        public static string[] tabs = { "Visual", "Exploits", "Player", "Players", "Items", "Monsters", "Settings" };
        public static List<TogglesLoad> TogglesLoad;
        Texture2D[] TabIcons = new Texture2D[tabs.Length];

        private float Fade = 1f;
        private float FadeByte = 1f;
        private float FadeSpeed = 5f;
        private float[] TabHover = new float[tabs.Length];

        void Start()
        {
            GuiBackground = DrawTexture.CreateTexture(700, 450, Color32(1, 1, 1));
            TabNormal = BoxTexture = DrawTexture.CreateTexture(700, 450, Color32(16, 16, 16));
            SliderTexture = DrawTexture.CreateTexture(23, 23, Color32(41, 79, 148));
            SliderBallTexture = DrawTexture.CreateTexture(23, 23, Color32(93, 140, 227));
            ButtonTexture = DrawTexture.CreateTexture(23, 23, Color32(29, 29, 29));
            NormalToggleTexture = DrawTexture.CreateTexture(26, 26, Color32(29, 29, 29));
            ActiveToggleTexture = DrawTexture.CreateTexture(26, 26, Color32(33, 33, 33));
            TextFieldTexture = DrawTexture.CreateTexture(23, 24, Color32(36, 36, 36));

            /* Icons */
            TabIcons[0] = BaseToTexture(EyeIconBase64);        // Visual
            TabIcons[1] = BaseToTexture(ExploitsIconBase64);   // Exploits
            TabIcons[2] = BaseToTexture(PlayerIconBase64);     // Player
            TabIcons[3] = BaseToTexture(PlayersIconBase64);    // Players
            TabIcons[4] = BaseToTexture(ItemsIconBase64);      // Items
            TabIcons[5] = BaseToTexture(MonstersIconBase64);   // Monsters
            TabIcons[6] = BaseToTexture(SettingsIcon64);       // Settings
        }
        void OnGUI()
        {
            FadeByte = Mathf.Lerp(FadeByte, Fade, Time.deltaTime * FadeSpeed);
            DrawTexture.DrawTextureRounded(windowRect, GuiBackground, new(12f, 12f, 12f, 12f));
            windowRect = GUI.Window(0, windowRect, Window, "", GUIStyle.none);

            if (FadeByte < 0.99f)
                GUI.FocusControl(null); /* Repaint */
            TogglesLoad?.ForEach(t => t?.OnGUI());
        }

        void Window(int ID)
        {
            GUI.DragWindow(new Rect(0, 0, windowRect.width, 20));

            DrawTexture.DrawTextureRounded(new Rect(-1.4f, 0f, 145, windowRect.height + 14), BoxTexture, new(10, 10, 10, 10));

            GUI.Label(new Rect(10, 22, 130, 30), "<size=16><b>Harmless</b></size>", new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                richText = true,
                normal = { textColor = Color.white }
            });

            GUILayout.BeginArea(new Rect(10, 55, 130, windowRect.height - 65));
            for (int i = 0; i < tabs.Length; i++)
            {
                GUILayout.BeginHorizontal(GUILayout.Height(26));

                GUILayout.Space(TabHover[i]);

                if (TabIcons != null && i < TabIcons.Length && TabIcons[i] != null)
                {
                    Rect IconRect = GUILayoutUtility.GetRect(23, 23, GUILayout.Width(20), GUILayout.Height(20));
                    IconRect.y += 7f;
                    IconRect.x += 6f;
                    GUI.DrawTexture(IconRect, TabIcons[i], ScaleMode.ScaleToFit, true);
                    GUILayout.Space(6);
                }

                if (GUILayout.Button(tabs[i], new GUIStyle(GUI.skin.button)
                {
                    fixedHeight = 26,
                    alignment = TextAnchor.MiddleLeft,
                    fontSize = 16,
                    normal = new GUIStyleState { background = TabNormal, textColor = Color.white },
                    hover = new GUIStyleState { background = TabNormal, textColor = Color.white },
                    active = new GUIStyleState { background = TabNormal, textColor = Color.white }
                }))
                {
                    if (SelectedTab != i)
                    {
                        SelectedTab = i;
                        FadeByte = 0f;
                        Fade = 1f;
                    }
                }

                Rect Rect = GUILayoutUtility.GetLastRect();
                bool Hovering = Rect.Contains(Event.current.mousePosition) && !Input.GetMouseButton(0);
                TabHover[i] = Mathf.Lerp(TabHover[i], Hovering ? 10f : 0f, Time.deltaTime * 10f);

                GUILayout.EndHorizontal();
            }

            GUILayout.EndArea();

            GUILayout.BeginArea(new Rect(150, 30, windowRect.width - 160, windowRect.height - 40));

            Color original = GUI.color;
            GUI.color = new Color(original.r, original.g, original.b, FadeByte);

            switch (SelectedTab)
            {
                case 0: VisualTab.Visual(); break;
                case 1: break;
                case 2: break;
            }

            GUI.color = original;
            GUILayout.EndArea();
        }

        public static void Update() => TogglesLoad.ForEach(t => t.Update());
        public static void FixedUpdate() => TogglesLoad?.ForEach(t => t?.FixedUpdate());
    }
}
