using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;

        Transform target;
        PlayerMovement playerMovement;
        ActionScheduler actionScheduler;

        private void Start()
        {
            playerMovement = GetComponent<PlayerMovement>();
            actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Update()
        {
            if (target == null) return;

            if (!GetIsInRange())
                playerMovement.MoveToPosition(target.position);
            else
                playerMovement.CancelAction();
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