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
    public bool flashed, scared;
    Animator anim;



    public int directionFacing;
    int prevDir;
    bool dying;

    public GameObject stunParticle;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        waypointIndex = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        viewAngle = spotlight.spotAngle;
        originalColor = spotlight.color;
        directionFacing = 1;
        prevDir = 1;
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
            if (scared)
            {
                if (player.GetComponent<BuffEli>().slamming && !dying)
                {
                    dying = true;
                    StartCoroutine(GuardDie());
                }
                Vector3 direction = -1 * (player.transform.position - transform.position).normalized;
                if (Vector3.Dot(direction, Vector3.right) > 0)
                {
                    directionFacing = 1;
                }
                else
                {
                    directionFacing = -1;
                }
                if (directionFacing != prevDir)
                {
                    anim.SetBool("FlipDir", directionFacing == -1);
                    anim.SetTrigger("Flip");
                    prevDir = directionFacing;
                }
                rb.velocity = direction * moveSpeed;
            }
            else
            {
                if (CanSeePlayer())
                {
                    playerVisibleTimer += Time.deltaTime;
                    anim.SetBool("Walking", false);
                    rb.velocity = Vector3.zero;
                }
                else
                {
                    playerVisibleTimer -= Time.deltaTime;
                    Patrol();
                }
                playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);
                spotlight.color = Color.Lerp(originalColor, Color.red, playerVisibleTimer);

                if (playerVisibleTimer >= timeToSpotPlayer)
                {
                    player.GetComponent<PlayerController>().Die();
                }
            }
        }
    }

    void Patrol()
    {
        Vector3 direction = waypoints[waypointIndex].position - transform.position;
        direction.Normalize();
        rb.velocity = direction * moveSpeed;
        anim.SetBool("Walking", true);
        if (rb.velocity != Vector3.zero)
        {
            if (Vector3.Dot(direction, Vector3.right) > 0)
            {
                directionFacing = 1;
            }
            else
            {
                directionFacing = -1;
            }
            if (directionFacing != prevDir)
            {
                anim.SetBool("FlipDir", directionFacing == -1);
                anim.SetTrigger("Flip");
                prevDir = directionFacing;
            }
        }
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
        Destroy(Instantiate(stunParticle, transform.position + Vector3.up, Quaternion.identity), flashDuration);
        anim.SetBool("Walking", false);
        yield return new WaitForSeconds(flashDuration);
        flashed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SlamRange") && player.GetComponent<BuffEli>().enabled)
        {
            if(player.gameObject.GetComponent<BuffEli>().lethal)
            {
                StartCoroutine(GuardDie());
            }
            else
            {
                scared = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SlamRange") && player.GetComponent<BuffEli>().enabled)
        {
            scared = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player.gameObject.GetComponent<BuffEli>().lethal)
            {
                StartCoroutine(GuardDie());
            }
        }
    }

    public IEnumerator GuardDie()
    {
        anim.SetTrigger("Die");
        yield return new WaitForSeconds(1.1f);
        Destroy(gameObject);
    }
}
