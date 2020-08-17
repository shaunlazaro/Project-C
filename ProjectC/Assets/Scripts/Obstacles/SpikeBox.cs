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

    void OnTriggerStay2D(Collider2D toucher)
    {
        if(toucher.gameObject.name == "Player")
        {
            if(!toucher.gameObject.GetComponent<PlayerController>().Invulnerable)
            {
                toucher.gameObject.GetComponent<PlayerController>().GetHit(damage,hitStun, !hardSpike);
                if(hardSpike)
                {
                    ManagerSingleton.Instance.data.RespawnPlayer();
                }
            }
        }
    }

}
