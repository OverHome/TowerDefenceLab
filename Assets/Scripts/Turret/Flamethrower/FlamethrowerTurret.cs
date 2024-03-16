using System;
using UnityEngine;

public class FlamethrowerTurret : BaseTurret
{
    [SerializeField] private ParticleSystem particleSystem;

    private void Start()
    {
        _fireAngel = 10f;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (IsTargetInShootAngle())
        {
            particleSystem.gameObject.SetActive(true);
        }
        else
        {
            particleSystem.gameObject.SetActive(false);
        }
        
    }

    protected override void FireProjectile()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.5f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                EnemyScript enemyScript = collider.GetComponent<EnemyScript>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(turretInfo.BaseDamage, false);
                    FlamethrowerBullet flameEffect = collider.GetComponentInChildren<FlamethrowerBullet>();
                    if (flameEffect == null)
                    {
                        var projectileInstantiate = Instantiate(projectile, enemyScript.transform.position, new Quaternion(), enemyScript.transform);
                        projectileInstantiate.GetComponent<FlamethrowerBullet>().Initialized(enemyScript, turretInfo.BaseDamage, 5f);
                    }
                    else
                    {
                        flameEffect.UpdateTime(5f);
                    }
                }
            }
        }
    }

 
}
