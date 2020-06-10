using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("triggered");
        if(col.gameObject.tag == "DestructibleWall")
        {
            col.gameObject.SetActive(false);
        }
    }
}
