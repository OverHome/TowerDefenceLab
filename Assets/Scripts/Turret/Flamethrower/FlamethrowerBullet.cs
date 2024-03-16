using System.Collections;
using UnityEngine;

public class FlamethrowerBullet : BaseBullet
{
    private float _flameTime;
    private EnemyScript _enemy;
    
    public void Initialized(EnemyScript enemy, float damage, float flameTime)
    {
        _enemy = enemy;
        _damage = damage;
        _flameTime = flameTime;
        StartCoroutine(Burn());
    }

    private IEnumerator Burn()
    {
        while (_flameTime > 0)
        {
            yield return new WaitForSeconds(1);
            _flameTime --;
            _enemy.TakeDamage(_damage, false);
        }

        Destroy(gameObject);
    }

    public void UpdateTime(float time)
    {
        _flameTime = time;
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }
}
