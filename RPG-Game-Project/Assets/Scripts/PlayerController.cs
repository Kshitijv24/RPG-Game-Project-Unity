using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Camera mainCamera;
    PlayerMovement playerMovement;

    private void Start()
    {
        mainCamera = Camera.main;
        playerMovement = new PlayerMovement();
    }

    private void Update()
    {
        if(Input.GetMouseButton(0))
            MoveToMousePosition();
    }

    private void MoveToMousePosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool isHitSomething = Physics.Raycast(ray, out hit);

        if (isHitSomething)
            playerMovement.MoveToPosition(hit.point);
    }
}
