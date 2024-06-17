using RPG.Core;
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
        [SerializeField] bool isRightHanded = true;
        [SerializeField] Projectile projectile = null;

        Transform handTransform;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (animatorOverrideController == null || weaponPrefab == null) return;

            GetHandTransform(rightHand, leftHand);
            Instantiate(weaponPrefab, handTransform);
            animator.runtimeAnimatorController = animatorOverrideController;
        }

        private Transform GetHandTransform(Transform rightHand, Transform leftHand)
        {
            if (isRightHanded)
                handTransform = rightHand;
            else
                handTransform = leftHand;

            return handTransform;
        }

        public float GetDamage() => weaponDamage;

        public float GetRange() => weaponRange;

        public bool HasProjectile() => projectile != null;

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile projectileInstance =
                Instantiate(
                    projectile,
                    GetHandTransform(rightHand, leftHand).position,
                    Quaternion.identity);

            projectileInstance.SetTarget(target);
        }
    }
}