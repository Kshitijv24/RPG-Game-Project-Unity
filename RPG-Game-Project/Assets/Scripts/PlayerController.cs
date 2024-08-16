using RPG.Attributes;
using RPG.Combat;
using RPG.Movement;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Camera mainCamera;
        MovementHandler playerMovement;
        Fighter fighter;
        Health health;

        enum CursoreType
        {
            None,
            Movement,
            Combat,
            UI,
            Pickup
        }

        [System.Serializable]
        struct CursorMapping
        {
            public CursoreType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappingArray;

        private void Awake()
        {
            playerMovement = GetComponent<MovementHandler>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (InteractWithUI()) return;
            if (health.IsDead())
            {
                SetCursor(CursoreType.None);
                return;
            }
            if (InteractWithCoponent()) return;
            if (MoveToMousePosition()) return;

            SetCursor(CursoreType.None);
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursoreType.UI);
                return true;
            }
            return false;
        }

        private void SetCursor(CursoreType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private bool InteractWithCoponent()
        {
            RaycastHit[] hitArray = Physics.RaycastAll(GetMouseRay());

            foreach (RaycastHit hit in hitArray)
            {
                IRayCastable[] rayCastablArray = hit.transform.GetComponents<IRayCastable>();

                foreach (IRayCastable rayCastable in rayCastablArray)
                {
                    if (rayCastable.HandleRayCast(this))
                    {
                        SetCursor(CursoreType.Pickup);
                        return true;
                    }
                }
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

                SetCursor(CursoreType.Movement);
                return true;
            }
            return false;
        }

        private CursorMapping GetCursorMapping(CursoreType type)
        {
            foreach (CursorMapping mapping in cursorMappingArray)
                if (mapping.type == type) return mapping;

            return cursorMappingArray[0];
        }

        private Ray GetMouseRay() => mainCamera.ScreenPointToRay(Input.mousePosition);
    }
}