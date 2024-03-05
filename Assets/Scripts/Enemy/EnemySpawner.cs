using System;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform towerPos;
    [SerializeField] private EnemyScript enemyScript;
    private void Start()
    {
        EnemyScript.TowerPos = towerPos;
        InvokeRepeating("SpawnEnemy", 0.0f, 0.5f);
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyScript.gameObject, transform.position, transform.rotation, transform.parent);
    }
}
