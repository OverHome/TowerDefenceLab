using UnityEngine;

public class SniperBullet : BaseBullet
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && _damage > 0)
        {
            EnemyScript enemyScript = other.GetComponent<EnemyScript>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(_damage);
            }
            Destroy(gameObject);
        }
        
    }
}
