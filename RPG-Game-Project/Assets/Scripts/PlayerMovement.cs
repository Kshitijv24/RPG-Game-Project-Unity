using RPG.Combat;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        NavMeshAgent navMeshAgent;
        Animator animator;
        Fighter fighter;

        string forwardSpeed = "ForwardSpeed";

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            fighter = GetComponent<Fighter>();
        }

        private void Update()
        {
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination)
        {
            fighter.CancelAttack();
            MoveToPosition(destination);
        }

        public void MoveToPosition(Vector3 destination)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
        }

        public void StopMovement()
        {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            animator.SetFloat(forwardSpeed, speed);
        }
    }
}