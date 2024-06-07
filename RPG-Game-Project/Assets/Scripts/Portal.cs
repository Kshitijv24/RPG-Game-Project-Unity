using RPG.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier { A, B, C, D}

        [SerializeField] int sceneToLoad = -1;
        [SerializeField] Transform spawnPoint;
        [SerializeField] DestinationIdentifier destination;
        [SerializeField] float fadeOutTime = 1f;
        [SerializeField] float fadeInTime = 2f;
        [SerializeField] float fadeWaitTime = 0.5f;

        SavingWrapper savingWrapper;

        private void Start() => savingWrapper = FindObjectOfType<SavingWrapper>();

        private void OnTriggerEnter(Collider other)
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player != null)
                StartCoroutine(SceneTransition());
        }

        private IEnumerator SceneTransition()
        {
            if(sceneToLoad < 0)
            {
                Debug.LogError("Scene to load not set");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            CanvasFader canvasFader = FindObjectOfType<CanvasFader>();

            yield return canvasFader.FadeOut(fadeOutTime);

            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            savingWrapper.Load();

            Portal otherPortal = GetOtherPortal();
            SetPlayerSpawnPoint(otherPortal);

            savingWrapper.Save();

            yield return new WaitForSeconds(fadeWaitTime);
            yield return canvasFader.FadeIn(fadeInTime);

            Destroy(gameObject);
        }

        private void SetPlayerSpawnPoint(Portal otherPortal)
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.position = otherPortal.spawnPoint.position;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }

        private Portal GetOtherPortal()
        {
            foreach (Portal portal in FindObjectsOfType<Portal>())
            {
                if (portal == this) continue;
                if (portal.destination != destination) continue;

                return portal;
            }
            return null;
        }
    }
}