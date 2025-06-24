using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public static event Action OnDeath;

    float timer = 0;
    float spikeTime;
    bool spikeUp = false;
    bool currentStatus = false;

    private void Awake()
    {
        RandomTimer();
    }
    private void Update()
    {
        if (timer <= spikeTime)
        {
            timer += Time.deltaTime;
        }
        else 
        {
            spikeUp = !spikeUp;
            timer = 0;
        }
        if (spikeUp != currentStatus)
        {
            currentStatus = spikeUp;
            if (currentStatus)
            {
                this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + 0.85f, this.transform.localPosition.z), 1f);
            }
            else
            {
                this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, new Vector3(this.transform.localPosition.x, this.transform.localPosition.y - 0.85f, this.transform.localPosition.z), 1f);
            }
            RandomTimer();
        }
    }

    private void RandomTimer()
    {
        spikeTime = UnityEngine.Random.Range(3f, 5f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && MazeGenerator.canDie)
        {
            OnDeath?.Invoke();
        }
    }
}
