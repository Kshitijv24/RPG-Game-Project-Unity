using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;
using UnityEngine.Rendering;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthRegenPercentage = 70f;
        [SerializeField] float healthPoints = -1.0f;

        Animator animator;
        string die = "Die";
        bool isDead;
        ActionScheduler actionScheduler;
        BaseStats baseStats;

        private void Awake()
        {
            baseStats = GetComponent<BaseStats>();
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
            
            baseStats.onLevelUP += RegenarateHealth;

            if(healthPoints < 0)
                healthPoints = baseStats.GetStat(Stat.Health);
        }

        private void RegenarateHealth()
        {
            float regenHealthPoints = baseStats.GetStat(Stat.Health) * (healthRegenPercentage / 100);
            healthPoints = Mathf.Max(healthPoints, regenHealthPoints);
        }

        public bool IsDead() => isDead;

        public void TakeDamage(GameObject damageDealer, float damage)
        {
            print(gameObject.name + "Took damage: " + damage);

            healthPoints = Mathf.Max(healthPoints - damage, 0);

            if (healthPoints == 0)
            {
                Die();
                AwardExperience(damageDealer);
            }
        }

        public float GetHealthPoints() => healthPoints;

        public float GetMaxHealthPoints() => baseStats.GetStat(Stat.Health);

        public float GetHealthPercentage() => 100 * (healthPoints / baseStats.GetStat(Stat.Health));

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            animator.SetTrigger(die);
            actionScheduler.CancelCurrentAction();
        }

        private void AwardExperience(GameObject damageDealer)
        {
            Experience experience = damageDealer.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperiencePoints));
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