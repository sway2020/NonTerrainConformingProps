// Originally written by algernon for Find It 2.
// Modified by sway
using System.Collections.Generic;
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

        internal static List<SkippedEntry> skippedEntries = new List<SkippedEntry>();
    }

    /// <summary>
    /// Defines the XML settings file.
    /// </summary>
    [XmlRoot(ElementName = "NonTerrainConformingProps", Namespace = "", IsNullable = false)]
    public class XMLSettingsFile
    {
        [XmlElement("SkipVanillaProps")]
        public bool SkipVanillaProps { get => Settings.skipVanillaProps; set => Settings.skipVanillaProps = value; }

        [XmlArray("SkippedEntries")]
        [XmlArrayItem("SkippedEntry")]
        public List<SkippedEntry> SkippedEntries { get => Settings.skippedEntries; set => Settings.skippedEntries = value; }
    }

    public class SkippedEntry
    {
        [XmlAttribute("Name")]
        public string name = "";

        [XmlAttribute("Skipped")]
        public bool skipped = false;

        public SkippedEntry() { }

        public SkippedEntry(string newName) { name = newName; }
    }
}