using System;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _totalCoinsText;

    public static PlayerManager Instance;

    public int TotalCoins;

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

    private void UpdateTotalCoinsText()
    {
        _totalCoinsText.text = TotalCoins.ToString();
    }
}