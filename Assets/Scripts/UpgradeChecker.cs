using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UpgradeChecker : MonoBehaviour
{
    [SerializeField] private UpgradeInfo upgradeInfo;
    [SerializeField] private Button blockButton;
    [SerializeField] private MonoBehaviour blockScript;


    private TextMeshProUGUI _blockText;
    private Image _blockImage;
    private Image[] _images;
    private void Start()
    {
        _blockImage = blockButton.GetComponent<Image>();   
        _blockText = blockButton.GetComponentInChildren<TextMeshProUGUI>();   
        _images = blockButton.GetComponentsInChildren<Image>();   
        if (PlayerProgress.Instance != null)
        {
            if (!PlayerProgress.Instance.IsOpenUpgrade(upgradeInfo.UpgradeName))
            {
                if (blockScript != null) blockScript.enabled = false;
                foreach (var image in _images)
                {
                    image.enabled = false;
                }
                blockButton.interactable = true;
                blockButton.enabled = false;
                _blockText.enabled = false;
                _blockImage.enabled = true;
                _blockImage.sprite = upgradeInfo.HideSprite;
                _blockImage.color = Color.white;
               
            }
        }
    }
}
