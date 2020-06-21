using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerSingleton : MonoBehaviour
{
    public static ManagerSingleton Instance {get;private set;}

    public DataManager data;
    public UnlockManager unlocks;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        data = gameObject.GetComponent<DataManager>();
        unlocks = gameObject.GetComponent<UnlockManager>();
    }
    
    public void Start()
    {
    }
}
