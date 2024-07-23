using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression;

        Experience experience;

        private void Start() => experience = GetComponent<Experience>();

        public float GetStat(Stat stat) => progression.GetStat(stat, characterClass, GetLevel());

        public int GetLevel()
        {
            if (experience == null) return startingLevel;

            float currentXP = experience.GetPoints();
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);

            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);
                
                if(XPToLevelUp > currentXP)
                    return level;
            }
            return penultimateLevel + 1;
        }
    }
}