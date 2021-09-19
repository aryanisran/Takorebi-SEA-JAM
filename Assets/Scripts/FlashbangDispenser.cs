using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashbangDispenser : MonoBehaviour
{
    public GameObject flashbangPrefab;
    public float spawnRate;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("DropFlash", 0f, spawnRate);
    }

    void DropFlash()
    {
        Instantiate(flashbangPrefab, transform.position, Quaternion.identity);
    }
}
