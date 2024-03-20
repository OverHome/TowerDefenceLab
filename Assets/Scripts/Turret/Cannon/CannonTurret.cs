using System;
using UnityEngine;

public class CannonTurret : BaseTurret
{
    [SerializeField] private AudioSource audioSource;
    private void Start()
    {
        _fireRateBoost = 0.2f;
        _damageBoost = 5f;
    }

    protected override void FireProjectile()
    {
        if(audioSource != null) audioSource.Play();
        GameObject projectileInstance = Instantiate(projectile, transform.position, transform.rotation);
        projectileInstance.GetComponent<BaseBullet>().Initialized(_target.position, turretInfo.BaseDamage + _damageBoost*TurretLevel,
            turretInfo.BulletSpeed);
    }
    public override void UpgradeTurret()
    {
        base.UpgradeTurret();
    }

    public override void SellTurret()
    {
        base.SellTurret();
    }
}
