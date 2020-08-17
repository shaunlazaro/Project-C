using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{

    public GameObject boundary;
    public GameObject enemyPrefab;
    private GameObject enemy;

    public bool dead;
    public bool respawn;

    void Update()
    {
        if (enemy == null && boundary.activeSelf && !dead)
        {
            enemy = GameObject.Instantiate(enemyPrefab,transform.position, Quaternion.identity);
            enemy.GetComponent<EnemyController>().SpawnController = this;
        }
        if (enemy != null && !boundary.activeSelf)
        {
            Destroy(enemy);
        }
        if(!boundary.activeSelf && respawn)
        {
            dead = false;
        }
    }

    public void CheckDeath()
    {
        if (enemy.GetComponent<EnemyController>().HP <= 0)
        {
            Destroy(enemy);
            dead = true;
        }
    }

}
