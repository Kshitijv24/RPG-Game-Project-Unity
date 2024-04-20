using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] Transform target;

    NavMeshAgent agent;
    Camera mainCamera;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            MoveToMousePosition();
    }

    private void MoveToMousePosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool isHitSomething = Physics.Raycast(ray, out hit);

        if(isHitSomething)
            agent.destination = hit.point;
    }
}
