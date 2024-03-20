using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractiveObject))]
public class TurretPlace : MonoBehaviour
{
    [SerializeField] private TurretUIScript turretUI;
    [SerializeField] private List<BaseTurret> turrets;
    [SerializeField] private bool isBoosted;
    [SerializeField] private GameObject place;
    [SerializeField] private GameObject placeBoosted;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip destroy;
    [SerializeField] private AudioClip levelUp;
    [SerializeField] private AudioClip build;

    private InteractiveObject _interactiveObject;
    private BaseTurret _selectTurret;
    private bool _isActiveTurret;
    private int _levelBoost = 2;

    private void OnEnable()
    {
        _interactiveObject = GetComponent<InteractiveObject>();
        _interactiveObject.OnObjectPressed.AddListener(ClickProcessing);
    }

    private void Start()
    {
        place.SetActive(!isBoosted);
        placeBoosted.SetActive(isBoosted);
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
            turretUI.SetPrices(_selectTurret.turretInfo.UpgradePrice, GetSellPrice());
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
        if (isBoosted) _selectTurret.TurretMaxLevel += _levelBoost;
        audioSource.PlayOneShot(build);
    }

    public void UpgradeTurret()
    {
        if (GameManager.Instance.TotalCoins < _selectTurret.turretInfo.UpgradePrice ||
            _selectTurret.TurretLevel == _selectTurret.TurretMaxLevel) return;
        GameManager.Instance.SpendCoins(_selectTurret.turretInfo.UpgradePrice);
        _selectTurret.UpgradeTurret();
        turretUI.SetLevelUI(_selectTurret.TurretLevel, _selectTurret.TurretMaxLevel);
        turretUI.SetPrices(_selectTurret.turretInfo.UpgradePrice, GetSellPrice());
        audioSource.PlayOneShot(levelUp);
        
    }

    public void SellTurret()
    {
        GameManager.Instance.AddCoins(GetSellPrice());
        _isActiveTurret = false;
        _selectTurret.SellTurret();
        if (isBoosted) _selectTurret.TurretMaxLevel -= _levelBoost;
        _selectTurret.gameObject.SetActive(_isActiveTurret);
        turretUI.DefRangeUI();
        turretUI.gameObject.SetActive(false);
        audioSource.PlayOneShot(destroy);
    }

    private int GetSellPrice()
    {
        return _selectTurret.turretInfo.BuyPrice / 2 +
               (_selectTurret.TurretLevel - 1) * (_selectTurret.turretInfo.UpgradePrice / 2);
    }
}