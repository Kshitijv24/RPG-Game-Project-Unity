using RPG.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] float speed = 1.0f;

    Health target = null;

    CapsuleCollider targetCapsuleCollider;

    private void Start() => targetCapsuleCollider = target.GetComponent<CapsuleCollider>();

    private void Update()
    {
        if (target == null) return;

        transform.LookAt(GetAimLocation());
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void SetTarget(Health target) => this.target = target;

    private Vector3 GetAimLocation() => target.transform.position + Vector3.up * targetCapsuleCollider.height / 2;
}
