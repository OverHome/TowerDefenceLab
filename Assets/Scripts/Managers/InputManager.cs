using UnityEngine;
using UnityEngine.Events;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    public UnityEvent<Vector2> OnScreenTap;
    public UnityEvent<int> OnChangeSelect;
    private Camera _arCamera;

    private int _selectedTurret = -1;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        _arCamera = Camera.main;
        EnableTouchInput();
    }

    private void OnDisable()
    {
        DisableTouchInput();
    }

    private void Update()
    {
        CheckMouseInput();
    }

    private void EnableTouchInput()
    {
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += OnFingerDown;
    }

    private void DisableTouchInput()
    {
        EnhancedTouch.Touch.onFingerDown -= OnFingerDown;
        EnhancedTouch.EnhancedTouchSupport.Disable();
    }

    private void OnFingerDown(EnhancedTouch.Finger finger)
    {
        InputProcessing(finger.screenPosition);
    }

    private void CheckMouseInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            InputProcessing(Input.mousePosition);
        }
    }

    private void InputProcessing(Vector2 position)
    {
        OnScreenTap.Invoke(position);
        Ray ray = _arCamera.ScreenPointToRay(position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            
            InteractiveObject interactiveObject = hit.collider.GetComponent<InteractiveObject>();
            if (interactiveObject != null)
            {
                interactiveObject.PressObject();
            }
        }
    }

    public void SelectTurretId(int id)
    {
        OnChangeSelect.Invoke(id);
        _selectedTurret = id;
    }
    
    public int GetTurretId()
    {
        return _selectedTurret;
    }
}