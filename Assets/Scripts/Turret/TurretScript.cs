using System.Collections;
using UnityEngine;

[RequireComponent(typeof(InteractiveObject))]
public class TurretScript : MonoBehaviour
{
    [SerializeField] private Canvas turretUI;
    [SerializeField] private int upgradePrice;

    public float fireRate = 1.0f;
    public float range = 10.0f;
    public GameObject projectile;

    private float _nextFireTime = 0.0f;
    private float _targetUpdateInterval = 0.1f;
    private Transform _target;
    private InteractiveObject _interactiveObject;
    private int _turretLevel = 1; // Уровень турели

    private void OnEnable()
    {
        _interactiveObject = GetComponent<InteractiveObject>();
        _interactiveObject.OnObjectPressed.AddListener(ShowUI);
        InputManager.Instance.OnScreenTap.AddListener(HideUI);
    }

    private void OnDisable()
    {
        _interactiveObject.OnObjectPressed.RemoveListener(ShowUI);
        InputManager.Instance.OnScreenTap.RemoveListener(HideUI);
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
        projectileInstance.transform.LookAt(_target);
        projectileInstance.GetComponent<Rigidbody>().AddForce(transform.forward * 20.0f, ForceMode.Impulse);
    }

    private void ShowUI()
    {
        turretUI.gameObject.SetActive(true);
    }

    private void HideUI(Vector2 screenPosition)
    {
        if (!RectTransformUtility.RectangleContainsScreenPoint(turretUI.GetComponent<RectTransform>(), screenPosition))
        {
            turretUI.gameObject.SetActive(false);
        }
    }
    
    public void UpgradeTurret()
    {
        if (PlayerManager.Instance.TotalCoins < upgradePrice) return;
        PlayerManager.Instance.AddCoins(-upgradePrice);
        _turretLevel++;
        _nextFireTime = Time.time;
        Debug.Log("Turret upgraded to level " + _turretLevel);
    }
}
