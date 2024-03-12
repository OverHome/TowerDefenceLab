using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private Image hpUIBar;
    [SerializeField] public float startHealth = 100;
    [SerializeField] private int coinValue = 5;
    public static Transform TowerPos;
    private NavMeshAgent _agent;
    private float _health = 100f;
    

    void Start()
    {
        _agent = GetComponent <NavMeshAgent>();
        
        _health = startHealth;
        
        _agent.SetDestination(TowerPos.position);
    }
    
    public void TakeDamage(float damage)
    {
        _health -= damage;
        hpUIBar.fillAmount = _health / startHealth;
        if (_health <= 0)
        {
            PlayerManager.Instance.AddCoins(coinValue);
            PlayerManager.Instance.AddKill();
            Destroy(gameObject);
        }
    }

}
