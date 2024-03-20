using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TurretUIScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelUI;
    [SerializeField] private Button updateButton;
    [SerializeField] private GameObject sphereRange;
    [SerializeField] private TextMeshProUGUI upgradePriceText;
    [SerializeField] private TextMeshProUGUI sellPriceText;
    private Camera _mainCamera;
    private Canvas _canvas;

    private Vector3 _defScale;
    private void Start()
    {
        _mainCamera = Camera.main;
        _canvas = GetComponentInChildren<Canvas>();
        _canvas.worldCamera = _mainCamera;
        InputManager.Instance.OnScreenTap.AddListener(HideUI);
    }

    private void HideUI(Vector2 pos)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = pos
        };
        List<RaycastResult> results = new ();
        EventSystem.current.RaycastAll(eventData, results);
        if (results.Count == 0 || (results.Count > 0 && results[^1].gameObject != transform.GetChild(0).GetChild(0).gameObject))
        {
            gameObject.SetActive(false);
        }
    }

    public void SetLevelUI(int level, int maxLevel)
    {
        levelUI.text = level.ToString();
        if (maxLevel == level)
        {
            updateButton.gameObject.SetActive(false);
        }
    }

    public void SetRangeUI(float range)
    {
        _defScale = sphereRange.transform.localScale;
        sphereRange.transform.localScale = new Vector3(range, 1, range)*sphereRange.transform.localScale.x;
    }
    
    public void DefRangeUI()
    {
        sphereRange.transform.localScale = _defScale;
    }

    private void LateUpdate()
    {
        _canvas.transform.LookAt(_canvas.transform.position + _mainCamera.transform.rotation * Vector3.forward,
            _mainCamera.transform.rotation * Vector3.up);
    }

    public void SetPrices(int upgradePrice, int sellPrice)
    {
        upgradePriceText.text = upgradePrice.ToString();
        sellPriceText.text = sellPrice.ToString();
    }
}
