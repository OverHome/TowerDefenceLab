using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TurretUIScript : MonoBehaviour
{
    [SerializeField] private TurretScript turret;
    [SerializeField] private TextMeshProUGUI levelUI;
    [SerializeField] private Button updateButton;
    private Camera _mainCamera;
    private Canvas _canvas;

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

    public void SetLevelUI()
    {
        levelUI.text = turret.TurretLevel.ToString();
        if (turret.TurretMaxLevel == turret.TurretLevel)
        {
            updateButton.gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        _canvas.transform.LookAt(_canvas.transform.position + _mainCamera.transform.rotation * Vector3.forward,
            _mainCamera.transform.rotation * Vector3.up);
    }

}
