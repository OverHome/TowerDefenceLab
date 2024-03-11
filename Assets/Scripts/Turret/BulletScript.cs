using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] public float Damage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Turret"))
        {
            return;
        }
        else if (other.CompareTag("Enemy"))
        {
            EnemyScript enemyScript = other.GetComponent<EnemyScript>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(Damage);
            }
        }

        Destroy(gameObject);
    }
}
