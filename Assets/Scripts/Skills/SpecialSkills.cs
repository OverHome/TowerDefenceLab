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


    public UnityEvent FireEvent;
    public UnityEvent CanselEvent;

    public void OpenSkillPanel()
    {
        skillsPanel.SetActive(!skillsPanel.activeSelf);
        showButtonImage.sprite = skillsPanel.activeSelf? activePanelImage: unActivePanelImage;
    }

    public void Start()
    {
        InputManager.Instance.OnScreenTap.AddListener(Fire);
    }

    private void Fire(Vector2 pos)
    {
        FireEvent.Invoke();
    }
    public void Cansel()
    {
        CanselEvent.Invoke();
    } 

}
