using UnityEngine;


public class BaseTurret : MonoBehaviour
{
    [SerializeField] protected float fireRate = 1.0f;
    [SerializeField] protected float range = 10.0f;
    [SerializeField] protected float baseDamage = 50.0f;
    [SerializeField] protected float bulletSpeed = 10.0f;
    [SerializeField] protected float rotationDelay = 2.0f;
    [SerializeField] protected GameObject projectile;

    public int BuyPrice = 50;
    public int UpgradePrice = 20;
    public int TurretLevel = 1;
    public int TurretMaxLevel = 3;

    protected Transform _target;
    private float _targetUpdateInterval = 0.1f;
    private float _rotationDelay = 2.0f;
    private float _rotationSpeed = 5f;
    private float _nextFireTime;
    private float _currentDelay;
    
    private void UpdateTarget()
    {
        GameObject nearestEnemy = EnemyManager.Instance.FindNearestEnemy(transform.position, range);
        Transform newTarget = nearestEnemy?.transform;

        if (_target != newTarget)
        {
            _currentDelay = rotationDelay;
            _target = newTarget;
        }
    }

    private void FixedUpdate()
    {
        UpdateTarget();
        LookAtTarget();

        if (Time.time >= _nextFireTime && _target != null && IsTargetInShootAngle())
        {
            _nextFireTime = Time.time + 1.0f / fireRate;
            FireProjectile();
        }
    }

    private void LookAtTarget()
    {
        if (_target != null)
        {
            if (_currentDelay > 0.0f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_target.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                    (_rotationSpeed / _rotationDelay) * Time.deltaTime);
                _currentDelay -= Time.deltaTime;
            }
            else
            {
                Quaternion targetRotation = Quaternion.LookRotation(_target.position - transform.position);
                transform.rotation =
                    Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            }
        }
    }

    private bool IsTargetInShootAngle()
    {
        if (_target != null)
        {
            Vector3 targetDirection = (_target.position - transform.position).normalized;
            float angle = Vector3.Angle(targetDirection, transform.forward);
            return angle <= 5f;
        }

        return false;
    }

    protected virtual void FireProjectile()
    {
        GameObject projectileInstance = Instantiate(projectile, transform.position, transform.rotation);
        projectileInstance.GetComponent<BaseBullet>().Initialized(_target.position, baseDamage, bulletSpeed);
    }

    public virtual void UpgradeTurret()
    {
        TurretLevel++;
        _nextFireTime = Time.time;
        print("Турель уровня: " + TurretLevel);
    }

    public virtual void SellTurret()
    {
        PlayerManager.Instance.AddCoins(BuyPrice / 2 + (TurretLevel - 1) * (UpgradePrice / 2));
        TurretLevel = 1;
        print("Турель продана");
    }
}