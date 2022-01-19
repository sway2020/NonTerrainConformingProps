// a simple modtools script to change prop shader
var shader = Shader.Find("Custom/Props/Prop/Default");
var asset = ToolsModifierControl.toolController.m_editPrefabInfo as PropInfo;
if (asset.m_material != null) asset.m_material.shader = shader;
if (asset.m_lodMaterial != null) asset.m_lodMaterial.shader = shader;
if (asset.m_lodMaterialCombined != null) asset.m_lodMaterialCombined.shader = shader;