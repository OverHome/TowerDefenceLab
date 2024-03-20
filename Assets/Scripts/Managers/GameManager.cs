using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _totalCoinsText;
    [SerializeField] private TextMeshProUGUI _totalСrystalText;

    public static GameManager Instance;

    public UnityEvent OnCoinCountEdit;
    public int TotalCoins;
    public int TotalKills;
    public UnityEvent OnEnd;
    public UnityEvent OnCrystalCountEdit;
    public bool IsGameStop;
    public bool IsWin;
    public int TowerHealth;
    public int StartTowerHealth;
    public int TempСrystalCount;
   

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
            if (PlayerProgress.Instance != null)
            {
                PlayerProgress.Instance.AddCrystal(TempСrystalCount);
            }
        }
    }

    public int GetStars()
    {
        switch (TowerHealth)
        {
            case <= 0:
                return 0;
            case var health when health < StartTowerHealth / 2:
                return 1;
            case var health when health < StartTowerHealth:
                return 2;
            default:
                return 3;
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

    public void AddСrystal(int count)
    {
        if(count < 0) return;
        TempСrystalCount += count;
        _totalСrystalText.text = TempСrystalCount.ToString();
        OnCrystalCountEdit.Invoke();
    }
    
    public void SpendСrystal(int count)
    {
        if(count < 0) return;
        TempСrystalCount -= count;
        _totalСrystalText.text = TempСrystalCount.ToString();
        OnCrystalCountEdit.Invoke();
    }
}