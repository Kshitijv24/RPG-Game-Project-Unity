using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        PlayableDirector playableDirector;

        private void Awake() => playableDirector = GetComponent<PlayableDirector>();

        private void Start()
        {
            playableDirector.played += DisableControl;
            playableDirector.stopped += EnableControl;
        }

        private void DisableControl(PlayableDirector playableDirector) => print("DisableControl");

        private void EnableControl(PlayableDirector playableDirector) => print("EnableControl");
    }
}