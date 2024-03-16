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
            if(InputManager.Instance.GetTurretId() != -1)
                EnableTurret();
        }
        else
        {
            turretUI.SetActive(true);
        }
    }

    private void EnableTurret()
    {
        BaseTurret turret = turrets[InputManager.Instance.GetTurretId()];
        InputManager.Instance.SelectTurretId(-1);
        if (PlayerManager.Instance.TotalCoins < turret.turretInfo.BuyPrice) return;
        PlayerManager.Instance.SpendCoins(turret.turretInfo.BuyPrice);
        _isActiveTurret = true;
        _selectTurret = turret;
        turret.gameObject.SetActive(_isActiveTurret);
    }

    public void UpgradeTurret()
    {
        if (PlayerManager.Instance.TotalCoins < _selectTurret.turretInfo.UpgradePrice ||
            _selectTurret.TurretLevel == _selectTurret.TurretMaxLevel) return;
        PlayerManager.Instance.SpendCoins( _selectTurret.turretInfo.UpgradePrice);
    }

    public void SellTurret()
    {
        _isActiveTurret = false;
        _selectTurret.SellTurret();
        _selectTurret.gameObject.SetActive(_isActiveTurret);
    }
}