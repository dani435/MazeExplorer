using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    TMPro.TMP_Text text;
    public static float elapsedTime;
    public static int minutes;
    public static bool canTime = false;

    private void Awake()
    {
        text = GetComponent<TMPro.TMP_Text>();
    }
    private void Update()
    {
        if (canTime) 
        {
            elapsedTime += Time.deltaTime;
            minutes = Mathf.FloorToInt(elapsedTime / 60f);

            if (minutes >= 100)
            {
                minutes = 0;
                MazeGenerator.death++;
            }
        }
        text.text = $"Time: {DisplayTime()}";
    }

    private string DisplayTime()
    {
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
        return timeString;
    }
}
