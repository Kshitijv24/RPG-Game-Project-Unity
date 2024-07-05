using RPG.Attributes;
using RPG.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter fighter;
        TextMeshProUGUI healthText;

        private void Awake()
        {
            fighter = FindObjectOfType<PlayerController>().GetComponent<Fighter>();
            healthText = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (fighter.GetTarget() == null)
                healthText.text = "N/A";

            Health health = fighter.GetTarget();
            if (health == null) return;
            healthText.text = String.Format("{0:0}%", health.GetHealthPercentage());
        }
    }
}