using UnityEngine;

public class BombardmentTurret : BaseTurret
{
    [SerializeField] private Transform bombPos;
    [SerializeField] private float damageRadius;
    
    protected override void FireProjectile()
    {
        GameObject projectileInstance = Instantiate(projectile, bombPos.position, transform.rotation);
        projectileInstance.GetComponent<BombardmentBullet>().Initialized(_target.position, turretInfo.BaseDamage, damageRadius);
    }
}
