using RPG.Control;
using RPG.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerLevelDisplay : MonoBehaviour
{
    BaseStats baseStats;
    TextMeshProUGUI levelText;

    private void Awake()
    {
        baseStats = FindObjectOfType<PlayerController>().GetComponent<BaseStats>();
        levelText = GetComponent<TextMeshProUGUI>();
    }

    private void Update() => levelText.text = String.Format("{0:0}", baseStats.CalculateLevel());
}
