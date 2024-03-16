using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractiveObject))]
public class TurretPlace : MonoBehaviour
{
    [SerializeField] private GameObject turretUI;
    [SerializeField] private List<BaseTurret> turrets;
    
    
    private InteractiveObject _interactiveObject;
    private BaseTurret _selectTurret;
    private bool _isActiveTurret;

    private void OnEnable()
    {
        _interactiveObject = GetComponent<InteractiveObject>();
        _interactiveObject.OnObjectPressed.AddListener(ClickProcessing);
    }

    private void OnDisable()
    {
        _interactiveObject.OnObjectPressed.RemoveListener(ClickProcessing);
    }

    private void ClickProcessing()
    {
        if (!_isActiveTurret)
        {
            EnableTurret();
        }
        else
        {
            turretUI.SetActive(true);
        }
    }

    private void EnableTurret()
    {
        BaseTurret turret = turrets[0];
        if (PlayerManager.Instance.TotalCoins < turret.BuyPrice) return;
        PlayerManager.Instance.SpendCoins(turret.BuyPrice);
        _isActiveTurret = true;
        _selectTurret = turret;
        turret.gameObject.SetActive(_isActiveTurret);
    }

    public void UpgradeTurret()
    {
        if (PlayerManager.Instance.TotalCoins < _selectTurret.UpgradePrice ||
            _selectTurret.TurretLevel == _selectTurret.TurretMaxLevel) return;
        PlayerManager.Instance.SpendCoins( _selectTurret.UpgradePrice);
    }

    public void SellTurret()
    {
        _isActiveTurret = false;
        _selectTurret.SellTurret();
        _selectTurret.gameObject.SetActive(_isActiveTurret);
    }
}