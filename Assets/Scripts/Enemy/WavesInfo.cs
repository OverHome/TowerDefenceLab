using UnityEngine;

[CreateAssetMenu(fileName = "Level 1", menuName = "WaveInfo", order = 0)]
public class WavesInfo : ScriptableObject
{ 
    public Wave[] waves;
}
