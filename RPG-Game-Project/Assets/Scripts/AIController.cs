using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;

        string playerTag = "Player";

        private void Update()
        {
            if (DistanceToPlayer() < chaseDistance)
            {
                print(gameObject.name + " Chase Player");
            }
        }

        private float DistanceToPlayer()
        {
            GameObject player = GameObject.FindWithTag(playerTag);
            return Vector3.Distance(player.transform.position, transform.position);
        }
    }
}