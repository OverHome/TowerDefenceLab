using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TowerScript : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        print(other.gameObject);
        if (other.gameObject.CompareTag("Enemy"))
        {
            PlayerManager.Instance.StopGame();
        }
    }
    
}
