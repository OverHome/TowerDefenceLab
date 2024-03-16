using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform towerPos;
    [SerializeField] private float initialDelay = 2f;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float difficultyIncreaseInterval = 20f;
    [SerializeField] private float difficultyIncreaseAmount = 1.2f;

    private float _spawnTimer;
    private float _difficultyIncreaseTimer;
    private float _startHealth;
    private EnemyScript _enemyScript;

    void Start()
    {
        EnemyScript.TowerPos = towerPos;
        
        _enemyScript = enemyPrefab.GetComponent<EnemyScript>();
        _startHealth =_enemyScript.StartHealth;
        _spawnTimer = initialDelay;
        _difficultyIncreaseTimer = 0f;
    }

    void Update()
    {
        UpdateTimers();

        if (_spawnTimer <= 0f)
        {
            SpawnEnemy();
            _spawnTimer = spawnInterval;
        }

        if (_difficultyIncreaseTimer >= difficultyIncreaseInterval)
        {
            IncreaseDifficulty();
            _difficultyIncreaseTimer = 0f;
        }
    }

    void UpdateTimers()
    {
        _spawnTimer -= Time.deltaTime;
        _difficultyIncreaseTimer += Time.deltaTime;
    }

    void SpawnEnemy()
    {
        GameObject enemyInstance = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        EnemyScript enemy = enemyInstance.GetComponent<EnemyScript>();
        enemy.StartHealth = _startHealth;
        EnemyManager.Instance.RegisterEnemy(enemy);
    }

    void IncreaseDifficulty()
    {
        spawnInterval /= difficultyIncreaseAmount;
        _startHealth *= difficultyIncreaseAmount;
       print("Сложность увеличена!");
    }
}