using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    bool passed;
    public int index;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !passed)
        {
            AudioManager.instance.Play("Collect");
            other.GetComponent<OldEli>().PassCheckpoint(index);
            passed = true;
        }
    }
}
