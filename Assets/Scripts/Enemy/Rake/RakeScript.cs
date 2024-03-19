
using System.Collections;
using UnityEngine;

public class RakeScript: EnemyScript
{
    
    protected override void SetupAnimator()
    {
        animator.SetInteger("moving", 1);
        _animSpeedRatio = 0.75f;
        animator.speed = _agent.speed*_animSpeedRatio;
    }
    protected override IEnumerator HandleEnemyDeath()
    {
        EnemyManager.Instance.UnregisterEnemy(this);
        _agent.enabled = false;
        GameManager.Instance.AddCoins(coinValue);
        GameManager.Instance.AddKill();
        animator.Play("die");
       
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
