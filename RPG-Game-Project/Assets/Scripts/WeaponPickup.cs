using RPG.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon;
        [SerializeField] float respawnTime = 5f;

        PlayerController player;
        Collider collider;

        private void Awake() => collider = GetComponent<Collider>();

        private void OnTriggerEnter(Collider other)
        {
            player = other.GetComponent<PlayerController>();

            if(player != null)
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                StartCoroutine(HideForSeconds(respawnTime));
            }
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
    }
}