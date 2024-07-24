using RPG.Saving;
using System;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        public event Action onExperienceGained;

        [SerializeField] float experiencePoints = 0f;

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            onExperienceGained();
        }

        public object CaptureState() => experiencePoints;

        public void RestoreState(object state) => experiencePoints = (float)state;

        public float GetPoints() => experiencePoints;
    }
}