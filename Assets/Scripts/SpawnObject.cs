using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class SpawnObject : MonoBehaviour
{
    [SerializeField] public GameObject spawnObject;


    private ARRaycastManager _raycastManager;
    private ARPlaneManager _planeManager;
    private bool _isSpawned;

    private void Start()
    {
        _raycastManager = GetComponent<ARRaycastManager>();
        _planeManager = GetComponent<ARPlaneManager>();
        if (LevelManager.Instance?.GetLevelPrefab() != null)
        {
            spawnObject = LevelManager.Instance.GetLevelPrefab();
        }
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += finger => { Spawn(finger.currentTouch.screenPosition); };

        // GetComponent<ARPlaneManager>().enabled = !Settings.Instance.UseDepth;
    }

    private void Spawn(Vector2 pos)
    {
        if (!_isSpawned && !IsTouchUI(pos))
        {
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            _raycastManager.Raycast(pos, hits, TrackableType.Planes);
            
            if (hits.Count == 0)
            {
                print("nope");
                return;
            }
            var spawnedObject = Instantiate(spawnObject, hits[0].pose.position + new Vector3(0, 0.5f, 0), new Quaternion());
            RotateToCamera(spawnedObject);
            _isSpawned = true;
            foreach (var plane in _planeManager.trackables)
            {
                plane.gameObject.SetActive(false);
            }
            print("yes");
            _planeManager.enabled = false;
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

    private bool IsTouchUI(Vector2 pos)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = pos
        };
        List<RaycastResult> results = new ();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count != 0;
    }
}