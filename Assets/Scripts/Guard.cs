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
    Color originalColor;
    public float viewDistance, timeToSpotPlayer, flashDuration;
    float viewAngle;
    float playerVisibleTimer;
    public LayerMask viewMask;
    Transform player;
    public bool flashed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        waypointIndex = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        viewAngle = spotlight.spotAngle;
        originalColor = spotlight.color;
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.SqrMagnitude(transform.position - waypoints[waypointIndex].position);
        if(dist <= 0.01f)
        {
            IncreaseIndex();
        }
        if (!flashed)
        {
            if (CanSeePlayer())
            {
                playerVisibleTimer += Time.deltaTime;
                rb.velocity = Vector3.zero;
            }
            else
            {
                playerVisibleTimer -= Time.deltaTime;
                Patrol();
            }
            playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);
            spotlight.color = Color.Lerp(originalColor, Color.red, playerVisibleTimer);

            if(playerVisibleTimer >= timeToSpotPlayer)
            {
                //Do player spotted stuff, game over
            }
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
        if (flashed)
        {
            return false;
        }
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

    public void CheckFlashed(Vector3 flashPos)
    {
        Vector3 offset = flashPos - transform.position;
        if (offset.sqrMagnitude <= viewDistance)
        {
            float angleBetweenGuardAndPlayer = Vector3.Angle(spotlight.transform.forward, offset.normalized);
            if (angleBetweenGuardAndPlayer < viewAngle / 2f)
            {
                if (!Physics.Linecast(transform.position, flashPos, viewMask))
                {
                    flashed = true;
                    StartCoroutine("FlashedCo");
                }
            }
        }
    }

    IEnumerator FlashedCo()
    {
        yield return new WaitForSeconds(flashDuration);
        flashed = false;
    }
}
