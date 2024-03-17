using System;
using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] public WavesInfo WavesInfo;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform towerPos;

    private EnemyScript _enemyScript;
    private float _countdown = 1;
    private int _waveCount;
    private bool _inSpawn;

    void Start()
    {
        EnemyScript.TowerPos = towerPos;
        StartCoroutine(WaveControl());
    }

    private IEnumerator WaveControl()
    {
        yield return new WaitForSeconds(_countdown);
        while (_waveCount != WavesInfo.waves.Length)
        {
            if ((!EnemyManager.Instance.AreEnemiesAlive() && !_inSpawn) || !WavesInfo.waves[_waveCount].IsWait)
            {
                StartCoroutine(WaveSpawn(WavesInfo.waves[_waveCount]));
                _waveCount++;
            }

            yield return new WaitForSeconds(WavesInfo.waves[_waveCount].WaitTime);
        }

        while (EnemyManager.Instance.AreEnemiesAlive())
        {
            yield return new WaitForSeconds(_countdown);
        }
        PlayerManager.Instance.StopGame();
    }

    private IEnumerator WaveSpawn(Wave wave)
    {
        _inSpawn = true;
        for (int i = 0; i < wave.Count; i++)
        {
            SpawnEnemy(wave.Enemy, wave.HealthMultiplier);
            yield return new WaitForSeconds(wave.Rate);
        }

        _inSpawn = false;
    }


    void SpawnEnemy(GameObject enemyPrefab, float health)
    {
        GameObject enemyInstance = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        EnemyScript enemy = enemyInstance.GetComponent<EnemyScript>();
        enemy.StartHealth *= health; 
    }

}