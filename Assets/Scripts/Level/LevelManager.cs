using System;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;


public class LevelManager : MonoBehaviour
{
    [SerializeField] private LevelInfo[] levels;
    public static LevelManager Instance { get; protected set; }

    private bool[] _completedLevels;
    private int _levelId;
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
            LoadLevels();
            SaveLevels();
        }
    }
    
    public bool IsOpenLevel(int id)
    {
        return id <= levels.Length && levels[id].IsOpen;
    }

    public void SetLevel(int levelId)
    {
        _levelId = levelId;
    }

    public GameObject GetLevelPrefab()
    {
        return levels[_levelId].LevelPrefab;
    }
    
    public WavesInfo GetWavesInfo()
    {
        return levels[_levelId].LevelWavesInfo;
    }

    public void OpenNextLevel()
    {
        if (levels.Length > _levelId + 1)
        {
            levels[_levelId+1].IsOpen = true;
            _completedLevels[_levelId + 1] = true;
        }
        SaveLevels();
    }
    
    public void SaveLevels() {
        string jsonData = JsonUtility.ToJson(new SaveLevelData(){CompletedLevels = _completedLevels});
        File.WriteAllText(Application.persistentDataPath + "/levels.json", jsonData);
    }

    private void LoadLevels() {
        string path = Application.persistentDataPath + "/levels.json";
        if (File.Exists(path)) {
            print("load");
            string jsonData = File.ReadAllText(path);
            _completedLevels = JsonUtility.FromJson<SaveLevelData>(jsonData).CompletedLevels;
            for (int i = 0; i < _completedLevels.Length; i++)
            {
                levels[i].IsOpen = _completedLevels[i];
            }

            if (levels.Length > _completedLevels.Length)
            {
                Array.Resize(ref _completedLevels, levels.Length);
            }
        }
        else
        {
            print("new save");
            _completedLevels = new bool[levels.Length];
            _completedLevels[0] = true;
        }
    }
}
