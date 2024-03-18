using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _totalCoinsText;

    public static GameManager Instance;

    public UnityEvent OnCoinCountEdit;
    public int TotalCoins;
    public int TotalKills;
    [FormerlySerializedAs("OnStop")] public UnityEvent OnEnd;
    public bool IsGameStop;
    public bool IsWin;
   

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

    public void EndGame(bool isWin)
    {
        OnEnd.Invoke();
        IsGameStop = true;
        Time.timeScale = IsGameStop ? 0 : 1;
        IsWin = isWin;
        if (isWin)
        {
            LevelManager.Instance.OpenNextLevel();
        }
    }
    
    public void StopGame(bool isStop)
    {
        IsGameStop = isStop;
        Time.timeScale = IsGameStop ? 0 : 1;
    }

    private void UpdateTotalCoinsText()
    {
        _totalCoinsText.text = TotalCoins.ToString();
    }
    
}