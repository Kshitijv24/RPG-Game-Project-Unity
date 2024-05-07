using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float healthPoints = 20f;

        Animator animator;
        string die = "Die";
        bool isDead;

        private void Start() => animator = GetComponent<Animator>();

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
        }
    }
}