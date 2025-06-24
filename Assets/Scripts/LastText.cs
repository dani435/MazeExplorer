using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastText : MonoBehaviour
{
    public bool lastText;
    TMPro.TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMPro.TMP_Text>();
    }

    private void Update()
    {
        if (MazeGenerator.allKeys == false && MazeGenerator.isInDoor)
        {
            if (text.name == "LastText")
            {
                text.text = "You need to take all the keys";
            }
        }
        else
        {
            if (text.name == "LastText")
            {
                text.text = " ";
            }
        }
    }
}
