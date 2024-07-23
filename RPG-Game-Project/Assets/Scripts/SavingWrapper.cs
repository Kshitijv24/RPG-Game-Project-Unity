using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";

        [SerializeField] float fadeInTime = 0.2f;

        SavingSystem savingSystem;

        private void Awake() => savingSystem = GetComponent<SavingSystem>();

        IEnumerator Start()
        {
            CanvasFader canvasFader = FindObjectOfType<CanvasFader>();
            canvasFader.FadeOutImmediate();

            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            yield return canvasFader.FadeIn(fadeInTime);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
                Load();
            if (Input.GetKeyDown(KeyCode.S))
                Save();
            if (Input.GetKeyDown(KeyCode.Delete))
                Delete();
        }

        public void Load() => savingSystem.Load(defaultSaveFile);

        public void Save() => savingSystem.Save(defaultSaveFile);

        public void Delete() => savingSystem.Delete(defaultSaveFile);
    }
}