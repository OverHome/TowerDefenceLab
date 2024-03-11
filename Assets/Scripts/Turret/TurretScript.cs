using System.Collections;
using UnityEngine;

[RequireComponent(typeof(InteractiveObject))]
public class TurretScript : MonoBehaviour
{
    [SerializeField] private Canvas turretUI;
    [SerializeField] private int upgradePrice;
    [SerializeField] private int sellPrice;

    [SerializeField] private float fireRate = 1.0f;
    [SerializeField] private float range = 10.0f;
    [SerializeField] private GameObject projectile;

    private BulletScript _projectileScript;
    private float _nextFireTime;
    private float _targetUpdateInterval = 0.1f;
    private Transform _target;
    private InteractiveObject _interactiveObject;
    private TurretPlace _turretPlace;
    private int _turretLevel = 1;
    private float _damage;

    private void OnEnable()
    {
        _interactiveObject = GetComponent<InteractiveObject>();
        _turretPlace = transform.parent.GetComponent<TurretPlace>();
        _projectileScript = projectile.GetComponent<BulletScript>();
        _damage = _projectileScript.Damage;
        _interactiveObject.OnObjectPressed.AddListener(ShowUI);
    }

    private void OnDisable()
    {
        _interactiveObject.OnObjectPressed.RemoveListener(ShowUI);
    }

    private void Start()
    {
        InvokeRepeating(nameof(UpdateTarget), 0.0f, _targetUpdateInterval);
    }

    private void UpdateTarget()
    {
        GameObject nearestEnemy = FindNearestEnemy();
        _target = nearestEnemy?.transform;
    }

    private GameObject FindNearestEnemy()
    {
        float shortestDistance = Mathf.Infinity;
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
        LookAtTarget();

        if (Time.time >= _nextFireTime && _target != null)
        {
            FireProjectile();
        }
    }

    private void LookAtTarget()
    {
        if (_target != null)
        {
            transform.LookAt(_target);
        }
    }

    private void FireProjectile()
    {
        _nextFireTime = Time.time + 1.0f / (fireRate*_turretLevel);

        GameObject projectileInstance = Instantiate(projectile, transform.position, transform.rotation);
        projectileInstance.GetComponent<BulletScript>().Damage = _damage;
        projectileInstance.GetComponent<Rigidbody>().AddForce(transform.forward * 20.0f, ForceMode.Impulse);
    }

    private void ShowUI()
    {
        turretUI.gameObject.SetActive(true);
    }
    
    public void UpgradeTurret()
    {
        if (PlayerManager.Instance.TotalCoins < upgradePrice) return;
        PlayerManager.Instance.AddCoins(-upgradePrice);
        _turretLevel++;
        _nextFireTime = Time.time;
        _damage += _turretLevel*10;
       print("Турель уровня: " + _turretLevel);
    }
    
    public void SellTurret()
    {
        turretUI.gameObject.SetActive(false);
        _turretPlace.SellTurret();
        PlayerManager.Instance.AddCoins(sellPrice);
        _turretLevel = 1;
    }
}
