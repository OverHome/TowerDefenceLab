using System;
using UnityEngine;

public class PlayerProgress : MonoBehaviour
{
    public static PlayerProgress Instance { get; protected set; }
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
    
}
