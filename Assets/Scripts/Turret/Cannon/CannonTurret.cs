using System;
using UnityEngine;

public class CannonTurret : BaseTurret
{
    private void Start()
    {
        _fireRateBoost = 0.2f;
        _damageBoost = 10f;
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
