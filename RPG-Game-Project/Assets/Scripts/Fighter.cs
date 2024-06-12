using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform handTransform = null;
        [SerializeField] Weapon weapon = null;

        Health target;
        MovementHandler playerMovement;
        ActionScheduler actionScheduler;
        Animator animator;
        string attack = "Attack";
        string stopAttack = "StopAttack";
        float timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            playerMovement = GetComponent<MovementHandler>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            SpawnWeapon();
        }

        private void SpawnWeapon()
        {
            if (weapon == null) return;
            weapon.Spawn(handTransform, animator);
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;

            if (target.IsDead()) return;

            if (!GetIsInRange())
                playerMovement.MoveToPosition(target.transform.position, 1f);
            else
            {
                playerMovement.CancelAttack();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if(timeSinceLastAttack > timeBetweenAttacks)
            {
                // this will trigger the Hit() Function.
                TriggerAttack();
                timeSinceLastAttack = 0f;
            }
        }

        private void TriggerAttack()
        {
            animator.ResetTrigger(stopAttack);
            animator.SetTrigger(attack);
        }

        // Default Unity's Animation Event, it calls automatically by unity.
        private void Hit()
        {
            if (target == null) return;

            target.TakeDamage(weapon.GetDamage());
        }

        private bool GetIsInRange() => 
            Vector3.Distance(transform.position, target.transform.position) < weapon.GetRange();

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;

            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Attack(GameObject combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void CancelAttack()
        {
            StopAttack();
            target = null;
            playerMovement.CancelAttack();
        }

        private void StopAttack()
        {
            animator.ResetTrigger(attack);
            animator.SetTrigger(stopAttack);
        }
    }
}