using RPG.Control;
using System;
using System.Collections;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRayCastable
    {
        [SerializeField] Weapon weapon;
        [SerializeField] float respawnTime = 5f;

        PlayerController player;
        Collider collider;

        private void Awake() => collider = GetComponent<Collider>();

        private void OnTriggerEnter(Collider other)
        {
            player = other.GetComponent<PlayerController>();

            if(player)
                HandlePickup(other.GetComponent<Fighter>());
        }

        private void HandlePickup(Fighter fighter)
        {
            fighter.EquipWeapon(weapon);
            StartCoroutine(HideForSeconds(respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickUp(false);
            yield return new WaitForSeconds(seconds);
            ShowPickUp(true);
        }

        private void ShowPickUp(bool shouldShow)
        {
            collider.enabled = shouldShow;

            foreach (Transform child in transform)
                child.gameObject.SetActive(shouldShow);
        }

        public bool HandleRayCast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
                HandlePickup(callingController.GetComponent<Fighter>());

            return true;
        }

        public CursorType GetCursorType() => CursorType.Pickup;
    }
}