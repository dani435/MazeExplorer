using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CollectableCount : MonoBehaviour
{
    TMPro.TMP_Text text;
    int keyCount;
    float elapsedTime = 10f;

    private void Start()
    {
        UpdateKeyCount();
    }
    private void Awake()
    {
        text = GetComponent<TMPro.TMP_Text>();
    }

    private void Update()
    {
        if (MazeGenerator.canDie == false)
        {           
            if (elapsedTime <= 1)
            {
                MazeGenerator.canDie = true;
                elapsedTime = 10f;
            }
            else
            {
                elapsedTime -= Time.deltaTime;
                if (text.name == "Immortality")
                {
                    text.text = $"Immortality: {DisplayTime()}";
                }
            }
        }
        else
        {
            if (text.name == "Immortality")
            {
                text.text = $" ";
            }
        }
    }

    private string DisplayTime()
    {
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        string timeString = string.Format("{00}", seconds);
        return timeString;
    }

    private void OnEnable()
    {
        Collectable.OnShieldCollected += OnCollectableShieldCollected;
        Collectable.OnKeyCollected += OnCollectableKeyCollected;
    }

    private void OnDisable()
    {
        Collectable.OnShieldCollected -= OnCollectableShieldCollected;
        Collectable.OnKeyCollected -= OnCollectableKeyCollected;
    }

    private void OnCollectableKeyCollected()
    {
        keyCount++;
        UpdateKeyCount();
        if (keyCount == MazeGenerator.keyNum)
        {
            MazeGenerator.allKeys = true;
            Timer.canTime = false;
        }
    }

    private void OnCollectableShieldCollected()
    {
        elapsedTime = 10f;
        MazeGenerator.canDie = false;
    }

    private void UpdateKeyCount()
    {
        if (text.name == "KeyCounter")
        {
            text.text = $"Key: {keyCount} / {MazeGenerator.keyNum}";
        }
    }
}
