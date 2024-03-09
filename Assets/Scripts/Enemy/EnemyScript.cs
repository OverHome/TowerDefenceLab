using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public int coinValue = 5;
    public static Transform TowerPos;
    private NavMeshAgent _agent;

    void Start()
    {
        _agent = GetComponent <NavMeshAgent>();
        _agent.SetDestination(TowerPos.position);
    }
    
    private void OnDestroy()
    {
        PlayerManager.Instance.AddCoins(coinValue);
    }
}
