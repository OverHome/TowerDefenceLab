
using System.Collections;
using UnityEngine;

public class LizardScript: EnemyScript
{
    protected override void Start()
    {
        base.Start();
        _isFireResist = true;
    }

    protected override void SetupAnimator()
    {
        animator.SetInteger("moving", 1);
        _animSpeedRatio = 1f;
        animator.speed = _agent.speed*_animSpeedRatio;
    }
    protected override IEnumerator HandleEnemyDeath()
    {
        EnemyManager.Instance.UnregisterEnemy(this);
        _agent.enabled = false;
        GameManager.Instance.AddCoins(coinValue);
        GameManager.Instance.AddKill();
        animator.Play("die");
        animator.speed = 2;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        
        Destroy(gameObject);
    }
    
    
    protected override IEnumerator PlayHitAnimation()
    {
        // _agent.isStopped = true;
        // animator.Play("hit");
        //
        // yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length/2);
        //
        // _agent.isStopped = false;
        yield break;
    }
}
