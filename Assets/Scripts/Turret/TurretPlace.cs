using UnityEngine;


public class TurretPlace : MonoBehaviour
{
    [SerializeField] private GameObject turret;
    private bool _isActiveTurret;
        
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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
}
