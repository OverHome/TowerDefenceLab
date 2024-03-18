using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Image hpUIBar;
    [SerializeField] public float StartHealth = 100;
    [SerializeField] private int coinValue = 5;

    public static Transform TowerPos;

    private NavMeshAgent _agent;
    private float _health;
    private Vector3 _lastMoveDirection;
    private bool _isDie;

    private void Start()
    {
        InitializeEnemy();
    }

    private void InitializeEnemy()
    {
        _agent = GetComponent<NavMeshAgent>();
        _health = StartHealth;

        SetAgentDestination();
        SetupAnimator();
    }

    private void SetAgentDestination()
    {
        _agent.SetDestination(TowerPos.position);
    }

    private void SetupAnimator()
    {
        animator.SetInteger("moving", 1);
        animator.speed = _agent.speed * 2;
    }

    private void FixedUpdate()
    {
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        Vector3 currentMoveDirection = _agent.velocity.normalized;

        if (currentMoveDirection != _lastMoveDirection)
        {
            RotateTowards(currentMoveDirection);
            _lastMoveDirection = currentMoveDirection;
        }
    }

    private void RotateTowards(Vector3 targetDirection)
    {
        Quaternion lookRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }

    public void TakeDamage(float damage, bool playHitAnim = true)
    {
        if(_isDie) return;
        _health -= damage;
        hpUIBar.fillAmount = _health / StartHealth;

        if (_health <= 0)
        {
            _isDie = true;
            StartCoroutine(HandleEnemyDeath());
        }
        else
        {
            if(playHitAnim) StartCoroutine(PlayHitAnimation());
        }
    }


    private IEnumerator HandleEnemyDeath()
    {
        EnemyManager.Instance.UnregisterEnemy(this);
        _agent.enabled = false;
        GameManager.Instance.AddCoins(coinValue);
        GameManager.Instance.AddKill();
        animator.Play(Random.Range(0, 2) == 0 ? "death1" : "death2");
        
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        
        Destroy(gameObject);
    }
    

    private IEnumerator PlayHitAnimation()
    {
        _agent.isStopped = true;
        animator.Play(Random.Range(0, 2) == 0 ? "hit1" : "hit2");

        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length / 2);

        _agent.isStopped = false;
    }
    private void OnEnable()
    {
        EnemyManager.Instance.RegisterEnemy(this);
    }
    private void OnDisable()
    {
        EnemyManager.Instance.UnregisterEnemy(this);
    }
}