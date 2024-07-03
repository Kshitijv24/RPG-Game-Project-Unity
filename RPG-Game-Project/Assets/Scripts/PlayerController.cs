using RPG.Attributes;
using RPG.Combat;
using RPG.Movement;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Camera mainCamera;
        MovementHandler playerMovement;
        Fighter fighter;
        Health health;

        private void Awake()
        {
            playerMovement = GetComponent<MovementHandler>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (health.IsDead()) return;
            if (InteractWithCombat()) return;
            if (MoveToMousePosition()) return;

            //print("Can not move there");
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hitArray = Physics.RaycastAll(GetMouseRay());
            
            foreach (RaycastHit hit in hitArray)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();

                if(target == null) continue;

                if(!fighter.CanAttack(target.gameObject)) continue;

                if (Input.GetMouseButton(0))
                    fighter.Attack(target.gameObject);

                return true;
            }
            return false;
        }

        private bool MoveToMousePosition()
        {
            RaycastHit hit;
            bool isHitSomething = Physics.Raycast(GetMouseRay(), out hit);

            if (isHitSomething)
            {
                if (Input.GetMouseButton(0))
                    playerMovement.StartMoveAction(hit.point, 1f);

                return true;
            }
            return false;
        }

        private Ray GetMouseRay() => mainCamera.ScreenPointToRay(Input.mousePosition);
    }
}