using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countUI;
    [SerializeField] private GameObject panelUI;

    private void Start()
    {
        GameManager.Instance.OnEnd.AddListener(ShowResult);
    }
    
    private void OnDisable()
    {
        GameManager.Instance.OnEnd.RemoveListener(ShowResult);
    }

    private void ShowResult()
    {
        panelUI.SetActive(true);
        countUI.text = GameManager.Instance.TotalKills.ToString();
    }

    public void GoInMenu()
    {
        SceneManager.LoadScene(0);
    }
}
