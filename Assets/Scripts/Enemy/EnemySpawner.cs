using System;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform towerPos;
    [SerializeField] private EnemyScript enemyScript;
    [SerializeField] private float spawnTimer;
    private void Start()
    {
        EnemyScript.TowerPos = towerPos;
        InvokeRepeating("SpawnEnemy", 0.0f, spawnTimer);
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyScript.gameObject, transform.position, transform.rotation, transform.parent);
    }
}
