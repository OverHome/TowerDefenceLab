using System;
using UnityEngine;

[RequireComponent(typeof(InteractiveObject))]
public class TurretPlace : MonoBehaviour
{
    [SerializeField] private GameObject turret;
    [SerializeField] private int price;
    
    private bool _isActiveTurret;
    private InteractiveObject _interactiveObject;

    private void OnEnable()
    {
        _interactiveObject = GetComponent<InteractiveObject>();
        _interactiveObject.OnObjectPressed.AddListener(EnableTurret);
    }

    private void OnDisable()
    {
        _interactiveObject.OnObjectPressed.RemoveListener(EnableTurret);
    }

    private void EnableTurret()
    {
        if (_isActiveTurret) return;
        if (PlayerManager.Instance.TotalCoins < price) return;
        PlayerManager.Instance.AddCoins(-price);
        _isActiveTurret = true;
        turret.SetActive(_isActiveTurret);
    }

    public void SellTurret()
    {
        _isActiveTurret = false;
    }
}
