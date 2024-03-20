using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Events;

public class PlayerProgress : MonoBehaviour
{
    public static PlayerProgress Instance { get; protected set; }

    public UnityEvent<int> OnCrystalCountChange;
    public UnityEvent OnNewUpgrade;

    private ProgressData _progressData;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            throw new System.Exception("An instance of this singleton already exists.");
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        LoadProgress();
        SaveProgress();
    }

    public void AddCrystal(int count)
    {
        if (count<0) return; 
        _progressData.CrystalCount += count;
        OnCrystalCountChange.Invoke(_progressData.CrystalCount);
        SaveProgress();
    }
    
    public void SpendCrystal(int count)
    {
        if (count<0) return; 
        _progressData.CrystalCount -= count;
        OnCrystalCountChange.Invoke(_progressData.CrystalCount);
        SaveProgress();
    }

    public int GetCrystalCount()
    {
        return _progressData.CrystalCount;
    }

    public bool IsOpenUpgrade(string upgradeName)
    {
        return  _progressData.Upgrades != null && _progressData.Upgrades.ContainsKey(upgradeName) && _progressData.Upgrades[upgradeName];
    }

    public void OpenUpgrade(string upgradeName, int cost)
    {
        _progressData.Upgrades.Add(upgradeName, true);
        SpendCrystal(cost);
        SaveProgress();
        OnNewUpgrade.Invoke();
    }
    
    private void SaveProgress() {
        string jsonData = JsonConvert.SerializeObject(_progressData);
        File.WriteAllText(Application.persistentDataPath + "/progress.json", jsonData);
    }

    private void LoadProgress() {
        string path = Application.persistentDataPath + "/progress.json";
        if (File.Exists(path)) {
            print("load");
            string jsonData = File.ReadAllText(path);
            _progressData = JsonConvert.DeserializeObject<ProgressData>(jsonData);
            _progressData.Upgrades ??= new Dictionary<string, bool>();
            OnCrystalCountChange.Invoke(_progressData.CrystalCount);
        }
        else
        {
            print("new save");
            _progressData = new ProgressData
            {
                Upgrades = new Dictionary<string, bool>()
            };
        }
    }
}
