using UnityEngine;


public class BaseTurret : MonoBehaviour
{
    [SerializeField] public TurretInfo turretInfo;
    [SerializeField] protected GameObject projectile;

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
        GameObject nearestEnemy = EnemyManager.Instance.FindNearestEnemy(transform.position, turretInfo.Range);
        Transform newTarget = nearestEnemy?.transform;

        if (_target != newTarget)
        {
            _currentDelay = turretInfo.RotationDelay;
            _target = newTarget;
        }
    }

    private void FixedUpdate()
    {
        UpdateTarget();
        LookAtTarget();

        if (Time.time >= _nextFireTime && _target != null && IsTargetInShootAngle())
        {
            _nextFireTime = Time.time + 1.0f / turretInfo.FireRate;
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
        projectileInstance.GetComponent<BaseBullet>().Initialized(_target.position, turretInfo.BaseDamage, turretInfo.BulletSpeed);
    }

    public virtual void UpgradeTurret()
    {
        TurretLevel++;
        _nextFireTime = Time.time;
        print("Турель уровня: " + TurretLevel);
    }

    public virtual void SellTurret()
    {
        PlayerManager.Instance.AddCoins(turretInfo.BuyPrice / 2 + (TurretLevel - 1) * (turretInfo.UpgradePrice / 2));
        TurretLevel = 1;
        print("Турель продана");
    }
}