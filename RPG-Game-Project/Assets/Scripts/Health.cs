using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints = 20f;

        Animator animator;
        string die = "Die";
        bool isDead;
        ActionScheduler actionScheduler;
        float originalHealthPoints;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Start()
        {
            healthPoints = GetComponent<BaseStats>().GetHealth();
            originalHealthPoints = GetComponent<BaseStats>().GetHealth();
        }

        public bool IsDead() => isDead;

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            if (healthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        public float GetHealthPercentage() => 100 * (healthPoints / originalHealthPoints);

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            animator.SetTrigger(die);
            actionScheduler.CancelCurrentAction();
        }

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetExperiencePoints());
        }

        public object CaptureState() => healthPoints;

        public void RestoreState(object state)
        {
            healthPoints = (float)state;

            if (healthPoints == 0)
                Die();
        }
    }
}