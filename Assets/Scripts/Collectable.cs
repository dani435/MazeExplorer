using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Collectable : MonoBehaviour
{
    public static event Action OnKeyCollected;
    public static event Action OnShieldCollected;

    void Update()
    {
        transform.localRotation = Quaternion.Euler(90f, Time.time * 100, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (this.CompareTag("Shield"))
            {
                OnShieldCollected?.Invoke();
            }
            else if (this.CompareTag("Key"))
            {
                OnKeyCollected?.Invoke();
            }
            Destroy(gameObject);
        }
    }
}
