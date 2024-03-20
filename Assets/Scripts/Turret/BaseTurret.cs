using UnityEngine;


public class BaseTurret : MonoBehaviour
{
    [SerializeField] public TurretInfo turretInfo;
    [SerializeField] protected GameObject projectile;

    public int TurretLevel = 1;
    public int TurretMaxLevel = 3;

    protected Transform _target;
    protected float _fireAngel = 5f;
    protected float _targetUpdateInterval = 0.1f;
    protected float _rotationDelay = 2.0f;
    protected float _rotationSpeed = 5f;
    protected float _nextFireTime;
    protected float _currentDelay;
    protected float _fireRateBoost = 0;
    protected float _damageBoost = 0;

    protected virtual void UpdateTarget()
    {
        GameObject nearestEnemy = EnemyManager.Instance.FindNearestEnemy(transform.position, turretInfo.Range);
        Transform newTarget = nearestEnemy?.transform;

        if (_target != newTarget)
        {
            _currentDelay = turretInfo.RotationDelay;
            _target = newTarget;
        }
    }

    protected virtual void FixedUpdate()
    {
        UpdateTarget();
        LookAtTarget();

        if (Time.time >= _nextFireTime && _target != null && IsTargetInShootAngle())
        {
            _nextFireTime = Time.time + 1.0f / (turretInfo.FireRate + _fireRateBoost*TurretLevel);
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

    protected bool IsTargetInShootAngle()
    {
        if (_target != null)
        {
            Vector3 targetDirection = (_target.position - transform.position).normalized;
            float angle = Vector3.Angle(targetDirection, transform.forward);
            return angle <= _fireAngel;
        }

        return false;
    }

    protected virtual void FireProjectile()
    {
        GameObject projectileInstance = Instantiate(projectile, transform.position, transform.rotation);
        projectileInstance.GetComponent<BaseBullet>().Initialized(_target.position, turretInfo.BaseDamage + _damageBoost*TurretLevel,
            turretInfo.BulletSpeed);
    }

    public virtual void UpgradeTurret()
    {
        TurretLevel++;
        _nextFireTime = Time.time;
        print("Турель уровня: " + TurretLevel);
    }

    public virtual void SellTurret()
    {
        TurretLevel = 1;
        print("Турель продана");
    }
}