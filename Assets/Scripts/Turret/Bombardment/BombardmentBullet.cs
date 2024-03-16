using System.Collections;
using UnityEngine;

public class BombardmentBullet : BaseBullet
{
    private float _angle = 75f;
    private float _damageRadius;
    private bool _damageDealt = false;
    protected override void Shoot()
    {
        Vector3 direction = (_destination - transform.position).normalized;
        float targetDistance = Vector3.Distance(transform.position, _destination);
        float projectileVelocity = Mathf.Sqrt(targetDistance * Mathf.Abs(Physics.gravity.y) / Mathf.Sin(2f * _angle * Mathf.Deg2Rad));
        float horizontalVelocity = projectileVelocity * Mathf.Cos(_angle * Mathf.Deg2Rad);
        float verticalVelocity = projectileVelocity * Mathf.Sin(_angle * Mathf.Deg2Rad);
      
        Vector3 velocity = direction * horizontalVelocity + Vector3.up * verticalVelocity;
        
        _rb.velocity = velocity;
    }
    
    public void Initialized(Vector3 destination, float damage, float damageRadius)
    {
        _destination = destination;
        _damage = damage;
        _damageRadius = damageRadius;
        Shoot();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!_damageDealt && other.CompareTag("Flore"))
        {
            _damageDealt = true;
            _rb.useGravity = false;
            _rb.isKinematic = true;
            Collider[] colliders = Physics.OverlapSphere(transform.position, _damageRadius);
            foreach (Collider collider in colliders)
            {
                EnemyScript healthScript = collider.GetComponent<EnemyScript>();

                if (healthScript != null)
                {
                    healthScript.TakeDamage(_damage);
                }
            }
            Destroy(gameObject);
        }
        
    }

}