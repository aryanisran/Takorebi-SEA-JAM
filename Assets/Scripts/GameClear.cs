using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClear : MonoBehaviour
{
    public GameObject endScreen;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<BuffEli>().isActiveAndEnabled)
            {
                endScreen.SetActive(true);
            }
        }
    }
}
