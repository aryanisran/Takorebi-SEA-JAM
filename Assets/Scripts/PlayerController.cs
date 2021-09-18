using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    SpriteRenderer sr;
    Animator anim;
    Vector2 moveInput;
    [SerializeField] float moveSpeed, jumpForce, jumpTime, groundCheckRadius , groundCheckDistance;

    bool isGrounded, isJumping;
    float maxJumpTime;
    [SerializeField] LayerMask realGround;

    bool isBuilding, prevFrame, canBuild;
    public GameObject guidePrefab, platformPrefab;
    GameObject guide, platform;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        maxJumpTime = jumpTime;
        canBuild = true;
    }

    // Update is called once per frame
    void Update()
    {
        #region Build Platform
        //Make the guide when we press X
        if (Input.GetKeyDown(KeyCode.X) && !isBuilding && canBuild)
        {
            isBuilding = true;
            prevFrame = true;
            //Round the x and y of the platform to the nearest 0.5 so it's in line with the grid
            float roundedX = Mathf.Round((transform.position.x + 1) * 2) / 2;
            float roundedY = Mathf.Round((transform.position.y) * 2) / 2;
            guide = Instantiate(guidePrefab, new Vector3(roundedX, roundedY, 0), Quaternion.identity);
        }
        if (isBuilding)
        {
            //Move the guide when pressing arrow buttons
            Vector3 inputDir;
            inputDir = Vector3.zero;
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                inputDir += Vector3.right;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                inputDir += Vector3.left;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                inputDir += Vector3.up;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                inputDir += Vector3.down;
            }
            if(Vector3.Distance(guide.transform.position + inputDir * 0.25f , transform.position) < 1.5)
            {
                guide.transform.position += inputDir * 0.25f;
            }
            //Finalize building when we press X again
            if (Input.GetKeyDown(KeyCode.X) && !prevFrame)
            {
                canBuild = false;
                Destroy(Instantiate(platformPrefab, guide.transform.position, Quaternion.identity), 5f);
                StartCoroutine(WaitPlatformCo());
                //Store a temporary reference to the guide so we don't delete the guide stored in our global variable, just in case it gives us null refernce issues
                GameObject temp = guide;
                guide = null;
                Destroy(temp);
                isBuilding = false;
            }
            //This is to make sure we don't finalize building on the same frame we first pressed X
            if (prevFrame && !Input.GetKey(KeyCode.X))
            {
                prevFrame = false;
            }
        }
        #endregion
        #region Movement
        if (!isBuilding)
        {
            //Take arrow key input and store it in Vector2
            moveInput.x = Input.GetAxis("Horizontal") * moveSpeed;
            moveInput.y = Input.GetAxis("Vertical") * moveSpeed;

            //Flip sprite if we walk backwards, flip back when we walk forwards
            if (moveInput.x < 0)
            {
                //anim.SetBool("FlipDir", true);
                //anim.SetTrigger("Flip");
                sr.flipX = true;
            }
            else if (moveInput.x > 0)
            {
                //anim.SetBool("FlipDir", false);
                //anim.SetTrigger("Flip");
                sr.flipX = false;
            }

            //Set walking animation when we move in x or z direction, set back to idle when we're not moving
            if (moveInput != Vector2.zero)
            {
                anim.SetBool("Walking", true);
            }
            else
            {
                anim.SetBool("Walking", false);
            }
        }
        #endregion
        #region Jump
        //Check if player is touching ground before jumping
        RaycastHit hit;
        isGrounded = Physics.SphereCast(transform.position, groundCheckRadius, Vector3.down, out hit, groundCheckDistance, realGround);

        //Jump when we hit space
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            isJumping = true;
        }

        //Go higher for as long as we are holding space
        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if(jumpTime > 0)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                jumpTime -= Time.deltaTime;
            }
            else
            {
                jumpTime = maxJumpTime;
                isJumping = false;
            }
        }

        //Reset jump variables when we let go of space
        if (Input.GetKeyUp(KeyCode.Space))
        {
            jumpTime = maxJumpTime;
            isJumping = false;
        }
        #endregion
    }

    private void FixedUpdate()
    {
        //Set movement based on input
        if (!isBuilding)
        {
            rb.velocity = new Vector3(moveInput.x, rb.velocity.y, moveInput.y);
        }
        //Read vertical speed and send it to animator
        anim.SetFloat("dY", rb.velocity.y);
    }

    IEnumerator WaitPlatformCo()
    {
        yield return new WaitForSeconds(5f);
        canBuild = true;
    }
}
