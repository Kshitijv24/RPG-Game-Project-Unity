using RPG.Attributes;
using RPG.Combat;
using RPG.Movement;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Camera mainCamera;
        MovementHandler playerMovement;
        Fighter fighter;
        Health health;

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappingArray;
        [SerializeField] float maxNavMeshProjectionDistance = 1f;
        [SerializeField] float maxNavMeshPathLength = 40f;

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
                SetCursor(CursorType.None);
                return;
            }
            if (InteractWithCoponent()) return;
            if (MoveToMousePosition()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private bool InteractWithCoponent()
        {
            RaycastHit[] hitArray = RaycastAllSorted();

            foreach (RaycastHit hit in hitArray)
            {
                IRayCastable[] rayCastablArray = hit.transform.GetComponents<IRayCastable>();

                foreach (IRayCastable rayCastable in rayCastablArray)
                {
                    if (rayCastable.HandleRayCast(this))
                    {
                        SetCursor(rayCastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
        }

        private RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hitArray = Physics.RaycastAll(GetMouseRay());
            float[] distances = new float[hitArray.Length];

            for (int i = 0; i < hitArray.Length; i++)
                distances[i] = hitArray[i].distance;

            Array.Sort(distances, hitArray);
            return hitArray;
        }

        private bool MoveToMousePosition()
        {
            Vector3 target;
            bool isHitSomething = RaycastNavMesh(out target);

            if (isHitSomething)
            {
                if (Input.GetMouseButton(0))
                    playerMovement.StartMoveAction(target, 1f);

                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = Vector3.zero;
            RaycastHit hit;
            bool isHitSomething = Physics.Raycast(GetMouseRay(), out hit);
            if (!isHitSomething) return false;

            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = 
                NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;

            target = navMeshHit.position;

            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > maxNavMeshPathLength) return false;

            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float total = 0f;
            if (path.corners.Length < 2) return total;
            for (int i = 0; i < path.corners.Length - 1; i++)
                total += Vector3.Distance(path.corners[i], path.corners[i + 1]);

            return total;
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping mapping in cursorMappingArray)
                if (mapping.type == type) return mapping;

            return cursorMappingArray[0];
        }

        private Ray GetMouseRay() => mainCamera.ScreenPointToRay(Input.mousePosition);
    }
}