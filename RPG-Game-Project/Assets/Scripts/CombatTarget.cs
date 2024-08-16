using RPG.Attributes;
using RPG.Control;
using UnityEngine;  

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRayCastable
    {
        public bool HandleRayCast(PlayerController callingController)
        {
            if (!callingController.GetComponent<Fighter>().CanAttack(gameObject))
                return false;

            if (Input.GetMouseButton(0))
                callingController.GetComponent<Fighter>().Attack(gameObject);

            return true;
        }
    }
}