using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashbangPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<OldEli>().PickupFlash();
            Destroy(gameObject);
        }
    }
}
