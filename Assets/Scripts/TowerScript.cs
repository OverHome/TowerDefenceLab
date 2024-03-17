using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TowerScript : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private int startHealth = 100;

    private float _health;

    private void Start()
    {
        _health = startHealth;
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(other.gameObject);
        if (other.gameObject.CompareTag("Enemy"))
        {
            _health -= 25;
            image.fillAmount = _health / startHealth;
        }

        if (_health <= 0)
        {
            PlayerManager.Instance.StopGame();
        }
    }
    
}
