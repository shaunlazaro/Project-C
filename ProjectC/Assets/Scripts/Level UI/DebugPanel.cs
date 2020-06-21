using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanel : MonoBehaviour
{
    private GameObject player;
    private PlayerController controller;
    public GameObject panel,b1,b2,b3,b4,b5;
    private Text t1,t2,t3,t4,t5;

    private UnlockManager unlocks;
    // Start is called before the first frame update
    void Start()
    {
        t1 = b1.GetComponentInChildren<Text>();
        t2 = b2.GetComponentInChildren<Text>();
        t3 = b3.GetComponentInChildren<Text>();
        t4 = b4.GetComponentInChildren<Text>();
        t5 = b5.GetComponentInChildren<Text>();

        player = GameObject.FindGameObjectWithTag("Player");
        controller = player.GetComponent<PlayerController>();        
        unlocks = ManagerSingleton.Instance.unlocks;
        ResetText();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.RightAlt))
        {
            panel.SetActive(!panel.activeSelf);
        }
    }

    private void ResetText()
    {
        t1.text = "Dash Unlocked: " + unlocks.DashUnlocked;
        t2.text = "Bomb Unlocked: " + unlocks.BombUnlocked;
        t3.text = "Wall Jump Unlocked: " + unlocks.WallJumpUnlocked;
        t4.text = "Double Jump Unlocked: " + unlocks.DoubleJumpUnlocked;
        t5.text = "UNUSED";

    }

    public void b1Press()
    {
        unlocks.DashUnlocked = !unlocks.DashUnlocked;
        ResetText();
    }
    public void b2Press()
    {
        unlocks.BombUnlocked = !unlocks.BombUnlocked;
        ResetText();
    }
    public void b3Press()
    {
        unlocks.WallJumpUnlocked = !unlocks.WallJumpUnlocked;
        ResetText();
    }
    public void b4Press()
    {
        unlocks.DoubleJumpUnlocked = !unlocks.DoubleJumpUnlocked;
        ResetText();
    }
    public void b5Press()
    {
        Debug.Log("B5, Useless");        
    }
}
