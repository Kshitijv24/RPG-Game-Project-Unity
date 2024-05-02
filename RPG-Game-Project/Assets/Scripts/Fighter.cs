using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponDamage = 5f;

        Transform target;
        PlayerMovement playerMovement;
        ActionScheduler actionScheduler;
        Animator animator;
        string attack = "Attack";
        float timeSinceLastAttack = 0f;

        private void Start()
        {
            playerMovement = GetComponent<PlayerMovement>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;

            if (!GetIsInRange())
                playerMovement.MoveToPosition(target.position);
            else
            {
                playerMovement.CancelAction();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            if(timeSinceLastAttack > timeBetweenAttacks)
            {
                // this will trigger the Hit() Function.
                animator.SetTrigger(attack);
                timeSinceLastAttack = 0f;
            }
        }

        // Default Unity's Animation Event
        private void Hit()
        {
            Health health = target.GetComponent<Health>();
            health.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange() => Vector3.Distance(transform.position, target.position) < weaponRange;

        public void Attack(CombatTarget combatTarget)
        {
            actionScheduler.StartAction(this);
            target = combatTarget.transform;
        }

        public void CancelAction() => target = null;
    }
}