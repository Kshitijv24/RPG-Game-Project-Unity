using RPG.Control;
using TMPro;
using UnityEngine;
using System;

namespace RPG.Attributes
{
    public class PlayerHealthDisplay : MonoBehaviour
    {
        Health health;
        TextMeshProUGUI healthText;

        private void Awake()
        {
            health = FindObjectOfType<PlayerController>().GetComponent<Health>();
            healthText = GetComponent<TextMeshProUGUI>();
        }

        private void Update() =>
            healthText.text =
            String.Format("{0:0}/{1:0}",
                health.GetHealthPoints(),
                health.GetMaxHealthPoints());
    }
}