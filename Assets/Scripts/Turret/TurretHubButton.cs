using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class TurretHubButton : MonoBehaviour
{
    [SerializeField] private TurretInfo turretInfo;
    [SerializeField] private Image imageUI;
    [SerializeField] private TextMeshProUGUI priceUI;

    private Button _button;
    private Image _buttonImage;
    private Color _defaultColor;

    private void OnEnable()
    {
        _button = GetComponent<Button>();
        _buttonImage = GetComponent<Image>();
        
    }

    private void Start()
    {
        imageUI.sprite = turretInfo.Image;
        _defaultColor = _buttonImage.color;
        priceUI.text = turretInfo.BuyPrice.ToString();
        
        _button.onClick.AddListener(SelectTurret);
        InputManager.Instance.OnChangeSelect.AddListener(ChangeVisibility);
        GameManager.Instance.OnCoinCountEdit.AddListener(SetActiveButton);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(SelectTurret);
    }

    private void ChangeVisibility(int id)
    {
        if (id != turretInfo.Id)
        {
            _buttonImage.color = _defaultColor;
        }
        else
        {
            _buttonImage.color = Color.yellow;
        }
    }

    private void SetActiveButton()
    {
        bool canAfford = GameManager.Instance.TotalCoins >= turretInfo.BuyPrice;
        _button.enabled = canAfford;
        imageUI.color = !canAfford ? new Color(1, 1, 1, 0.3f) : Color.white;
    }

    private void SelectTurret()
    {
        InputManager.Instance.SelectTurretId(InputManager.Instance.GetTurretId() == turretInfo.Id ? -1 : turretInfo.Id);
    }
}
