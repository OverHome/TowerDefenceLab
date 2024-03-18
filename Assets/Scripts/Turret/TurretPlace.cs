using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractiveObject))]
public class TurretPlace : MonoBehaviour
{
    [SerializeField] private TurretUIScript turretUI;
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
            if (InputManager.Instance.GetTurretId() != -1)
                EnableTurret();
        }
        else
        {
            turretUI.gameObject.SetActive(true);
        }
    }

    private void EnableTurret()
    {
        BaseTurret turret = turrets[InputManager.Instance.GetTurretId()];
        InputManager.Instance.SelectTurretId(-1);
        if (GameManager.Instance.TotalCoins < turret.turretInfo.BuyPrice) return;
        GameManager.Instance.SpendCoins(turret.turretInfo.BuyPrice);
        _isActiveTurret = true;
        _selectTurret = turret;
        turret.gameObject.SetActive(_isActiveTurret);
        turretUI.SetRangeUI(turret.turretInfo.Range);
    }

    public void UpgradeTurret()
    {
        if (GameManager.Instance.TotalCoins < _selectTurret.turretInfo.UpgradePrice ||
            _selectTurret.TurretLevel == _selectTurret.TurretMaxLevel) return;
        GameManager.Instance.SpendCoins(_selectTurret.turretInfo.UpgradePrice);
        _selectTurret.UpgradeTurret();
        turretUI.SetLevelUI(_selectTurret.TurretLevel, _selectTurret.TurretMaxLevel);
    }

    public void SellTurret()
    {
        GameManager.Instance.AddCoins(_selectTurret.turretInfo.BuyPrice / 2 +
                                        (_selectTurret.TurretLevel - 1) * (_selectTurret.turretInfo.UpgradePrice / 2));
        _isActiveTurret = false;
        _selectTurret.SellTurret();
        _selectTurret.gameObject.SetActive(_isActiveTurret);
    }
}