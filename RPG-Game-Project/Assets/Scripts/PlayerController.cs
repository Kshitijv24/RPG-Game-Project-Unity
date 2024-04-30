using RPG.Combat;
using RPG.Movement;
using System;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Camera mainCamera;
        PlayerMovement playerMovement;
        Fighter fighter;

        private void Start()
        {
            mainCamera = Camera.main;
            playerMovement = GetComponent<PlayerMovement>();
            fighter = GetComponent<Fighter>();
        }

        private void Update()
        {
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

                if (Input.GetMouseButtonDown(0))
                    fighter.Attack(target);

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
                    playerMovement.StartMoveAction(hit.point);

                return true;
            }
            return false;
        }

        private Ray GetMouseRay() => mainCamera.ScreenPointToRay(Input.mousePosition);
    }
}