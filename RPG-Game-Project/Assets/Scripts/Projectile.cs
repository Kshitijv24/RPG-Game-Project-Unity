using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] float speed = 1.0f;

    Health target = null;
    float damage = 0;

    CapsuleCollider targetCapsuleCollider;

    private void Start() => targetCapsuleCollider = target.GetComponent<CapsuleCollider>();

    private void Update()
    {
        if (target == null) return;

        transform.LookAt(GetAimLocation());
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void SetTarget(Health target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }

    private Vector3 GetAimLocation() => target.transform.position + Vector3.up * targetCapsuleCollider.height / 2;

    private void OnTriggerEnter(Collider other)
    {
        Health targetHealth = other.GetComponent<Health>();

        if (targetHealth != target) return;

        target.TakeDamage(damage);
        Destroy(gameObject);
    }
}