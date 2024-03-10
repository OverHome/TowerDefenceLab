using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraZoom : MonoBehaviour
{
    public GameObject GameObject;
    public Slider zoomSlider;
    public float minFOV = 20f;
    public float maxFOV = 60f;

    void Start()
    {
        // Назначьте функцию обратного вызова для слайдера
        zoomSlider.onValueChanged.AddListener(ChangeFOV);
    }

    void ChangeFOV(float value)
    {
        // Изменение значения поля зрения с учетом ограничений
        float newFOV = Mathf.Lerp(minFOV, maxFOV, value);
        GameObject.transform.localScale = new Vector3(newFOV, newFOV, newFOV);
    }
}
