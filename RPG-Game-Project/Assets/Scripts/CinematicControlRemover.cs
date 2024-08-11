using RPG.Control;
using RPG.Core;
using RPG.Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        PlayableDirector playableDirector;
        PlayerController playerController;
        ActionScheduler actionScheduler;

        private void Awake()
        {
            playableDirector = GetComponent<PlayableDirector>();
            playerController = FindObjectOfType<PlayerController>();
            actionScheduler = playerController.GetComponent<ActionScheduler>();
        }

        private void OnEnable()
        {
            playableDirector.played += DisableControl;
            playableDirector.stopped += EnableControl;
        }

        private void OnDisable()
        {
            playableDirector.played -= DisableControl;
            playableDirector.stopped -= EnableControl;
        }

        private void DisableControl(PlayableDirector playableDirector)
        {
            print("DisableControl");
            actionScheduler.CancelCurrentAction();
            playerController.enabled = false;
        }

        private void EnableControl(PlayableDirector playableDirector)
        {
            print("EnableControl");
            playerController.enabled = true;
        }
    }
}