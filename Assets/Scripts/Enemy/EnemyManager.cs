using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    private List<EnemyScript> enemies = new List<EnemyScript>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterEnemy(EnemyScript enemy)
    {
        enemies.Add(enemy);
    }

    public void UnregisterEnemy(EnemyScript enemy)
    {
        enemies.Remove(enemy);
    }

    public GameObject FindNearestEnemy(Vector3 position, float shortestDistance)
    {
        EnemyScript nearestEnemy = null;

        foreach (EnemyScript enemy in enemies)
        {
            float distance = Vector3.Distance(position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy != null ? nearestEnemy.gameObject : null;
    }

    public GameObject FindBiggestEnemy(Vector3 position, float shortestDistance)
    {
        if (enemies.Count == 0) return null;
        
        EnemyScript nearestEnemy = enemies[0];

        foreach (EnemyScript enemy in enemies)
        {
            float distance = Vector3.Distance(position, enemy.transform.position);
            if (distance < shortestDistance && nearestEnemy.StartHealth < enemy.StartHealth)
            {
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy != null ? nearestEnemy.gameObject : null;
    }

    public bool AreEnemiesAlive()
    {
        return enemies.Count != 0;
    }
}