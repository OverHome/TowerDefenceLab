using System;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform towerPos;
    [SerializeField] private EnemyScript enemyScript;
    private void Start()
    {
        EnemyScript.TowerPos = towerPos;
        var enemy = Instantiate(enemyScript.gameObject, transform.position, transform.rotation, transform.parent);
    }
}
