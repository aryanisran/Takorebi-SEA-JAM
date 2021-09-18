using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    //PlayerController player;
    Rigidbody rb;
    //[SerializeField] GameObject lamp;

    float dist;

    [SerializeField] Light spotlight;
    public float viewDistance;
    float viewAngle;
    public LayerMask viewMask;
    Transform player;

    PlayerController the_player;

    void Start()
    {
        the_player = FindObjectOfType<PlayerController>();
        viewAngle = spotlight.spotAngle;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanSeePlayer())
        {
            //spotlight.color = Color.red;
            StartCoroutine(LampDeath());
        }
        else
        {
            spotlight.color = Color.red;
        }
    }

    public IEnumerator LampDeath()
    {
        rb.useGravity = true;
        yield return new WaitForSeconds(0.5f);
        the_player.Die();
        print("game over");

    }

    bool CanSeePlayer()
    {
        Vector3 offset = player.position - transform.position;
        if (offset.sqrMagnitude <= viewDistance)
        {
            float angleBetweenLampAndPlayer = Vector3.Angle(spotlight.transform.forward, offset.normalized);
            if (angleBetweenLampAndPlayer < viewAngle / 2f)
            {
                if (!Physics.Linecast(transform.position, player.position, viewMask))
                {
                    return true;
                }
            }
        }
        return false;
    }


}
