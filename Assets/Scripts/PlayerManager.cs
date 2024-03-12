using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _totalCoinsText;

    public static PlayerManager Instance;

    public int TotalCoins;
    public int TotalKills;
    public UnityEvent OnStop;
    public bool IsGameStop;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        TotalCoins = 50;
        UpdateTotalCoinsText();
    }

    public void AddCoins(int amount)
    {
        TotalCoins += amount;
        UpdateTotalCoinsText();
    }
    
    public void AddKill()
    {
        TotalKills++;
    }

    public void StopGame()
    {
        OnStop.Invoke();
        IsGameStop = true;
        Time.timeScale = IsGameStop ? 0 : 1;

    }

    private void UpdateTotalCoinsText()
    {
        _totalCoinsText.text = TotalCoins.ToString();
    }
}