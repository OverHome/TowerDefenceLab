using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// using Input = UnityEngine.Windows.Input;


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

        float currentRotation = transform.eulerAngles.x;
        float normalizedRotation = Mathf.Clamp(currentRotation, 0f, 90f);
        float scaleValue = Mathf.Lerp(2f, 1f, normalizedRotation / 90f);
        transform.localScale = new Vector3(1, scaleValue, 1);
    }

}
