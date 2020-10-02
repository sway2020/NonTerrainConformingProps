using CitiesHarmony.API;
using HarmonyLib;
using ICities;
using System.Reflection;
using ColossalFramework;
using System.Collections.Generic;
using System;
using ColossalFramework.UI;
using UnityEngine;

namespace NonTerrainConformingProps
{
    public class Mod : IUserMod
    {
        public string Name => "Non-Terrain Conforming Props";
        public string Description => "Generate a \"non-terrain conforming\" version of each terrain-conforming prop";

        public static List<PropInfo> tcProps = new List<PropInfo>();
        public static Dictionary<PropInfo, PropInfo> cloneMap = new Dictionary<PropInfo, PropInfo>();
        public static bool PrefabsInitialized = false;

        public void OnEnabled()
        {
            HarmonyHelper.DoOnHarmonyReady(() => Patcher.PatchAll());
            XMLUtils.LoadSettings();
        }

        public void OnDisabled()
        {
            if (HarmonyHelper.IsHarmonyInstalled) Patcher.UnpatchAll();
        }
      
        public void OnSettingsUI(UIHelperBase helper)
        {
            try
            {

                UIHelper group = helper.AddGroup(Name) as UIHelper;
                UIPanel panel = group.self as UIPanel;

                // Disable debug messages logging
                UICheckBox checkBox = (UICheckBox)group.AddCheckbox("Skip terrain-conforming vanilla props", Settings.skipVanillaProps, (b) =>
                {
                    Settings.skipVanillaProps = b;
                    XMLUtils.SaveSettings();
                });
                checkBox.tooltip = "Generated \"non-terrain conforming\" vanilla props will disappear next time when a save file is loaded";
                group.AddSpace(10);
            }
            catch (Exception e)
            {
                Debug.Log("OnSettingsUI failed");
                Debug.LogException(e);
            }
        }

        public static class Patcher
        {
            private const string HarmonyId = "sway.NonTerrainConformingProps";

            private static bool patched = false;

            public static void PatchAll()
            {
                if (patched) return;

                UnityEngine.Debug.Log("Non-Terrain Conforming Props: Patching...");

                patched = true;

                // Harmony.DEBUG = true;
                var harmony = new Harmony("sway.NonTerrainConformingProps");
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }

            public static void UnpatchAll()
            {
                if (!patched) return;

                var harmony = new Harmony(HarmonyId);
                harmony.UnpatchAll(HarmonyId);

                patched = false;

                UnityEngine.Debug.Log("Non-Terrain Conforming Props: Reverted...");
            }

        }

        [HarmonyPatch(typeof(RenderManager), "Managers_CheckReferences")]
        public static class LoadingHook
        {
            public static void Prefix()
            {
                if (!Mod.PrefabsInitialized)
                {
                    Mod.PrefabsInitialized = true;
                    Singleton<LoadingManager>.instance.QueueLoadingAction(Enumerations.CreateClones());
                    Singleton<LoadingManager>.instance.QueueLoadingAction(Enumerations.InitializeAndBindClones());
                }
            }
        }

    }

}
