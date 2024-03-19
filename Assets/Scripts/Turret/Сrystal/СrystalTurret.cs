
using UnityEngine;

public class CristalTurret: BaseTurret
{
    [SerializeField] private int cristalCount;
    
    
    protected override void FixedUpdate()
    {
        if (Time.time >= _nextFireTime && !GameManager.Instance.IsGameStop)
        {
            _nextFireTime = Time.time + 1.0f / (turretInfo.FireRate + _fireRateBoost*TurretLevel);
            AddCristal();
        }
    }

    private void AddCristal()
    {
        GameManager.Instance.AddСrystal(cristalCount*TurretLevel);
    }
}
