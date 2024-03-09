using UnityEngine;
using UnityEngine.Events;

public class InteractiveObject : MonoBehaviour
{
    public UnityEvent OnObjectPressed { get; } = new();

    public void PressObject()
    {
        OnObjectPressed.Invoke();
    }
}