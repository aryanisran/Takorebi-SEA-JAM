using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    SpriteRenderer sr;
    public Animator anim;
    Vector2 moveInput;
    [SerializeField] public float moveSpeed, jumpForce, jumpTime, groundCheckRadius , groundCheckDistance;

    [SerializeField] GameObject blackScreen;
    [SerializeField] GameObject keySprite;
    [SerializeField] GameObject oldAbilities, buffAbilities;

    public bool isGrounded, isJumping;
    float maxJumpTime;
    [SerializeField] LayerMask realGround;

    OldEli oldEli;
    BuffEli buffEli;

    public int directionFacing;
    int prevDir;
    public int keysHolding;

    public GameObject pauseScreen, settingsScreen;
    bool paused;
    public bool settingsOpen { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        maxJumpTime = jumpTime;

        directionFacing = 1;
        oldEli = GetComponent<OldEli>();
        buffEli = GetComponent<BuffEli>();

        int i = PlayerPrefs.GetInt("Checkpoint", 0);
        Debug.Log(PlayerPrefs.GetInt("Checkpoint", 0));
        transform.position = new Vector3(oldEli.checkPoints[i].position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (keysHolding >= 1)
        {
            keySprite.SetActive(true);
        }
        else { keySprite.SetActive(false); }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
            {
                pauseScreen.SetActive(true);
                Time.timeScale = 0;
                paused = true;
            }
            else
            {
                if (settingsOpen)
                {
                    settingsScreen.SetActive(false);
                    settingsOpen = false;
                }
                else
                {
                    pauseScreen.SetActive(false);
                    Time.timeScale = 1;
                    paused = false;
                }
            }
        }

        if (oldEli.isActiveAndEnabled == true)
        {
            oldAbilities.SetActive(true);
            buffAbilities.SetActive(false);
        }
        else
        {
            oldAbilities.SetActive(false);
            buffAbilities.SetActive(true);
        }
        
        #region Movement
        if (!oldEli.isBuilding)
        {
            //Take arrow key input and store it in Vector2
            moveInput.x = Input.GetAxis("Horizontal") * moveSpeed;
            moveInput.y = Input.GetAxis("Vertical") * moveSpeed;

            //Set walking animation when we move in x or z direction, set back to idle when we're not moving
            if (moveInput != Vector2.zero)
            {
                anim.SetBool("Walking", true);
            }
            else
            {
                anim.SetBool("Walking", false);
            }

            //Flip sprite if we walk backwards, flip back when we walk forwards
            if (moveInput.x < 0)
            {
                directionFacing = -1;
            }
            else if (moveInput.x > 0)
            {
                directionFacing = 1;
            }

            if(directionFacing != prevDir)
            {
                anim.SetBool("FlipDir", directionFacing == -1);
                anim.SetTrigger("Flip");
                prevDir = directionFacing;
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
            AudioManager.instance.Play("Jump");
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
        if (CanMoveCheck())
        {
            rb.velocity = new Vector3(moveInput.x, rb.velocity.y, moveInput.y);
        }
        //Read vertical speed and send it to animator
        anim.SetFloat("dY", rb.velocity.y);
    }

    bool CanMoveCheck()
    {
        if (oldEli.enabled)
        {
            if (oldEli.isBuilding)
            {
                return false;
            }
        }
        else if (buffEli.enabled)
        {
            if (buffEli.kicking)
            {
                return false;
            }
        }
        return true;
    }

    public IEnumerator Death()
    {
        if (oldEli.isActiveAndEnabled)
        {
            anim.SetTrigger("Die");
            yield return new WaitForSeconds(1f);
            blackScreen.GetComponent<Animator>().SetTrigger("Die");
            yield return new WaitForSeconds(1f);
            Respawn();
        }
    }
    public void Die()
    {
        StartCoroutine(Death());
    }

    public void Respawn()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SwitchForm()
    {
        if (oldEli.enabled == true)
        {
            oldEli.enabled = false;
            buffEli.enabled = true;
            AudioManager.instance.Stop("Old BGM");
            AudioManager.instance.Play("Young BGM");

            // OLD ELI ANIMATION -> BUFF ELI ANIMATIONS
        } 
        else if (buffEli.enabled == true)
        {
            buffEli.enabled = false;
            oldEli.enabled = true;
            AudioManager.instance.Play("Old BGM");
            AudioManager.instance.Stop("Young BGM");

            // BUFF ELI ANIMATIONS -> OLD ELI ANIMATIONS
        }
    }
}
