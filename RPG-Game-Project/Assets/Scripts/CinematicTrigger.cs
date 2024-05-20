using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        PlayableDirector playableDirector;
        bool triggeredOnce;

        private void Start() => playableDirector = GetComponent<PlayableDirector>();

        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponent<PlayerController>()) return;
            if (triggeredOnce) return;

            playableDirector.Play();
            triggeredOnce = true;
        }
    }
}