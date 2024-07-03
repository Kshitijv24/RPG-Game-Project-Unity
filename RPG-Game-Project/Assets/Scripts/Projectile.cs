using RPG.Attributes;
using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1.0f;
        [SerializeField] bool isHoming = true;
        [SerializeField] Transform hitEffect;
        [SerializeField] float maxLifeTime = 10f;
        [SerializeField] GameObject[] destroyOnHitArray;
        [SerializeField] float lifeAfterImpact = 0.1f;

        Health target;
        float damage = 0;

        CapsuleCollider targetCapsuleCollider;

        private void Start()
        {
            targetCapsuleCollider = target.GetComponent<CapsuleCollider>();
            transform.LookAt(GetAimLocation());
        }

        private void Update()
        {
            if (target == null) return;

            if (isHoming && !target.IsDead())
                transform.LookAt(GetAimLocation());

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, float damage)
        {
            this.target = target;
            this.damage = damage;

            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation() => target.transform.position + Vector3.up * targetCapsuleCollider.height / 2;

        private void OnTriggerEnter(Collider other)
        {
            Health targetHealth = other.GetComponent<Health>();

            if (targetHealth != target) return;
            if (target.IsDead()) return;

            target.TakeDamage(damage);
            speed = 0;

            if (hitEffect != null)
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);

            foreach (GameObject objectToBeDestroyed in destroyOnHitArray)
                Destroy(objectToBeDestroyed);

            Destroy(gameObject, lifeAfterImpact);
        }
    }
}