using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultsPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countUI;
    [SerializeField] private GameObject panelUI;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Image[] stars;
    [SerializeField] private AudioSource audioSourceWin;
    [SerializeField] private AudioSource audioSourceLose;

    private void Start()
    {
        GameManager.Instance.OnEnd.AddListener(ShowResult);
    }
    
    private void OnDestroy()
    {
        GameManager.Instance.OnEnd.RemoveListener(ShowResult);
    }

    private void ShowResult()
    {
        panelUI.SetActive(true);
        if (GameManager.Instance.IsWin)
        {
            countUI.text = GameManager.Instance.TempСrystalCount.ToString();
            audioSourceWin.Play();
        }
        else
        {
            countUI.text = "ТЫ ПРОИГРАЛ\nНИКАКИХ КРИСТАЛЛОВ";
            countUI.transform.parent.GetComponent<TextMeshProUGUI>().enabled = false;
            audioSourceLose.Play();
        }
        
        for (int i = 0; i < GameManager.Instance.GetStars(); i++)
        {
            stars[i].color = Color.white;
        }
        nextLevelButton.gameObject.SetActive(LevelManager.Instance.IsOpenLevel(LevelManager.Instance.GetLevel()+1));
    }

    public void GoInMenu()
    {
        GameManager.Instance.StopGame(false);
        SceneManager.LoadScene(0);
    }
    
    public void ReloadLevel()
    {
        GameManager.Instance.StopGame(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void LoadNextLevel()
    {
        GameManager.Instance.StopGame(false);
        LevelManager.Instance.SetLevel(LevelManager.Instance.GetLevel()+1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
