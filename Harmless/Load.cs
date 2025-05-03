using Harmless.Texture;
using Harmless.ToggleManager;
using Harmless.Utill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Harmless.Loader
{
    public class Loader : MonoBehaviour
    {
        public static GameObject GameObj;
        public static void Load()
        {
            GameObj = new GameObject();
            var comps = new System.Type[]
            {
              typeof(HarmlessMain), typeof(TogglesLoad), typeof(HarmonyPatches)
            };

            foreach (var component in comps) GameObj.AddComponent(component);
            DontDestroyOnLoad(GameObj);
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Namespace?.Contains("Harmless.Modules") == true && t.IsSubclassOf(typeof(MonoBehaviour)));
            if (types != null)
            {
                foreach (var type in types) GameObj.AddComponent(type);
            }
        }
    }
}
