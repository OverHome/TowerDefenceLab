using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static Settings Instance { get; protected set; }

    public bool UseDepth { get; set; } = true;
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
