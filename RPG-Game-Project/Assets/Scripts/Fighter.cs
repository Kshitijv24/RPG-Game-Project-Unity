using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] float weaponRange = 2f;

        Transform target;
        PlayerMovement playerMovement;

        private void Start()
        {
            playerMovement = GetComponent<PlayerMovement>();
        }

        private void Update()
        {
            if (target == null) return;

            if (!GetIsInRange())
                playerMovement.MoveToPosition(target.position);
            else
                playerMovement.StopMovement();
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.transform;
        }

        public void CancelAttack()
        {
            target = null;
        }
    }
}