using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurretUIScript : MonoBehaviour
{
    private Camera _mainCamera;
    private Canvas _canvas;
    private TurretScript _turret;

    private void Start()
    {
        _mainCamera = Camera.main;
        _canvas = GetComponent<Canvas>();
        _turret = transform.parent.GetComponent<TurretScript>();
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
        if (results.Count == 0 || (results.Count > 0 && results[^1].gameObject != transform.GetChild(0).gameObject))
        {
            gameObject.SetActive(false);
        }
       
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + _mainCamera.transform.rotation * Vector3.forward,
            _mainCamera.transform.rotation * Vector3.up);
    }

}
