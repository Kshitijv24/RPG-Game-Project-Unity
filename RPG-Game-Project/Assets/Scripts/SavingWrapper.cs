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

        SavingSystem savingSystem;

        private void Awake() => savingSystem = GetComponent<SavingSystem>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
                Load();
            if (Input.GetKeyDown(KeyCode.S))
                Save();
        }

        private void Load() => savingSystem.Load(defaultSaveFile);

        private void Save() => savingSystem.Save(defaultSaveFile);
    }
}