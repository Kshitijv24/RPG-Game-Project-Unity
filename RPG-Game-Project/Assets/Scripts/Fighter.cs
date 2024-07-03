using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform rightHand;
        [SerializeField] Transform leftHand;
        [SerializeField] Weapon defaultWeapon;

        Health target;
        MovementHandler playerMovement;
        ActionScheduler actionScheduler;
        Animator animator;
        Weapon currentWeapon;

        string attack = "Attack";
        string stopAttack = "StopAttack";
        float timeSinceLastAttack = Mathf.Infinity;

        private void Awake()
        {
            playerMovement = GetComponent<MovementHandler>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            if (defaultWeapon != null) return;

            EquipWeapon(defaultWeapon);
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

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            weapon.Spawn(rightHand, leftHand, animator);
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

            if (currentWeapon.HasProjectile())
                currentWeapon.LaunchProjectile(rightHand, leftHand, target);
            else
                target.TakeDamage(currentWeapon.GetDamage());
        }

        private void Shoot() => Hit();

        private bool GetIsInRange() => 
            Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetRange();

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

        public object CaptureState() => currentWeapon.name;

        public void RestoreState(object state)
        {
            string weaponName = (string)state;
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }
    }
}