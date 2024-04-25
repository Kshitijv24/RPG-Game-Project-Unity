using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] Transform target;

    NavMeshAgent navMeshAgent;
    Animator animator;
    Camera mainCamera;

    string forwardSpeed = "ForwardSpeed";

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
            MoveToMousePosition();

        UpdateAnimator();
    }

    private void MoveToMousePosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool isHitSomething = Physics.Raycast(ray, out hit);

        if(isHitSomething)
            navMeshAgent.destination = hit.point;
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = navMeshAgent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        animator.SetFloat(forwardSpeed, speed);
    }
}
