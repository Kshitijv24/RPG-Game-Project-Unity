using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapon/New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] AnimatorOverrideController animatorOverrideController = null;
        [SerializeField] GameObject weaponPrefab = null;
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponDamage = 5f;

        public void Spawn(Transform handTransform, Animator animator)
        {
            if (animatorOverrideController == null || weaponPrefab == null) return;

            Instantiate(weaponPrefab, handTransform);
            animator.runtimeAnimatorController = animatorOverrideController;
        }

        public float GetDamage() => weaponDamage;

        public float GetRange() => weaponRange;
    }
}