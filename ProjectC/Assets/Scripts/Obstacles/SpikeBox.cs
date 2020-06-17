using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBox : MonoBehaviour
{
    public float damage;
    public float hitStun;

    public bool hardSpike;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D toucher)
    {
        if(toucher.gameObject.name == "Player")
        {
            Debug.Log("You've touched a spike!");
            toucher.gameObject.GetComponent<PlayerController>().GetHit(damage,hitStun, !hardSpike);
            if(hardSpike)
            {
                ManagerSingleton.Instance.data.RespawnPlayer();
            }
        }
    }

}
