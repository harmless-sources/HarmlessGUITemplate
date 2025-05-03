using UnityEngine;
using static Harmless.Texture.TextureFields;
using static Harmless.Texture.MainTexture;
using static Harmless.ToggleManager.Fields;
using Harmless.ToggleManager;
using Harmless.Texture;

namespace Harmless.TabHandle
{
    public class VisualTab
    {
        public static float Ezzz;
        public static string Ezzzz;

        public static void Visual()
        {
            CreateBox("Visual", () =>
            {
                CreateToggle(ref Test, "i hate ts", "Did you know i hate ts");
                CreateButton("Ezzz", () => { Debug.Log("Ezzz"); });
                Ezzz = CreateSlider("Efffzzz", Ezzz, 10, 100);
                 CreateTextField("ssss", ref Ezzzz);
            });
            CreateRightBox("Settings", () =>
            {
            });
        }
    }
}
