using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public Rigidbody2D rigid;
    public float cannotActTimeLeft;
    public float initialStun;

    public bool canAct
    {
        get {return cannotActTimeLeft < 0;}
    }

    // Start is called before the first frame update
    void Awake()
    {
        rigid = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(cannotActTimeLeft >= 0)
            cannotActTimeLeft -= Time.deltaTime;
    }
}
