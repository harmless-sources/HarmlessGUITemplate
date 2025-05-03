using HarmonyLib;
using UnityEngine;

namespace Harmless.Utill
{
    public class HarmonyPatches : MonoBehaviour
    {
        public static Harmony harmony;
        private void Awake()
        {
            harmony = new Harmony("HamlessEZ");
            harmony.PatchAll();
        }
    }
}