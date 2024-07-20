using RPG.Attributes;
using RPG.Combat;
using RPG.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience experience;
        TextMeshProUGUI experienceText;

        private void Awake()
        {
            experience = FindObjectOfType<PlayerController>().GetComponent<Experience>();
            experienceText = GetComponent<TextMeshProUGUI>();
        }

        private void Update() => experienceText.text = String.Format("{0:0}", experience.GetPoints());
    }
}
