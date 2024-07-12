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
        [SerializeField] Transform hitEffect;
        [SerializeField] bool isHoming = true;
        [SerializeField] float maxLifeTime = 10f;
        [SerializeField] float lifeAfterImpact = 0.1f;
        [SerializeField] GameObject[] destroyOnHitArray;

        GameObject damageDealer = null;
        Health target = null;
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

        public void SetTarget(Health target, GameObject damageDealer, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.damageDealer = damageDealer;

            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation() => target.transform.position + Vector3.up * targetCapsuleCollider.height / 2;

        private void OnTriggerEnter(Collider other)
        {
            Health targetHealth = other.GetComponent<Health>();

            if (targetHealth != target) return;
            if (target.IsDead()) return;

            target.TakeDamage(damageDealer, damage);
            speed = 0;

            if (hitEffect != null)
                Instantiate(hitEffect, GetAimLocation(), transform.rotation);

            foreach (GameObject objectToBeDestroyed in destroyOnHitArray)
                Destroy(objectToBeDestroyed);

            Destroy(gameObject, lifeAfterImpact);
        }
    }
}