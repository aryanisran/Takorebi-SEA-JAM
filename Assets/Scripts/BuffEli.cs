using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEli : MonoBehaviour
{
    PlayerController player;

    public Transform kickDirection;
    bool kicking;
    bool currentlySlow;
    public bool canKick;

    bool lethal;

    public float kickSpeed;

    float kickCooldown = 2;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!player.isGrounded)
        {
            canKick = true;

            if (Input.GetKeyDown(KeyCode.Z))
            {
                StartCoroutine(Kick());
            }
        }
        else
        {
            canKick = false;
        }

        if (this.transform.position.y > 2f && this.transform.position.y < 3.5f && kicking == true && canKick == false)
        {
            // Time stop if high enough
            Time.timeScale = 1f;
            //print("time is slowing");
            //------------------------------------
        }
        else if (this.transform.position.y < 2f)
        {
            Time.timeScale = 1f;
            //print("time stopped slowing");
        }
    }

    public IEnumerator Kick()
    {
        //player.moveSpeed = 0f;
        canKick = false;
        kicking = true;
        KickMotion();
        yield return new WaitForSeconds(kickCooldown);
        canKick = true;
        kicking = false;
        //player.moveSpeed = 3f;
    }

    public void KickMotion()
    {
        print("high jump kick!!!!");
        player.rb.velocity = new Vector3(kickSpeed, -kickSpeed);
        //player.rb.AddForce(this.transform.up * -kickSpeed);
    }
}
