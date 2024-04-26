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
            InteractWithCombat();
            InteractWithMovement();
        }

        private void InteractWithCombat()
        {
            RaycastHit[] hitArray = Physics.RaycastAll(GetMouseRay());
            
            foreach (RaycastHit hit in hitArray)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();

                if(target == null) continue;

                if(Input.GetMouseButtonDown(0))
                    fighter.Attack(target);
            }
        }

        private void InteractWithMovement()
        {
            if (Input.GetMouseButton(0))
                MoveToMousePosition();
        }

        private void MoveToMousePosition()
        {
            RaycastHit hit;
            bool isHitSomething = Physics.Raycast(GetMouseRay(), out hit);

            if (isHitSomething)
                playerMovement.MoveToPosition(hit.point);
        }

        private Ray GetMouseRay()
        {
            return mainCamera.ScreenPointToRay(Input.mousePosition);
        }
    }
}