using CitiesHarmony.API;
using HarmonyLib;
using ICities;
using System.Reflection;
using ColossalFramework;
using System.Collections.Generic;
using System;
using ColossalFramework.UI;
using UnityEngine;
using System.IO;
using ColossalFramework.IO;

namespace NonTerrainConformingProps
{
    public class Mod : IUserMod
    {
        public const string version = "1.1.6";
        public string Name => "Non-Terrain Conforming Props " + version;
        public string Description
        {
            get { return Translations.Translate("NTCP_DESC"); }
        }

        public static List<PropInfo> tcProps = new List<PropInfo>();
        public static Dictionary<PropInfo, PropInfo> cloneMap = new Dictionary<PropInfo, PropInfo>();
        public static bool PrefabsInitialized = false;
        public static Dictionary<string, bool> skippedDictionary = new Dictionary<string, bool>();
        public static HashSet<PropInfo> generatedNTCPProp = new HashSet<PropInfo>();

        public void OnEnabled()
        {
            HarmonyHelper.DoOnHarmonyReady(() => Patcher.PatchAll());
            XMLUtils.LoadSettings();
            foreach (SkippedEntry entry in Settings.skippedEntries)
            {
                if (skippedDictionary.ContainsKey(entry.name)) continue;
                skippedDictionary.Add(entry.name, entry.skipped);
            }
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

                UICheckBox checkBox = (UICheckBox)group.AddCheckbox(Translations.Translate("NTCP_SET_VAN"), Settings.skipVanillaProps, (b) =>
                {
                    Settings.skipVanillaProps = b;
                    XMLUtils.SaveSettings();
                });
                checkBox.tooltip = Translations.Translate("NTCP_SET_VANTP");
                group.AddSpace(10);

                // languate settings
                UIDropDown languageDropDown = (UIDropDown)group.AddDropdown(Translations.Translate("TRN_CHOICE"), Translations.LanguageList, Translations.Index, (value) =>
                {
                    Translations.Index = value;
                    XMLUtils.SaveSettings();
                });

                languageDropDown.width = 300;
                group.AddSpace(10);

                // show path to NonTerrainConformingPropsConfig.xml
                string path = Path.Combine(DataLocation.executableDirectory, "NonTerrainConformingPropsConfig.xml");
                UITextField customTagsFilePath = (UITextField)group.AddTextfield(Translations.Translate("NTCP_SET_CONF") + " - NonTerrainConformingPropsConfig.xml", path, _ => { }, _ => { });
                customTagsFilePath.width = panel.width - 30;
                group.AddButton(Translations.Translate("NTCP_SET_CONFFE"), () => System.Diagnostics.Process.Start(DataLocation.executableDirectory));
            
            }
            catch (Exception e)
            {
                Debug.Log("OnSettingsUI failed");
                Debug.LogException(e);
            }
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
