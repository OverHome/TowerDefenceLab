using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private UpgradeInfo upgrade;

    private Button _button;
    private TextMeshProUGUI _buttonText;
    private Image _buttonImage;
    
    private void Start()
    {
        _button = GetComponent<Button>();
        _buttonImage = GetComponent<Image>();
        _buttonText = GetComponentInChildren<TextMeshProUGUI>();

        ChangeEnabled();
        PlayerProgress.Instance.OnNewUpgrade.AddListener( ChangeEnabled);
        _button.onClick.AddListener(BuyUpgrade);
    }

    private void BuyUpgrade()
    {
        if (PlayerProgress.Instance.GetCrystalCount() >= upgrade.CrystalCost)
        {
            PlayerProgress.Instance.OpenUpgrade(upgrade.UpgradeName, upgrade.CrystalCost);
        }
    }

    private void ChangeEnabled()
    {
        
        if (upgrade.UpgradeNameNeeded == "" || PlayerProgress.Instance.IsOpenUpgrade(upgrade.UpgradeNameNeeded))
        {
            SetVisible();
        }
        else
        {
            SetDeVisible();
        }
        
        if (PlayerProgress.Instance.IsOpenUpgrade(upgrade.UpgradeName))
        {
            SetOpened();
        }
    }

    private void SetVisible()
    {
        _button.enabled = true;
        _button.interactable = true;
        _buttonImage.sprite = upgrade.UpgradeSprite;
        _buttonText.text = upgrade.CrystalCost.ToString();
        
    }
    
    private void SetDeVisible()
    {
        _button.enabled = false;
        _button.interactable = true;
        _buttonImage.sprite = upgrade.HideSprite;
        _buttonText.text = "";
    }
    
    private void SetOpened()
    {
        _button.enabled = true;
        _button.interactable = false;
        _buttonImage.sprite = upgrade.UpgradeSprite;
        _buttonText.text = "";
    }
}
