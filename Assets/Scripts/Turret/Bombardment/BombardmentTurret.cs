using System;
using UnityEngine;

public class BombardmentTurret : BaseTurret
{
    [SerializeField] private AudioSource audioSourceFire;
    [SerializeField] private AudioSource audioSourceBoom;
    [SerializeField] private Transform bombPos;
    [SerializeField] private float damageRadius;

    private float _damageRadiusBoost = 0.3f;


    private void Start()
    {
        _damageBoost = 10f;
    }

    protected override void FireProjectile()
    {
        if(audioSourceFire != null) audioSourceFire.Play();
        GameObject projectileInstance = Instantiate(projectile, bombPos.position, transform.rotation);
        projectileInstance.GetComponent<BombardmentBullet>().Initialized(_target.position,
            turretInfo.BaseDamage + _damageBoost * TurretLevel,
            damageRadius + _damageRadiusBoost * TurretLevel);
        projectileInstance.GetComponent<BombardmentBullet>().OnBoom.AddListener((() => audioSourceBoom.Play()));
    }
}