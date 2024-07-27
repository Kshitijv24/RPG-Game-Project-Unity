using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] GameObject targetToDestroy;

        ParticleSystem particleSystem;

        private void Awake() => particleSystem = GetComponent<ParticleSystem>();

        private void Update()
        {
            if (!particleSystem.IsAlive())
            {
                if(targetToDestroy != null)
                    Destroy(targetToDestroy);
                else
                    Destroy(gameObject);
            }
        }
    }
}