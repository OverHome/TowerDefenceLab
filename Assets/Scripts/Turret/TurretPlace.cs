using UnityEngine;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;


public class TurretPlace : MonoBehaviour
{
    [SerializeField] private GameObject turret;
    private bool _isActiveTurret;
    
    private void OnEnable()
    {
        EnhancedTouch.Touch.onFingerDown += finger => {EnableTurret(Camera.main.ScreenPointToRay(finger.currentTouch.screenPosition)); };
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            EnableTurret(Camera.main.ScreenPointToRay(Input.mousePosition));
           
        }
    }

    private void EnableTurret(Ray ray)
    {
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject == gameObject)
            {
                _isActiveTurret = true;
                turret.SetActive(_isActiveTurret);
            }
        }
    }
}
