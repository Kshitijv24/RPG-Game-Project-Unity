using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;
using UnityEngine.Rendering;
using GameDevTV.Utils;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthRegenPercentage = 70f;
        [SerializeField] LazyValue<float> healthPoints;

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

            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private void OnEnable() => baseStats.onLevelUP += RegenarateHealth;

        private void Start() => healthPoints.ForceInit();

        private void OnDisable() => baseStats.onLevelUP -= RegenarateHealth;

        private float GetInitialHealth() => baseStats.GetStat(Stat.Health);

        private void RegenarateHealth()
        {
            float regenHealthPoints = baseStats.GetStat(Stat.Health) * (healthRegenPercentage / 100);
            healthPoints.value = Mathf.Max(healthPoints.value, regenHealthPoints);
        }

        public bool IsDead() => isDead;

        public void TakeDamage(GameObject damageDealer, float damage)
        {
            print(gameObject.name + "Took damage: " + damage);

            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);

            if (healthPoints.value == 0)
            {
                Die();
                AwardExperience(damageDealer);
            }
        }

        public float GetHealthPoints() => healthPoints.value;

        public float GetMaxHealthPoints() => baseStats.GetStat(Stat.Health);

        public float GetHealthPercentage() => 100 * (healthPoints.value / baseStats.GetStat(Stat.Health));

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

        public object CaptureState() => healthPoints.value;

        public void RestoreState(object state)
        {
            healthPoints.value = (float)state;

            if (healthPoints.value == 0)
                Die();
        }
    }
}