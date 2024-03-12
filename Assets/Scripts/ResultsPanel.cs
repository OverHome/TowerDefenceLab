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
        PlayerManager.Instance.OnStop.AddListener(ShowResult);
    }
    
    private void OnDisable()
    {
        PlayerManager.Instance.OnStop.RemoveListener(ShowResult);
    }

    private void ShowResult()
    {
        panelUI.SetActive(true);
        countUI.text = PlayerManager.Instance.TotalKills.ToString();
    }

    public void GoInMenu()
    {
        SceneManager.LoadScene(0);
    }
}
