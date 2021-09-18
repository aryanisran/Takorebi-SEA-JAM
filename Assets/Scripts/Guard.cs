using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] Transform[] waypoints;
    [SerializeField] float moveSpeed;

    int waypointIndex;
    float dist;

    [SerializeField] Light spotlight;
    public float viewDistance;
    float viewAngle;
    public LayerMask viewMask;
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        waypointIndex = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        viewAngle = spotlight.spotAngle;
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.SqrMagnitude(transform.position - waypoints[waypointIndex].position);
        if(dist <= 0.01f)
        {
            IncreaseIndex();
        }
        Debug.Log(CanSeePlayer());
        if (CanSeePlayer())
        {
            spotlight.color = Color.red;
            rb.velocity = Vector3.zero;
        }
        else
        {
            spotlight.color = Color.white;
            Patrol();
        }
    }

    void Patrol()
    {
        Vector3 direction = waypoints[waypointIndex].position - transform.position;
        direction.Normalize();
        rb.velocity = direction * moveSpeed;
        Vector3 targetPos = new Vector3(waypoints[waypointIndex].position.x, spotlight.transform.position.y, waypoints[waypointIndex].position.z);
        spotlight.transform.LookAt(targetPos);
    }

    void IncreaseIndex()
    {
        waypointIndex++;
        if(waypointIndex >= waypoints.Length)
        {
            waypointIndex = 0;
        }
    }

    bool CanSeePlayer()
    {
        Vector3 offset = player.position - transform.position;
        if(offset.sqrMagnitude <= viewDistance)
        {
            float angleBetweenGuardAndPlayer = Vector3.Angle(spotlight.transform.forward, offset.normalized);
            if(angleBetweenGuardAndPlayer < viewAngle / 2f)
            {
                if(!Physics.Linecast(transform.position, player.position, viewMask))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
