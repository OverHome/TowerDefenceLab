using UnityEngine;

public class EnemyUIScript : MonoBehaviour
{
    private Camera _mainCamera;
    
    private void Start()
    {
        _mainCamera = Camera.main;
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
