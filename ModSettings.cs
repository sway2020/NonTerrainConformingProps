// Originally written by algernon for Find It 2.
using System.Xml.Serialization;

namespace NonTerrainConformingProps
{
    /// <summary>
    /// Class to hold global mod settings.
    /// </summary>
    [XmlRoot(ElementName = "NonTerrainConformingProps", Namespace = "", IsNullable = false)]
    internal static class Settings
    {
        internal static bool skipVanillaProps = false;
    }

    /// <summary>
    /// Defines the XML settings file.
    /// </summary>
    [XmlRoot(ElementName = "NonTerrainConformingProps", Namespace = "", IsNullable = false)]
    public class XMLSettingsFile
    {
        [XmlElement("SkipVanillaProps")]
        public bool SkipVanillaProps { get => Settings.skipVanillaProps; set => Settings.skipVanillaProps = value; }
    }
}