using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanel : MonoBehaviour
{
    public GameObject player;
    private PlayerController controller;
    public GameObject b1,b2,b3,b4,b5;
    private Text t1,t2,t3,t4,t5;
    // Start is called before the first frame update
    void Start()
    {
        t1 = b1.GetComponentInChildren<Text>();
        t2 = b2.GetComponentInChildren<Text>();
        t3 = b3.GetComponentInChildren<Text>();
        t4 = b4.GetComponentInChildren<Text>();
        t5 = b5.GetComponentInChildren<Text>();

        controller = player.GetComponent<PlayerController>();        
    }

    public void b1Press()
    {
        
    }
    public void b2Press()
    {
        
    }
    public void b3Press()
    {
        
    }
    public void b4Press()
    {
        
    }
    public void b5Press()
    {
        
    }
}
