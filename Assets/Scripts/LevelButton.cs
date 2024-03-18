using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private int levelId;
  
    private Image _buttonImage;
    private Button _button;
    private TextMeshProUGUI _buttonText;
    private void Start()
    {
        _button = GetComponent<Button>();
        _buttonImage = GetComponent<Image>();
        _buttonText = GetComponentInChildren<TextMeshProUGUI>();
        _button.interactable = LevelManager.Instance.IsOpenLevel(levelId);
        _buttonText.text = levelId.ToString();
        _button.onClick.AddListener(LoadLevel);
    }

    private void LoadLevel()
    {
        LevelManager.Instance.SetLevel(levelId);
        SceneManager.LoadScene(1);
        
    }
    
}
