using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{
    private PlayerController player;
    private float damage;
    private float knockback;
    private float hitstun;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        damage = player.swordDamage;
        knockback = player.swordSwingKnockback;
        hitstun = player.swordSwingEnemyStun;
    }

    void OnTriggerEnter2D(Collider2D toucher)
    {
        if(toucher.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit!");
            toucher.gameObject.GetComponent<EnemyController>().GetHit(damage,knockback,hitstun);
            Physics2D.IgnoreCollision(toucher, gameObject.GetComponent<PolygonCollider2D>(), true);
        }
    }

}