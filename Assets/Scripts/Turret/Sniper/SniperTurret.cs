using System.Collections;
using UnityEngine;

public class SniperTurret : BaseTurret
{
    [SerializeField] private Transform arrowPos;
    
    protected override void FireProjectile()
    {
        GameObject projectileInstance = Instantiate(projectile,arrowPos.position, transform.rotation);

        StartCoroutine(Fire(projectileInstance));
    }

    private IEnumerator Fire(GameObject projectileInstance)
    {
        yield return new WaitForSeconds(0.5f);
        projectileInstance.GetComponent<BaseBullet>().Initialized(_target.position, turretInfo.BaseDamage, turretInfo.BulletSpeed);
    }
}
