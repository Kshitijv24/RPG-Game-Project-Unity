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

        Transform handTransform;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (animatorOverrideController == null || weaponPrefab == null) return;
            
            if (isRightHanded)
                handTransform = rightHand;
            else
                handTransform = leftHand;

            Instantiate(weaponPrefab, handTransform);
            animator.runtimeAnimatorController = animatorOverrideController;
        }

        public float GetDamage() => weaponDamage;

        public float GetRange() => weaponRange;
    }
}