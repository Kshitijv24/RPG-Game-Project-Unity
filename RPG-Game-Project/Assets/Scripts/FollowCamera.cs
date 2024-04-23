using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
	[SerializeField] Transform player;

    private void Update()
    {
        transform.position = player.position;
    }
}
