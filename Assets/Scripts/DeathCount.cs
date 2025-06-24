using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCount : MonoBehaviour
{
    TMPro.TMP_Text text;
    int death;

    private void Start()
    {
        UpdateCount();
    }
    private void Awake()
    {
        text = GetComponent<TMPro.TMP_Text>();
    }

    private void OnEnable()
    {
        SpikeTrap.OnDeath += OnDeath;
    }

    private void OnDisable()
    {
        SpikeTrap.OnDeath -= OnDeath;
    }

    private void OnDeath()
    {
        MazeGenerator.death++;
        UpdateCount();
    }

    private void UpdateCount()
    {
        text.text = $"Death Counter: {MazeGenerator.death}";
    }
}
