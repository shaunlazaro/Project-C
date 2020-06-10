using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerSingleton : MonoBehaviour
{
    public static ManagerSingleton Instance {get;private set;}

    public  DataManager data;

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
    }
    
    public void Start()
    {
        data = this.gameObject.GetComponent<DataManager>();
    }
}
