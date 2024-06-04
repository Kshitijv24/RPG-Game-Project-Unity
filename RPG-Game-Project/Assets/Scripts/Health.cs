using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints = 20f;

        Animator animator;
        string die = "Die";
        bool isDead;
        ActionScheduler actionScheduler;

        private void Start()
        {
            animator = GetComponent<Animator>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        public bool IsDead() => isDead;

        public void TakeDamage(float damage)
        {
            healthPoints = Mathf.Max(healthPoints - damage, 0);

            if (healthPoints == 0)
                Die();
        }

        private void Die()
        {
            if (isDead) return;

            isDead = true;
            animator.SetTrigger(die);
            actionScheduler.CancelCurrentAction();
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