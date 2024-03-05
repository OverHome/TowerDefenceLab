using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] private GameObject spawnObject;

    private ARRaycastManager _raycastManager;
    private ARPlaneManager _planeManager;

    private void Start()
    {
        _raycastManager = GetComponent<ARRaycastManager>();
        _planeManager = GetComponent<ARPlaneManager>();
    }

    private void OnEnable()
    {
        EnhancedTouch.Touch.onFingerDown += finger => {Spawn(finger.currentTouch.screenPosition);};
        TouchSimulation.Enable(); 
        EnhancedTouchSupport.Enable();
    }

    private void Spawn(Vector2 pos)
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        _raycastManager.Raycast(pos, hits, TrackableType.Planes);
        Instantiate(spawnObject, hits[0].pose.position, new Quaternion());
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Spawn(Input.mousePosition);
        }
    }
}
