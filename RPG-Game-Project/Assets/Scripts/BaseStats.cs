using GameDevTV.Utils;
using System;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        public event Action onLevelUP;

        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression;
        [SerializeField] Transform levelUpParticleEffect;
        [SerializeField] bool shouldUseModifiers;

        Experience experience;
        LazyValue<int> currentLevel;

        private void Awake()
        {
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void OnEnable()
        {
            if (experience != null)
                experience.onExperienceGained += UpdateLevel;
        }

        private void Start() => currentLevel.ForceInit();

        private void OnDisable()
        {
            if (experience != null)
                experience.onExperienceGained -= UpdateLevel;
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();

            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                LevelUpEffect();
                onLevelUP?.Invoke();
            }
        }

        private void LevelUpEffect() => Instantiate(levelUpParticleEffect, transform);

        public float GetStat(Stat stat) =>
            (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat) / 100);

        private float GetBaseStat(Stat stat) => progression.GetStat(stat, characterClass, GetLevel());

        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;

            float total = 0f;

            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifiers(stat))
                    total += modifier;
            }
            return total;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;

            float total = 0f;

            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetPercentageModifiers(stat))
                    total += modifier;
            }
            return total;
        }

        public int GetLevel() => currentLevel.value;

        private int CalculateLevel()
        {
            if (experience == null) return startingLevel;

            float currentXP = experience.GetPoints();
            int penultimateLevel = progression.GetLevels(Stat.ExperienceToLevelUp, characterClass);

            for (int level = 1; level <= penultimateLevel; level++)
            {
                float XPToLevelUp = progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level);

                if (XPToLevelUp > currentXP)
                    return level;
            }
            return penultimateLevel + 1;
        }
    }
}