using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    public Transform target;
    public float fireRate = 1.0f;
    public float range = 10.0f;
    public GameObject projectile;

    private float nextFireTime = 0.0f;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0.0f, 0.5f);
    }

    void UpdateTarget()
    {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null)
        {
            target = nearestEnemy.transform;
        }
    }

    void Update()
    {
        // Поворот турели к цели
        if (target != null)
        {
            transform.LookAt(target);
        }

        // Стрельба по цели
        if (Time.time >= nextFireTime && target != null)
        {
            nextFireTime = Time.time + 1.0f / fireRate;
            
            // Создание снаряда
            GameObject projectileInstance = Instantiate(projectile, transform.position, transform.rotation);
            projectileInstance.transform.LookAt(target);;
            // Направление снаряда к цели
            projectileInstance.GetComponent<Rigidbody>().AddForce(transform.forward * 5.0f, ForceMode.Impulse);
        }
    }
}
