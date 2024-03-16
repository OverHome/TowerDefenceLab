using System.Collections;
using UnityEngine;

[RequireComponent(typeof(InteractiveObject))]
public class TurretScript : MonoBehaviour
{
    [SerializeField] private GameObject turretUI;
    [SerializeField] private int upgradePrice;
    [SerializeField] private int sellPrice;

    [SerializeField] private float fireRate = 1.0f;
    [SerializeField] private float range = 10.0f;
    [SerializeField] private GameObject sphereRange;
    [SerializeField] private float bulletSpeed = 10.0f; 
    [SerializeField] private GameObject projectile;

    // private BulletScript _projectileScript;
    private float _nextFireTime;
    private float _targetUpdateInterval = 0.1f;
    private Transform _target;
    private InteractiveObject _interactiveObject;
    private TurretPlace _turretPlace;
    public int TurretLevel = 1;
    public int TurretMaxLevel = 3;
    private float _damage;
    private float _rotationDelay = 2.0f;
    private float _currentDelay = 0.0f;
    private float _rotationSpeed = 5f;
    private TurretUIScript _uiScript;

    private void OnEnable()
    {
        _interactiveObject = GetComponent<InteractiveObject>();
        _turretPlace = transform.parent.GetComponent<TurretPlace>();
        // _projectileScript = projectile.GetComponent<BulletScript>();
        // _uiScript = turretUI.GetComponent<TurretUIScript>();
        // _damage = _projectileScript.Damage;
        _interactiveObject.OnObjectPressed.AddListener(ShowUI);
    }

    private void OnDisable()
    {
        _interactiveObject.OnObjectPressed.RemoveListener(ShowUI);
    }

    private void Start()
    {
        InvokeRepeating(nameof(UpdateTarget), 0.0f, _targetUpdateInterval);
        sphereRange.transform.localScale *= range;
    }

    private void UpdateTarget()
    {
        GameObject nearestEnemy = FindNearestEnemy();
        Transform newTarget = nearestEnemy?.transform;

        if (_target != newTarget)
        {
            _currentDelay = _rotationDelay; // Устанавливаем задержку перед поворотом к новой цели
            _target = newTarget;
        }
    }

    private GameObject FindNearestEnemy()
    {
        float shortestDistance = range;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }

    private void Update()
    {
        UpdateTarget();
        LookAtTarget();

        if (Time.time >= _nextFireTime && _target != null && IsTargetInShootAngle())
        {
            FireProjectile();
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


    private void LookAtTarget()
    {
        if (_target != null)
        {
            if (_currentDelay > 0.0f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_target.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, (_rotationSpeed / _rotationDelay) * Time.deltaTime);
                _currentDelay -= Time.deltaTime;
            }
            else
            {
                Quaternion targetRotation = Quaternion.LookRotation(_target.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            }
        }
    }


    private void FireProjectile()
    {
        _nextFireTime = Time.time + 1.0f / (fireRate * TurretLevel);

        GameObject projectileInstance = Instantiate(projectile, transform.position, transform.rotation);
        // projectileInstance.GetComponent<BulletScript>().Damage = _damage;
        // Vector3 bulletDirection = CalculateBulletDirection();
        // projectileInstance.GetComponent<Rigidbody>().velocity = bulletDirection;
    }

    private Vector3 CalculateBulletDirection()
    {
        if (_target != null)
        {
            return (_target.position - transform.position).normalized * bulletSpeed;
        }
        return Vector3.zero;
    }
    private void ShowUI()
    {
        turretUI.SetActive(true);
    }
    
    public void UpgradeTurret()
    {
        if (PlayerManager.Instance.TotalCoins < upgradePrice || TurretLevel == TurretMaxLevel) return;
        PlayerManager.Instance.AddCoins(-upgradePrice);
        TurretLevel++;
        _nextFireTime = Time.time;
        _damage += TurretLevel*10;
       print("Турель уровня: " + TurretLevel);
       _uiScript.SetLevelUI();
    }
    
    public void SellTurret()
    {
        turretUI.SetActive(false);
        _turretPlace.SellTurret();
        PlayerManager.Instance.AddCoins(sellPrice);
        TurretLevel = 1;
    }
}
