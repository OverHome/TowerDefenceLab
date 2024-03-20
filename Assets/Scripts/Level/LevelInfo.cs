using System;
using UnityEngine;

[Serializable]
public class LevelInfo
{
    public GameObject LevelPrefab;
    public WavesInfo LevelWavesInfo;
    public bool IsOpen;
    public int StartCoinCount;
}
