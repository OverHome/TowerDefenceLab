using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    protected Rigidbody _rb;
    protected Vector3 _destination;
    protected float _damage;
    protected float _speed;
    protected bool _isTrigerred;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        Invoke(nameof(DestroyObject), 10f);
    }

    public void Initialized(Vector3 destination, float damage, float speed)
    {
        _destination = destination;
        _damage = damage;
        _speed = speed;
        Shoot();
    }

    protected virtual void Shoot()
    {
        _rb.velocity = (_destination - transform.position).normalized * _speed;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")&& !_isTrigerred)
        {
            _isTrigerred = true;
            EnemyScript enemyScript = other.GetComponent<EnemyScript>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(_damage);
            }
            Destroy(gameObject);
        }
        
    }
    
    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
