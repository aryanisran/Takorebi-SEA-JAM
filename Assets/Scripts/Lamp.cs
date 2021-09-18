using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    //PlayerController player;
    Rigidbody rb;
    [SerializeField] GameObject lamp;

    //float fallSpeed = 10f;
    //bool inside;
    // Start is called before the first frame update
    void Start()
    {
        //player = FindObjectOfType<PlayerController>();
        rb = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //player.Die();
            rb.useGravity = true;
        }
    }


}
