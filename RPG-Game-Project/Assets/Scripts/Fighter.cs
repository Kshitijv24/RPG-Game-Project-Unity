using RPG.Attributes;
using RPG.Core;
using RPG.Movement;
using RPG.Saving;
using UnityEngine;
using RPG.Stats;
using System.Collections.Generic;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform rightHand = null;
        [SerializeField] Transform leftHand = null;
        [SerializeField] Weapon defaultWeapon = null;

        Health target = null;
        MovementHandler movement = null;
        ActionScheduler actionScheduler = null;
        Animator animator = null;
        Weapon currentWeapon = null;
        BaseStats baseStats = null;

        string attack = "Attack";
        string stopAttack = "StopAttack";
        float timeSinceLastAttack = Mathf.Infinity;

        private void Awake()
        {
            movement = GetComponent<MovementHandler>();
            actionScheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
            baseStats = GetComponent<BaseStats>();
        }

        private void Start()
        {
            if (currentWeapon != null) return;

            EquipWeapon(defaultWeapon);
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;

            if (target.IsDead()) return;

            if (!GetIsInRange())
                movement.MoveToPosition(target.transform.position, 1f);
            else
            {
                movement.CancelAttack();
                AttackBehaviour();
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            currentWeapon = weapon;
            weapon.Spawn(rightHand, leftHand, animator);
        }

        public Health GetTarget() => target;

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
            float damage = baseStats.GetStat(Stat.Damage);

            if (currentWeapon.HasProjectile())
                currentWeapon.LaunchProjectile(rightHand, leftHand, target, gameObject, damage);
            else
                target.TakeDamage(gameObject, damage);
        }

        private void Shoot() => Hit();

        private bool GetIsInRange()
        {
            if (currentWeapon == null)
                Debug.Log("currentWeapon is null");

            return Vector3.Distance(transform.position, target.transform.position) < currentWeapon.GetRange();
        }

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
            movement.CancelAttack();
        }

        private void StopAttack()
        {
            animator.ResetTrigger(attack);
            animator.SetTrigger(stopAttack);
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
                yield return currentWeapon.GetDamage();
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
                yield return currentWeapon.GetWeaponDamagePercentageBonas();
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