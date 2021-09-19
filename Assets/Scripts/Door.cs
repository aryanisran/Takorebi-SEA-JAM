using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public PlayerController player;
    //Animator anim;

    public GameObject realDoor;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        //anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && player.keysHolding >= 1)
        {
            player.keysHolding--;
            realDoor.GetComponent<Animator>().SetTrigger("Open");
        }
    }

}
