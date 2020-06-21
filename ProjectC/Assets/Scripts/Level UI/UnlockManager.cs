using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnlockManager : MonoBehaviour
{
    public bool DashUnlocked
    {
        get{
            return Convert.ToBoolean(PlayerPrefs.GetInt("DashUnlocked"));
        }
        set{
            PlayerPrefs.SetInt("DashUnlocked", Convert.ToInt32(value));
        }
    }
    public bool DoubleJumpUnlocked
    {
        get{
            return Convert.ToBoolean(PlayerPrefs.GetInt("DoubleJumpUnlocked"));
        }
        set{
            PlayerPrefs.SetInt("DoubleJumpUnlocked", Convert.ToInt32(value));
        }
    }

    public bool BombUnlocked
    {
        get{
            return Convert.ToBoolean(PlayerPrefs.GetInt("BombUnlocked"));
        }
        set{
            PlayerPrefs.SetInt("BombUnlocked", Convert.ToInt32(value));
        }
    }

    public bool WallJumpUnlocked
    {
        get{
            return Convert.ToBoolean(PlayerPrefs.GetInt("WallJumpUnlocked"));
        }
        set{
            PlayerPrefs.SetInt("WallJumpUnlocked", Convert.ToInt32(value));
        }
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.C) && Input.GetKey(KeyCode.L) && Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.R))
        {
            Debug.Log("CLEARING ALL SAVE DATA");
            DashUnlocked = false;
            DoubleJumpUnlocked = false;
            BombUnlocked = false;
            WallJumpUnlocked = false;
        }
    }
}
