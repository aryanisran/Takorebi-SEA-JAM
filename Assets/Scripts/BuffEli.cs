using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffEli : MonoBehaviour
{
    PlayerController player;


    public Transform kickDirection;
    public bool kicking;
    public bool slamming;

    bool currentlySlow;
    public bool canKick;
    public bool canSlam = true;

    public bool lethal;

    public float kickSpeed;

    float kickCooldown = 2;

    [SerializeField] GameObject kickUI;
    [SerializeField] Slider slamUI;
    public float slamUITime = 0f;

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
            kickUI.SetActive(false);

            if (Input.GetKeyDown(KeyCode.Z))
            {
                StartCoroutine(Kick());
            }
        }
        else
        {
            kickUI.SetActive(true);
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

        slamUI.value = slamUITime;
        if (slamUITime >= 0)
        {
            slamUITime = slamUITime - Time.deltaTime;
        }
        else
        {
            slamUITime = 0f;
        }

        if (kicking == true)
        {
            player.anim.SetBool("Kick", true);
        } else { player.anim.SetBool("Kick", false); }

        if (slamming == true)
        {
            player.anim.SetBool("Slam", true);
        } else { player.anim.SetBool("Slam", false); }
    }

    public IEnumerator Kick()
    {
        AudioManager.instance.Play("Kick");
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
        AudioManager.instance.Play("Slam");
        print("slam");
        slamUITime = 2f;
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
