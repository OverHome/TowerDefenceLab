using System;
using UnityEngine;

[RequireComponent(typeof(InteractiveObject))]
public class TurretPlace : MonoBehaviour
{
    [SerializeField] private GameObject turret;
    
    private bool _isActiveTurret;
    private InteractiveObject _interactiveObject;

    private void OnEnable()
    {
        _interactiveObject = GetComponent<InteractiveObject>();
        _interactiveObject.OnObjectPressed.AddListener(EnableTurret);
    }

    private void OnDisable()
    {
        _interactiveObject.OnObjectPressed.RemoveListener(EnableTurret);
    }

    private void EnableTurret()
    {
        if (!_isActiveTurret)
        {
            _isActiveTurret = true;
            turret.SetActive(_isActiveTurret);
        }
       
    }
}
