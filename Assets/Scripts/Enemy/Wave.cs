using UnityEngine;
using UnityEngine.Internal;

[System.Serializable]
public class Wave
{
    public GameObject Enemy;
    [Min(1)] public int Count;
    public float Rate = 1;
    public bool IsWait = true;
    public float WaitTime = 1;
    public float HealthMultiplier = 1;
}