using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	[SerializeField] Transform target = null;
	[SerializeField] float speed = 1.0f;

    CapsuleCollider targetCapsuleCollider;

    private void Start() => targetCapsuleCollider = target.GetComponent<CapsuleCollider>();

    private void Update()
    {
        if (target == null) return;

        transform.LookAt(GetAimLocation());
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private Vector3 GetAimLocation() => target.position + Vector3.up * targetCapsuleCollider.height / 2;
}
