using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _totalCoinsText;

    public static PlayerManager Instance;

    public UnityEvent OnCoinCountEdit;
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
        TotalCoins = 500;
        UpdateTotalCoinsText();
    }

    public void AddCoins(int amount)
    {
        if (amount <= 0) throw new InvalidDataException();
        TotalCoins += amount;
        OnCoinCountEdit.Invoke();
        UpdateTotalCoinsText();
    }
    
    public void SpendCoins(int amount)
    {
        if (amount <= 0) throw new InvalidDataException();
        TotalCoins -= amount;
        OnCoinCountEdit.Invoke();
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