using System.Collections;
using UnityEngine;

public class SniperTurret : BaseTurret
{
    [SerializeField] private Transform arrowPos;
    [SerializeField] private AudioSource audioSourceFire;
    [SerializeField] private AudioSource audioSourcePrepare;
    
    protected override void FireProjectile()
    {
        if(audioSourcePrepare != null)  audioSourcePrepare.Play();
        GameObject projectileInstance = Instantiate(projectile,arrowPos.position, transform.rotation);
        StartCoroutine(Fire(projectileInstance));
    }

    private IEnumerator Fire(GameObject projectileInstance)
    {
        yield return new WaitForSeconds(0.5f);
        if(audioSourceFire != null) audioSourceFire.Play();
        projectileInstance.GetComponent<BaseBullet>().Initialized(_target.position, turretInfo.BaseDamage*TurretLevel, turretInfo.BulletSpeed);
    }
    
    protected override void UpdateTarget()
    {
        GameObject nearestEnemy = EnemyManager.Instance.FindBiggestEnemy(transform.position, turretInfo.Range);
        Transform newTarget = nearestEnemy?.transform;

        if (_target != newTarget)
        {
            _currentDelay = turretInfo.RotationDelay;
            _target = newTarget;
        }
    }
}
