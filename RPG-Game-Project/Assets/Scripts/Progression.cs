using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression")]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] progressionCharacterClassArray;

        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            foreach (ProgressionCharacterClass progressionCharacterClass in progressionCharacterClassArray)
            {
                if (progressionCharacterClass.characterClass != characterClass) continue;

                foreach (ProgressionStat progressionStat in progressionCharacterClass.stats)
                {
                    if (progressionStat.stat != stat) continue;
                    if (progressionStat.levels.Length < level) continue;

                    return progressionStat.levels[level - 1];
                }
            }
            return 0;
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        class ProgressionStat
        {
            public Stat stat;
            public float[] levels;
        }


    }
}