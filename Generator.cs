using System;
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
            // Debug.Log("Generated NTCP version of: " + prop.name);
            PropInfo propInfo = CloneProp(prop);

            ((UnityEngine.Object)propInfo).name = (((UnityEngine.Object)prop).name.Replace("_Data", "") + " NTCP_Data");

            if (propInfo.m_material != null)
			{
				propInfo.m_material.shader = propDefaultShader;
			}
			Mod.cloneMap.Add(propInfo, prop);
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
                /*
                Debug.Log($"m_lodRenderDistance key{key.m_lodRenderDistance} value{value.m_lodRenderDistance}");
                key.m_lodRenderDistance = value.m_lodRenderDistance;
                Debug.Log($"m_maxRenderDistance key{key.m_maxRenderDistance} value{value.m_maxRenderDistance}");
                key.m_maxRenderDistance = value.m_maxRenderDistance;
                */
                key.m_generatedInfo = UnityEngine.Object.Instantiate<PropInfoGen>(key.m_generatedInfo);
                key.m_generatedInfo.name = key.name;
                key.m_generatedInfo.m_propInfo = key;
                /*
                Debug.Log($"ntcp m_uvmapArea key{key.m_generatedInfo.m_uvmapArea} value{value.m_generatedInfo.m_uvmapArea}");
                key.m_generatedInfo.m_uvmapArea = value.m_generatedInfo.m_uvmapArea;
                Debug.Log($"ntcp m_triangleArea key{key.m_generatedInfo.m_triangleArea} value{value.m_generatedInfo.m_triangleArea}");
                key.m_generatedInfo.m_triangleArea = value.m_generatedInfo.m_triangleArea;
                if (key.m_mesh != null)
                {
                    Debug.Log($"ntcp m_size key{key.m_generatedInfo.m_size} value{Vector3.one * (Math.Max(key.m_mesh.bounds.extents.x, Math.Max(key.m_mesh.bounds.extents.y, key.m_mesh.bounds.extents.z)) * 2f - 1f)}");
                    key.m_generatedInfo.m_size = Vector3.one * (Math.Max(key.m_mesh.bounds.extents.x, Math.Max(key.m_mesh.bounds.extents.y, key.m_mesh.bounds.extents.z)) * 2f - 1f);
                }
                if (key.m_material != null)
                {
                    key.m_material.SetColor("_ColorV0", key.m_color0);
                    key.m_material.SetColor("_ColorV1", key.m_color1);
                    key.m_material.SetColor("_ColorV2", key.m_color2);
                    key.m_material.SetColor("_ColorV3", key.m_color3);
                }
                if (key.m_lodMaterial != null)
                {
                    key.m_lodMaterial.SetColor("_ColorV0", key.m_color0);
                    key.m_lodMaterial.SetColor("_ColorV1", key.m_color1);
                    key.m_lodMaterial.SetColor("_ColorV2", key.m_color2);
                    key.m_lodMaterial.SetColor("_ColorV3", key.m_color3);
                }
                */
            }
        }

        public override void OnLevelUnloading()
		{
			Mod.PrefabsInitialized = false;
		}
	}
}
