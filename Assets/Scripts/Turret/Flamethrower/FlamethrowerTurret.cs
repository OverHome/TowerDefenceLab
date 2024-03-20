using System;
using UnityEngine;

public class FlamethrowerTurret : BaseTurret
{
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private int burnTime = 2;

    private void Start()
    {
        _fireAngel = 15f;
        _damageBoost = 1f;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (IsTargetInShootAngle())
        {
            particleSystem.gameObject.SetActive(true);
            if(!audioSource.isPlaying) audioSource.Play();
        }
        else
        {
            particleSystem.gameObject.SetActive(false);
            audioSource.Stop();
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
                if (enemyScript != null && !enemyScript.GetFireResist())
                {
                    enemyScript.TakeDamage(turretInfo.BaseDamage+_damageBoost*TurretLevel, false);
                    FlamethrowerBullet flameEffect = collider.GetComponentInChildren<FlamethrowerBullet>();
                    if (flameEffect == null)
                    {
                        var projectileInstantiate = Instantiate(projectile, enemyScript.transform.position,
                            new Quaternion(), enemyScript.transform);
                        projectileInstantiate.GetComponent<FlamethrowerBullet>()
                            .Initialized(enemyScript, turretInfo.BaseDamage+_damageBoost*TurretLevel, burnTime);
                    }
                    else
                    {
                        flameEffect.UpdateTime(burnTime);
                    }
                }
            }
        }
    }
}