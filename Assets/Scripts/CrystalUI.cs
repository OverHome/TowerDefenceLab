using System;
using TMPro;
using UnityEngine;

public class CrystalUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI crystalCount;

    private void Start()
    {
        Invoke(nameof(Initialize), 0.5f); // гавно решение но пока так ;(
    }

    private void Initialize()
    {
        SetCrystalCountUI(PlayerProgress.Instance.GetCrystalCount());
        PlayerProgress.Instance.OnCrystalCountChange.AddListener(SetCrystalCountUI);
    }

private void SetCrystalCountUI(int cout)
    {
        crystalCount.text = cout.ToString();
    }
}
