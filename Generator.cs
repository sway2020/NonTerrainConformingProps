// modified from Elektrix's Tree and Vehicle Props
using System.Collections.Generic;
using ICities;
using UnityEngine;

namespace NonTerrainConformingProps
{
    public class Generator : LoadingExtensionBase
    {
        private static readonly Shader propFenceShader = Shader.Find("Custom/Props/Prop/Fence");
        private static readonly Shader propDefaultShader = Shader.Find("Custom/Props/Prop/Default");

        public static void GenerateNTCProp(PropInfo prop)
        {
            if (prop.m_material?.shader != propFenceShader) return;
            if (Settings.skipVanillaProps && !prop.m_isCustomContent) return;
            if (Mod.skippedDictionary.ContainsKey(prop.name) && Mod.skippedDictionary[prop.name]) return;

            PropInfo propInfo = CloneProp(prop);
            ((UnityEngine.Object)propInfo).name = (((UnityEngine.Object)prop).name.Replace("_Data", "") + " NTCP_Data");

            if (propInfo.m_material != null)
            {
                propInfo.m_material.shader = propDefaultShader;
            }

            Mod.cloneMap.Add(propInfo, prop);
            if (!Mod.skippedDictionary.ContainsKey(prop.name))
            {
                Settings.skippedEntries.Add(new SkippedEntry(prop.name));
            }
        }

        public static PropInfo CloneProp(PropInfo prop)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate(prop.gameObject);
            gameObject.SetActive(value: false);
            PrefabInfo component = gameObject.GetComponent<PrefabInfo>();
            component.m_isCustomContent = prop.m_isCustomContent;
            return gameObject.GetComponent<PropInfo>();
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            foreach (KeyValuePair<PropInfo, PropInfo> keyValuePair in Mod.cloneMap)
            {
                PropInfo key = keyValuePair.Key;
                PropInfo value = keyValuePair.Value;

                if (value.m_material != null)
                {
                    key.m_material = UnityEngine.Object.Instantiate<Material>(value.m_material);
                    key.m_material.shader = propDefaultShader;
                }

                if (value.m_lodMaterial != null)
                {
                    key.m_lodMaterial = UnityEngine.Object.Instantiate<Material>(value.m_lodMaterial);
                    key.m_lodMaterial.shader = propDefaultShader;
                    key.m_lodMaterialCombined = UnityEngine.Object.Instantiate<Material>(value.m_lodMaterialCombined);
                    key.m_lodMaterialCombined.shader = propDefaultShader;
                    key.m_lodColors = value.m_lodColors;
                }
                key.m_generatedInfo = UnityEngine.Object.Instantiate<PropInfoGen>(key.m_generatedInfo);
                key.m_generatedInfo.name = key.name;
                key.m_generatedInfo.m_propInfo = key;
            }

            XMLUtils.SaveSettings();
        }

        public override void OnLevelUnloading()
        {
            Mod.PrefabsInitialized = false;
        }
    }
}
