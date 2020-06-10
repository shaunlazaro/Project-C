using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryManager : MonoBehaviour
{

    private BoxCollider2D managerBox; // Boxcollider of the manager 
    //(boxcollider of the child is identical, but the gameobject is toggled off most of the time)
    private Transform player;
    private GameObject boundary; // the child boundary - only one will ever be toggled on, and the camera follows it 

    
    void Start()
    {
        managerBox = this.GetComponent<BoxCollider2D>();
        player = GameObject.Find("Player").transform;
        boundary = transform.GetChild(0).gameObject;
        CheckIfPlayerPresent();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfPlayerPresent();
    }

    void CheckIfPlayerPresent()
    {
        if(managerBox.bounds.min.x < player.position.x && player.position.x < managerBox.bounds.max.x &&
            managerBox.bounds.min.y < player.position.y && player.position.y < managerBox.bounds.max.y) //Checks if player falls within the boxcollider
        {
            boundary.SetActive(true);
        }
        else
        {
            boundary.SetActive(false);
        }
    }
}
