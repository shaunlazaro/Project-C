using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{

    public GameObject boundary;
    public GameObject enemyPrefab;
    private GameObject enemy;

    void Update()
    {
        if (enemy == null && boundary.activeSelf)
        {
            enemy = GameObject.Instantiate(enemyPrefab,transform.position, Quaternion.identity);
        }
        if (enemy != null && !boundary.activeSelf)
        {
            Destroy(enemy);
        }
    }

}
