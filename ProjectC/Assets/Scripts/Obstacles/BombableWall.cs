using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombableWall : MonoBehaviour
{
    public void DestroyWall()
    {
        Destroy(gameObject);
    }
}
