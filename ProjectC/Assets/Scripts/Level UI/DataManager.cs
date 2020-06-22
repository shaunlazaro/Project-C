using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private UIManager graphics;

    public Checkpoint currentCheckpoint;

    public void RespawnPlayer()
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = currentCheckpoint.transform.position;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void CheckpointTouched(Checkpoint checkpointTouched, bool isHealing)
    {
        currentCheckpoint = checkpointTouched;
        if(isHealing)
        {
            HP = maxHP;
        }
    }

    public float HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            graphics.UpdateHP(hp,maxHP);
        }
    }
    private float hp;
    public float MaxHP
    {
        get{ return maxHP;}
        set{ maxHP = value;}
    }
    private float maxHP;


    public float desiredMaxHPTemp;

    public void Start()
    {
        graphics = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        maxHP = desiredMaxHPTemp;
        hp = maxHP;
        StartCoroutine(UpdateHPInASec());
    }

    private IEnumerator UpdateHPInASec()
    {
        yield return new WaitForEndOfFrame();
        HP = hp;
    }

}
