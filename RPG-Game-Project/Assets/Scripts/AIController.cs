using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;

        Fighter fighter;
        Health health;
        PlayerMovement movement;
        ActionScheduler actionScheduler;

        GameObject player;
        Vector3 guardPosition;

        string playerTag = "Player";
        float timeSinceLastSawPlayer = Mathf.Infinity;
        int currentWayPointIndex = 0;

        private void Start()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            movement = GetComponent<PlayerMovement>();
            actionScheduler = GetComponent<ActionScheduler>();
            player = GameObject.FindWithTag(playerTag);
            guardPosition = transform.position;
        }

        private void Update()
        {
            if (health.IsDead()) return;

            if (InAttackRangeOfPlayer() && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                AttackBehaviour();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;

            if(patrolPath != null)
            {
                if (AtWayPoint())
                {
                    CycleWayPoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            movement.StartMoveAction(nextPosition);
        }

        private bool AtWayPoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void CycleWayPoint() => currentWayPointIndex = patrolPath.GetNextIndex(currentWayPointIndex);

        private Vector3 GetCurrentWaypoint() => patrolPath.GetWaypoint(currentWayPointIndex);

        private void SuspicionBehaviour() => actionScheduler.CancelCurrentAction();

        private void AttackBehaviour()
        {
            fighter.Attack(player);
        }

        private bool InAttackRangeOfPlayer()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}