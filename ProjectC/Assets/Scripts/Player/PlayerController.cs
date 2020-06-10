﻿using System.Collections;
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
    public float jumpBuffer;
    private float jumpBufferLeft;

    /*public Transform cameraTarget;
    public float camTargetDistance, camTargetSpeed;*/

    public float bombVelocityX,bombVelocityY;
    public GameObject bombPrefab,bombDropPosition;

    private DataManager data;
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
            // Horizontal Movement
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                rigid.velocity = new Vector2(runSpeed * Input.GetAxisRaw("Horizontal"), rigid.velocity.y);
            }
            else
            {
                rigid.velocity = new Vector2(0, rigid.velocity.y);
            }
            // Flip sprite according to movement
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                trans.localScale = new Vector3(Input.GetAxisRaw("Horizontal"), 1, 1);
                if (Input.GetAxisRaw("Horizontal") < 0.1)
                    facingRight = false;
                else
                    facingRight = true;
            }
            isGrounded = (IsGrounded() && rigid.velocity.y <= 0.1);
            Debug.Log("Y Velocity = " + rigid.velocity.y);

            if (isGrounded)
                coyoteBufferLeft = coyoteBuffer;
            else
                coyoteBufferLeft -= Time.deltaTime;

            // Attempted Jump, Buffers for jumpBuffer seconds
            if (Input.GetButtonDown("Jump"))
                jumpBufferLeft = jumpBuffer;
            else
                jumpBufferLeft -= Time.deltaTime;

            // Vertical Movement
            if (jumpBufferLeft > 0 && coyoteBufferLeft > 0)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jumpPower);
                jumpBufferLeft = 0;
                coyoteBufferLeft = 0;
            }
            // Short Jump
            if (Input.GetButtonUp("Jump") && rigid.velocity.y > 0)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y * 0.75f);
            }
            
            // Attacks
            if(Input.GetButtonDown("Fire1") && isGrounded)
            {
                Debug.Log("Attack Button Pressed");
                if(Input.GetAxisRaw("Vertical") < -0.1f) //Hold Down + Attack = Bomb
                {
                    Debug.Log("Bomb Attack Attempted");
                    GameObject bombDropped = Instantiate(bombPrefab, bombDropPosition.transform.position, Quaternion.identity);
                    Physics2D.IgnoreCollision(hitbox, bombDropped.GetComponent<Collider2D>());
                    bombDropped.GetComponent<Rigidbody2D>().velocity = new Vector2(rigid.velocity.x + bombVelocityX*transform.localScale.x,bombVelocityY);
                }
            }
            
            // Walking Animations
            anim.SetFloat("Walking", Mathf.Abs(Input.GetAxisRaw("Horizontal")));


        }

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



    }

    public void GetHit(float damage, float hitStun)
    {
        data.HP-=damage;
        hitstunTime+=hitStun;
        if(data.HP <= 0) Debug.Log("Dead!");
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
        0f, Vector2.down, 0.15f, platformLayerMask).collider; 
        //Physics2D.Raycast(hitbox.bounds.center, Vector2.down, 
        //    hitbox.bounds.extents.y+0.1f, platformLayerMask).collider;
        bool grounded =  rayIntersect != null;
        Color rayColor = Color.red;
        Debug.DrawRay(hitbox.bounds.center, Vector2.down * (hitbox.bounds.extents.y+0.1f), rayColor);
        Debug.Log("Grounded: " + grounded);
        return grounded;
    }
}
