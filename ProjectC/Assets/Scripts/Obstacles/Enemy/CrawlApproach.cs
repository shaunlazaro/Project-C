﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrawlApproach : MonoBehaviour
{
    private GameObject player;
    private Transform playerTransform;
    private EnemyController controller;
    private Rigidbody2D rigid;

    public float acceptableDistance; // Set to 0 for chase enemies.
    public float thrust;

    public float jumpThrustXBase;
    private float jumpThrustX;
    public float jumpThrustY;
    public float jumpDistance;
    public float jumpCooldown;
    public float jumpStun; // Amount of time to pause after jumping.
    private float jumpCooldownLeft;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerTransform = player.transform;
        controller = gameObject.GetComponent<EnemyController>();
        rigid = controller.rigid;
    }

    // Update is called once per frame
    void Update()
    {
        //Jump
        if(jumpCooldownLeft > 0)
        {
            jumpCooldownLeft -=Time.deltaTime;
        }
        else if(Vector2.Distance(transform.position, playerTransform.position) < jumpDistance)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, jumpThrustY);
            jumpCooldownLeft = jumpCooldown;
            jumpThrustX = jumpThrustXBase * horizontalDirectionModifier();

            controller.cannotActTimeLeft = jumpStun;
        }
        if(System.Math.Round(rigid.velocity.y,2) != 0)
        {
            rigid.velocity = new Vector2(jumpThrustX, rigid.velocity.y);
        }
        else if(Vector2.Distance(transform.position, playerTransform.position) > acceptableDistance)
        {
            //rigid.AddForce(new Vector2(thrust*horizontalDirectionModifier(), 0));
            rigid.velocity = new Vector2(thrust*horizontalDirectionModifier(), rigid.velocity.y);
        }
    }

    
    public int horizontalDirectionModifier()
    {
        if(!controller.canAct)
            return 0;

        if(transform.position.x > playerTransform.position.x)
            return -1;
        else
            return 1;
    }
}
