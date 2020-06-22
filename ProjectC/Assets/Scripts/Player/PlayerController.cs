using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayerMask;
    private Rigidbody2D rigid;
    private Transform trans;
    private Collider2D hitbox;
    private SpriteRenderer playerSprite;
    private Animator anim;
    public float runSpeed;
    public bool isGrounded;

    public float coyoteBuffer;
    private float coyoteBufferLeft;

    public float jumpPower;
    public float doubleJumpPower;
    public float jumpBuffer;
    private float jumpBufferLeft;
    public float jumpFallingBoost;
    public float fallSpeedCap;

    public float dashSpeed;
    private Vector2 dashVelocity;
    private Vector2 dashVelocityLeft;
    public float dashTime;
    private float dashTimeLeft;
    private float originalGravityScale;

    private bool airDashAllowed = false;
    private bool doubleJumpAllowed = false;
    private float timeSinceGroundedJump = 0;
    public float minimumJumpTimeBeforeFastFallAllowed;
    public float minimumJumpTimeBeforeDoubleJumpAllowed;

    private bool isSlidingDownWall;
    public float wallSlideSpeed; //Max falling speed
    public Vector2 wallJumpDirection;
    public float wallJumpForce;
    public float numberOfWallJumpsAllowed;
    private float wallJumpsUsed;
    public float wallJumpStun;
    private float wallJumpStunLeft = 0;
    public float timeBeforeGrappleWallAgain;

    /*public Transform cameraTarget;
    public float camTargetDistance, camTargetSpeed;*/

    public float bombVelocityX,bombVelocityY;
    public GameObject bombPrefab,bombDropPosition;

    private DataManager data;
    private UnlockManager unlocks;
    public float hitstunTime;
    public float knockBackForce;
    public bool facingRight;


    // Start is called before the first frame update
    void Start()
    {
        rigid = this.GetComponent<Rigidbody2D>();
        trans = this.GetComponent<Transform>();
        hitbox = this.GetComponent<Collider2D>();
        playerSprite = this.GetComponent<SpriteRenderer>();
        anim = this.GetComponent<Animator>();
        data = ManagerSingleton.Instance.data;
        unlocks = ManagerSingleton.Instance.unlocks;

        dashVelocity = new Vector2(dashSpeed, 0);
        dashVelocityLeft = new Vector2(-dashSpeed, 0);
        originalGravityScale = rigid.gravityScale;
        
        wallJumpDirection.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        // Movement: Only happens if player isnt in hitstun
        if (hitstunTime > 0)
        {
            hitstunTime -= Time.deltaTime;
            //TODO: Hurt Frame
            //When player lands, remove the player's velocity:
            if (IsGrounded() && rigid.velocity.y <= 0.1)
            {
                rigid.velocity = new Vector2(0, rigid.velocity.y);
            }
        }
        else
        {
            if (dashTimeLeft > 0)
            {
                dashTimeLeft -= Time.deltaTime;
            }
            else if (wallJumpStunLeft > 0)
            {
                wallJumpStunLeft -= Time.deltaTime;
                if(wallJumpStun - wallJumpStunLeft > timeBeforeGrappleWallAgain &&IsTouchingWall())
                {
                    //Allow another walljump earlier than other actions.
                    if(Input.GetButtonDown("Jump") && wallJumpsUsed < numberOfWallJumpsAllowed)
                    {
                        wallJumpStunLeft = wallJumpStun;
                        wallJumpsUsed++;
                        if(!facingRight)
                        rigid.velocity = new Vector2(wallJumpDirection.x*wallJumpForce, wallJumpDirection.y*wallJumpForce);
                        else
                            rigid.velocity = new Vector2(wallJumpDirection.x*-wallJumpForce, wallJumpDirection.y*wallJumpForce);
                        facingRight = !facingRight;
                        trans.localScale = new Vector3(trans.localScale.x*-1, trans.localScale.y, trans.localScale.z);
                    }
                }
                // Minimum time passed, + not touching wall OR no wall jumps left
                else if(wallJumpStun - wallJumpStunLeft > timeBeforeGrappleWallAgain && 
                    (!IsTouchingWall() || wallJumpsUsed == numberOfWallJumpsAllowed))
                {
                    //Allow early double jump.
                    if(Input.GetButtonDown("Jump"))
                    {
                        rigid.velocity = new Vector2(rigid.velocity.x, doubleJumpPower);
                        doubleJumpAllowed = false;                        
                    }
                }
            }
            else
            {
                // Check if grounded before anything else!
                isGrounded = (IsGrounded() && rigid.velocity.y <= 0.1);
                isSlidingDownWall = IsTouchingWall() && !isGrounded && rigid.velocity.y < 0;

                #region Movement
                rigid.gravityScale = originalGravityScale;
                
                // Walking
                if (Input.GetAxisRaw("Horizontal") != 0)
                {
                    rigid.velocity = new Vector2(runSpeed * Input.GetAxisRaw("Horizontal"), rigid.velocity.y);
                }
                else
                {
                    rigid.velocity = new Vector2(0, rigid.velocity.y);
                }
                // Flip sprite accordingly
                if (Input.GetAxisRaw("Horizontal") != 0)
                {
                    trans.localScale = new Vector3(Input.GetAxisRaw("Horizontal"), 1, 1);
                    if (Input.GetAxisRaw("Horizontal") < 0.1)
                        facingRight = false;
                    else
                        facingRight = true;
                }
                // Dashing
                if (Input.GetButtonDown("Dash") && (airDashAllowed || isGrounded) && unlocks.DashUnlocked)
                {
                    if(!isGrounded)
                        airDashAllowed = false;
                    
                    rigid.gravityScale = 0;
                    dashTimeLeft = dashTime;
                    if(IsTouchingWall())
                    {
                        if (facingRight)
                        {
                            rigid.velocity = dashVelocityLeft;
                        }
                        else
                        {
                            rigid.velocity = dashVelocity;
                        }
                    }
                    else
                    {
                        if (facingRight)
                        {
                            rigid.velocity = dashVelocity;
                        }
                        else
                        {
                            rigid.velocity = dashVelocityLeft;
                        }
                    }
                    
                    coyoteBufferLeft = 0; // Prevents strange jumping allowed after dash.
                    isGrounded = false;
                }
                #endregion

                #region Vertical Movement
                
                // Reset Air Dash, Double Jump, Wall Jump
                if(isGrounded)
                {
                    airDashAllowed = true;
                    doubleJumpAllowed = true;
                    timeSinceGroundedJump = 0;
                    wallJumpsUsed = 0;
                }
                else{
                    timeSinceGroundedJump += Time.deltaTime;
                }
                // Coyote time, part of jump check
                if (isGrounded)
                    coyoteBufferLeft = coyoteBuffer;
                else
                    coyoteBufferLeft -= Time.deltaTime;

                // Attempted Jump, Stores input in buffer
                if (Input.GetButtonDown("Jump"))
                    jumpBufferLeft = jumpBuffer;
                else
                    jumpBufferLeft -= Time.deltaTime;

                // If coyote buffer is still left, then the player is considered to be on the ground.
                if (jumpBufferLeft > 0 && coyoteBufferLeft > 0)
                {
                    rigid.velocity = new Vector2(rigid.velocity.x*1.1f, jumpPower);
                    jumpBufferLeft = 0;
                    coyoteBufferLeft = 0;
                }

                // Short hop, but only when rising and only on the first hop, not the second.
                if (Input.GetButtonUp("Jump") && rigid.velocity.y > 0 && doubleJumpAllowed)
                {
                    if(timeSinceGroundedJump < minimumJumpTimeBeforeFastFallAllowed)
                        StartCoroutine(WaitThenShortenJump(minimumJumpTimeBeforeFastFallAllowed - timeSinceGroundedJump));
                    else
                        rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * 0.1f);
                }

                // Double Jump
                if(jumpBufferLeft > 0 && !isGrounded && unlocks.DoubleJumpUnlocked && doubleJumpAllowed && 
                    (!IsTouchingWall() || wallJumpsUsed == numberOfWallJumpsAllowed))
                {
                    rigid.velocity = new Vector2(rigid.velocity.x, doubleJumpPower);
                    doubleJumpAllowed = false;
                    jumpBufferLeft = 0;
                }
                // Wall Jump
                else if(jumpBufferLeft > 0 && !isGrounded && unlocks.WallJumpUnlocked && wallJumpsUsed < numberOfWallJumpsAllowed &&IsTouchingWall())
                {
                    jumpBufferLeft = 0;
                    wallJumpStunLeft = wallJumpStun;
                    wallJumpsUsed++;
                    if(!facingRight)
                        rigid.velocity = new Vector2(wallJumpDirection.x*wallJumpForce, wallJumpDirection.y*wallJumpForce);
                    else
                        rigid.velocity = new Vector2(wallJumpDirection.x*-wallJumpForce, wallJumpDirection.y*wallJumpForce);
                    facingRight = !facingRight;
                    trans.localScale = new Vector3(trans.localScale.x*-1, trans.localScale.y, trans.localScale.z);
                }

                // Always Fast Falling
                if (rigid.velocity.y < 0 )
                {
                    rigid.velocity += Vector2.up * Physics2D.gravity.y * Time.deltaTime * jumpFallingBoost;
                }
                if(rigid.velocity.y < fallSpeedCap)
                {
                    rigid.velocity = new Vector2(rigid.velocity.x, fallSpeedCap);
                }



                // Limit vertical speed if clinging to wall
                if(isSlidingDownWall && rigid.velocity.y < wallSlideSpeed) rigid.velocity = new Vector2(rigid.velocity.x, wallSlideSpeed);
                #endregion

                #region Attacks
                if (Input.GetButtonDown("Attack") && isGrounded)
                {
                    Debug.Log("Attack Button Pressed");
                    if (Input.GetAxisRaw("Vertical") < -0.1f && unlocks.BombUnlocked) //Hold Down + Attack = Bomb
                    {
                        Debug.Log("Bomb Attack Attempted");
                        GameObject bombDropped = Instantiate(bombPrefab, bombDropPosition.transform.position, Quaternion.identity);
                        Physics2D.IgnoreCollision(hitbox, bombDropped.GetComponent<Collider2D>());
                        bombDropped.GetComponent<Rigidbody2D>().velocity = new Vector2(bombVelocityX * transform.localScale.x, bombVelocityY);
                    }
                }
                #endregion
            }


            #region Animations
            // Walking Animations
            anim.SetFloat("Walking", Mathf.Abs(Input.GetAxisRaw("Horizontal")));
            #endregion

        }

        #region OldCamCode
        /* Camera
        if (hitstunTime <=0)
        {
            if(Input.GetAxisRaw("Horizontal") != 0)
            {
                cameraTarget.localPosition = new Vector3(
                    Mathf.Lerp(cameraTarget.localPosition.x, camTargetDistance*Input.GetAxisRaw("Horizontal"), camTargetSpeed*Time.deltaTime),
                    cameraTarget.localPosition.y, cameraTarget.localPosition.z);
            }
        }
        else
        {
            // Camera While Hurting:
        }*/
        #endregion

    }

    // Used to force a minimum amount of time before short hop is activated.
    public IEnumerator WaitThenShortenJump(float timeRequired)
    {
        yield return new WaitForSeconds(timeRequired);
        rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * 0.1f);
    }

    public void GetHit(float damage, float hitStun, bool isKnockedBack)
    {
        data.HP = data.HP - damage;
        hitstunTime+=hitStun;
        if(data.HP <= 0) Debug.Log("Dead!");
        if(isKnockedBack)
            StartCoroutine(GetKnockedBack());

    }
    IEnumerator GetKnockedBack()
    {
        yield return new WaitForEndOfFrame();
        if(facingRight)
            rigid.velocity = new Vector2(-knockBackForce, knockBackForce);
        else
            rigid.velocity = new Vector2(knockBackForce, knockBackForce);
    }

    private bool IsGrounded()
    {
        Collider2D rayIntersect = Physics2D.BoxCast(hitbox.bounds.center, new Vector2(hitbox.bounds.size.x*0.9f, hitbox.bounds.size.y),
        0f, Vector2.down, 0.1f, platformLayerMask).collider; 
        //Physics2D.Raycast(hitbox.bounds.center, Vector2.down, 
        //    hitbox.bounds.extents.y+0.1f, platformLayerMask).collider;
        bool grounded =  rayIntersect != null;
        Color rayColor = Color.red;
        Debug.DrawRay(hitbox.bounds.center, Vector2.down * (hitbox.bounds.extents.y+0.1f), rayColor);
        return grounded;
    }

    private bool IsTouchingWall()
    {
        Collider2D rayIntersect;
        Color rayColor = Color.magenta;

        if(facingRight)
        {
            rayIntersect = Physics2D.BoxCast(hitbox.bounds.center, new Vector2(hitbox.bounds.size.x, hitbox.bounds.size.y),
                0f, Vector2.right, 0.05f, platformLayerMask).collider; 
            Debug.DrawRay(hitbox.bounds.center, Vector2.right * (hitbox.bounds.extents.x+0.05f), rayColor);
        }
        else
        {
            rayIntersect = Physics2D.BoxCast(hitbox.bounds.center, new Vector2(hitbox.bounds.size.x, hitbox.bounds.size.y),
                0f, Vector2.left, 0.05f, platformLayerMask).collider; 
            Debug.DrawRay(hitbox.bounds.center, Vector2.left * (hitbox.bounds.extents.x+0.05f), rayColor);
        }
        bool grounded =  rayIntersect != null;
        return grounded;
    }
}
