using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SpecialSkills : MonoBehaviour
{
    [SerializeField] private Sprite unActivePanelImage;
    [SerializeField] private Sprite activePanelImage;
    [SerializeField] private GameObject skillsPanel;
    [SerializeField] private Image showButtonImage;
    [SerializeField] private GameObject firePanel;


    public UnityEvent FireEvent;
    public UnityEvent CanselEvent;
    public UnityEvent<float> EditEvent;

    public void OpenSkillPanel()
    {
        skillsPanel.SetActive(!skillsPanel.activeSelf);
        showButtonImage.sprite = skillsPanel.activeSelf? activePanelImage: unActivePanelImage;
    }

    public void Fire()
    {
        FireEvent.Invoke();
        firePanel.SetActive(false);
    }
    public void Cansel()
    {
        CanselEvent.Invoke();
        firePanel.SetActive(false);
    } 
    public void Edit(float value)
    {
        EditEvent.Invoke(value);
    }

}
