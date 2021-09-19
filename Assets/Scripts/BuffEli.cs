using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffEli : MonoBehaviour
{
    PlayerController player;

    public Transform kickDirection;
    public bool kicking;
    public bool slamming;

    bool currentlySlow;
    public bool canKick;
    public bool canSlam = true;

    bool lethal;

    public float kickSpeed;

    float kickCooldown = 2;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerController>();
        player.moveSpeed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (canSlam && !kicking && player.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                StartCoroutine(Slam());
            }
        }

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
            kicking = false;
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



        if (kicking == true || slamming == true)
        {
            lethal = true;
        }
        else
        {
            lethal = false;
        }
    }

    public IEnumerator Kick()
    {
        kicking = true;
        KickMotion();
        yield return new WaitForSeconds(kickCooldown);
    }

    public void KickMotion()
    {
        print("high jump kick!!!!");
        player.rb.velocity = new Vector3(player.directionFacing * kickSpeed * (9 / 4), -kickSpeed, player.rb.velocity.z);
        //player.rb.AddForce(this.transform.up * -kickSpeed);
    }

    public IEnumerator Slam()
    {
        print("slam");
        canSlam = false;
        player.moveSpeed = 0f;
        player.jumpForce = 0f;
        //play the animation here
        slamming = true;
        yield return new WaitForSeconds(2f);
        canSlam = true;
        slamming = false;
        player.jumpForce = 5f;
        player.moveSpeed = 3f;
    }    
}
