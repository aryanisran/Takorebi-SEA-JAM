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
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        maxJumpTime = jumpTime;
    }

    // Update is called once per frame
    void Update()
    {
        #region Movement
        //Take arrow key input and store it in Vector2
        moveInput.x = Input.GetAxis("Horizontal") * moveSpeed;
        moveInput.y = Input.GetAxis("Vertical") * moveSpeed;

        //Flip sprite if we walk backwards, flip back when we walk forwards
        if (moveInput.x < 0)
        {
            sr.flipX = true;
        }
        else if (moveInput.x > 0)
        {
            sr.flipX = false;
        }

        //Set walking animation when we move in x or z direction, set back to idle when we're not moving
        if(moveInput != Vector2.zero)
        {
            anim.SetBool("Walking", true);
        }
        else
        {
            anim.SetBool("Walking", false);
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
        rb.velocity = new Vector3(moveInput.x, rb.velocity.y, moveInput.y);
        //Read vertical speed and send it to animator
        anim.SetFloat("dY", rb.velocity.y);
    }
}
