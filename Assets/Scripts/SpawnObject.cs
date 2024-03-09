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
    private bool _isSpawned;

    private void Start()
    {
        _raycastManager = GetComponent<ARRaycastManager>();
        _planeManager = GetComponent<ARPlaneManager>();
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += finger => { Spawn(finger.currentTouch.screenPosition); };

        // GetComponent<ARPlaneManager>().enabled = !Settings.Instance.UseDepth;
    }

    private void Spawn(Vector2 pos)
    {
        if (!_isSpawned)
        {
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            _raycastManager.Raycast(pos, hits, Settings.Instance?.UseDepth.CompareTo(true) == 1
                ? TrackableType.Depth
                : TrackableType.Planes | TrackableType.FeaturePoint);
            var spawnedObject = Instantiate(spawnObject, hits[0].pose.position + new Vector3(0, 0.5f, 0), new Quaternion());
            RotateToCamera(spawnedObject);
            _isSpawned = true;
            foreach (var plane in _planeManager.trackables)
            {
                plane.gameObject.SetActive(false);
            }
        }
    }

    private void RotateToCamera(GameObject obj)
    {
        Vector3 position = obj.transform.position; 
        Vector3 cameraPosition = Camera.main.transform.position; 
        Vector3 direction = cameraPosition - position; 
        Vector3 targetRotationEuler = Quaternion.LookRotation(forward: direction).eulerAngles;
        Vector3 scaledEuler = Vector3.Scale(targetRotationEuler, obj.transform. up.normalized); // (0, 1, 0)
        Quaternion targetRotation = Quaternion.Euler(scaledEuler);
        obj.transform.rotation *= targetRotation;
        obj.transform.Rotate(0f, 90f, 0f);
    }
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Spawn(Input.mousePosition);
        }
    }
}