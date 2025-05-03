using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Harmless.ToggleManager
{
    [Serializable]
    public class ValueConfig
    {
        public List<string> BoolToggleKeys = new List<string>();
        public List<bool> BoolToggleValues = new List<bool>();
        public List<string> FloatValueKeys = new List<string>();
        public List<float> FloatValueValues = new List<float>();
        public List<string> ColorValueKeys = new List<string>();
        public List<Color> ColorValueValues = new List<Color>();
    }

    public static class SaveConfig
    {
        private static ValueConfig current = new ValueConfig();
        private static string FileSave = Path.Combine(Path.GetDirectoryName(Application.dataPath), "Harmlessconfig.json");

        public static void Save() => SaveConfigMeth();
        public static void Load() => LoadConfigMeth();

        private static void SaveConfigMeth()
        {
            current.BoolToggleKeys = GetType(typeof(bool));
            current.FloatValueKeys = GetType(typeof(float));
            current.ColorValueKeys = GetType(typeof(Color));
            current.BoolToggleValues = current.BoolToggleKeys.Select(k => (bool)typeof(Fields).GetField(k).GetValue(null)).ToList();
            current.FloatValueValues = current.FloatValueKeys.Select(k => (float)typeof(Fields).GetField(k).GetValue(null)).ToList();
            current.ColorValueValues = current.ColorValueKeys.Select(k => (Color)typeof(Fields).GetField(k).GetValue(null)).ToList();
            try { File.WriteAllText(FileSave, JsonUtility.ToJson(current, true)); }
            catch (Exception e) { Debug.LogError($"Error saving config: {e.Message}"); }
        }

        private static void LoadConfigMeth()
        {
            if (File.Exists(FileSave))
            {
                try
                {
                    current = JsonUtility.FromJson<ValueConfig>(File.ReadAllText(FileSave));
                    for (int i = 0; i < current.BoolToggleKeys.Count; i++)
                    {
                        var field = typeof(Fields).GetField(current.BoolToggleKeys[i]);
                        if (field?.FieldType == typeof(bool))
                            field.SetValue(null, current.BoolToggleValues[i]);
                    }

                    for (int i = 0; i < current.FloatValueKeys.Count; i++)
                    {
                        var field = typeof(Fields).GetField(current.FloatValueKeys[i]);
                        if (field?.FieldType == typeof(float))
                            field.SetValue(null, current.FloatValueValues[i]);
                    }
                    for (int i = 0; i < current.ColorValueKeys.Count; i++)
                    {
                        var field = typeof(Fields).GetField(current.ColorValueKeys[i]);
                        if (field?.FieldType == typeof(Color))
                            field.SetValue(null, current.ColorValueValues[i]);
                    }
                }
                catch (Exception e) { Debug.LogError($"Error loading config: {e.Message}"); }
            }
            else Debug.LogWarning("Config file not found.");
        }
        public static void TurnOffAllMods()
        {
            foreach (var field in typeof(Fields).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (field.FieldType == typeof(bool))
                {
                    field.SetValue(null, false);
                }
            }
        }
        private static List<string> GetType(Type Type) => typeof(Fields).GetFields(BindingFlags.Public | BindingFlags.Static).Where(f => f.FieldType == Type).Select(f => f.Name).ToList();
    }
}