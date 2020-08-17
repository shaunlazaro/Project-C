using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private EnemySpawnController spawnController;
    public EnemySpawnController SpawnController
    {
        set{spawnController = value;}
    }
    private SpriteRenderer enemyRenderer;
    public Rigidbody2D rigid;
    public float cannotActTimeLeft;
    public float initialStun;
    public float maxHp;
    private float currentHp;
    public float HP
    {
        get{return currentHp;}
        set{currentHp = value;
            spawnController.CheckDeath();}
    }
    public bool canAct
    {
        get {return cannotActTimeLeft < 0 && hitStunLeft <= 0;}
    }
    public float HitStunLeft
    {
        get{return hitStunLeft;}
    }

    public bool looksAtPlayer;

    // Weak and strong knockback resistance.
    public bool knockBackResist;
    public bool knockBackImmune;

    private float hitStunLeft;

    public Vector2 knockBackDirection;
    private bool invulnerable;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        enemyRenderer = gameObject.GetComponent<SpriteRenderer>();
        currentHp = maxHp;
        knockBackDirection.Normalize();
    }


    // Update is called once per frame
    void Update()
    {
        if(hitStunLeft >= 0)
            hitStunLeft -= Time.deltaTime;
        if(cannotActTimeLeft >= 0)
            cannotActTimeLeft -= Time.deltaTime;

        if(GameObject.FindGameObjectWithTag("Player").transform.position.x > transform.position.x && looksAtPlayer)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x)*-1,transform.localScale.y,transform.localScale.z);
        }
        else if (looksAtPlayer)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x),transform.localScale.y,transform.localScale.z);
        }
    }

    public void GetHit(float damage, float knockBack, float hitStun, bool strongKnockback = false)
    {
        if(!invulnerable)
        {
        HP -= damage;
        if(!knockBackImmune || (!knockBackResist && !strongKnockback))
        {
            rigid.velocity = knockBack * knockBackDirection;
            hitStunLeft += hitStun; // Movement script must not change velocity during hitstun, or knockback will not apply.
        }
        StartCoroutine(FlashOnHit());
        }
    }

    private IEnumerator FlashOnHit()
    {
        invulnerable = true;
        enemyRenderer.color = Color.red;
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();        
        enemyRenderer.color = Color.white;
        invulnerable = false;
    }
}
