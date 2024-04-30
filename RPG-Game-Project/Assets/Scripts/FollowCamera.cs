using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform player;

        private void LateUpdate() => transform.position = player.position;
    }
}