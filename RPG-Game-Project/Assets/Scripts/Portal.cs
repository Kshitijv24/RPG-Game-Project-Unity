using RPG.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;

        private void OnTriggerEnter(Collider other)
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
                StartCoroutine(SceneTransition());
        }

        private IEnumerator SceneTransition()
        {
            DontDestroyOnLoad(gameObject);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            Portal otherPortal = GetOtherPortal();
            SetPlayerSpawnPoint(otherPortal);

            print("Scene Loaded");
            Destroy(gameObject);
        }

        private void SetPlayerSpawnPoint(Portal otherPortal)
        {
            PlayerController player = FindObjectOfType<PlayerController>();

            player.transform.position = otherPortal.spawnPoint.position;
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) 
                    continue;

                return portal;
            }
            return null;
        }
    }
}