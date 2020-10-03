// modified from Elektrix's Tree and Vehicle Props
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NonTerrainConformingProps
{
    public class Enumerations
    {
        public static IEnumerator CreateClones()
        {
            int count = PrefabCollection<PropInfo>.LoadedCount();
            for (uint i = 0; i < count; i++) { 
                Generator.GenerateNTCProp(PrefabCollection<PropInfo>.GetLoaded(i));
                yield return null;
            }
            yield return 0;
        }

        public static IEnumerator InitializeAndBindClones()
        {
            yield return null;
            PrefabCollection<PropInfo>.InitializePrefabs("NTC Prop", Mod.cloneMap.Select((KeyValuePair<PropInfo, PropInfo> k) => k.Key).ToArray(), null);
            yield return null;
            PrefabCollection<PropInfo>.BindPrefabs();
            yield return null;
        }
    }
}