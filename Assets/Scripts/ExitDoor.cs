using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        MazeGenerator.isInDoor = true;
    }
    private void OnTriggerExit(Collider other)
    {
        MazeGenerator.isInDoor = false;
    }
}
