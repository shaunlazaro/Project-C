using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private DataManager dataManager;
    public bool isHealingCheckpoint;

    // Start is called before the first frame update
    void Start()
    {
        dataManager = ManagerSingleton.Instance.data;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            dataManager.CheckpointTouched(this, isHealingCheckpoint);
        }
    }
}
