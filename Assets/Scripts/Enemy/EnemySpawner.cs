using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private WavesInfo wavesInfo;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform towerPos;

    private EnemyScript _enemyScript;
    private float _countdown = 2;
    private int _waveCount;
    private bool _inSpawn;

    void Start()
    {
        EnemyScript.TowerPos = towerPos;
        if (LevelManager.Instance?.GetWavesInfo() != null)
        {
            wavesInfo = LevelManager.Instance.GetWavesInfo();
        }

        StartCoroutine(WaveControl());
    }

    private IEnumerator WaveControl()
    {
        yield return new WaitForSeconds(_countdown);
        while (_waveCount != wavesInfo.waves.Length)
        {
            yield return new WaitForSeconds(wavesInfo.waves[_waveCount].WaitTime);
            if ((!EnemyManager.Instance.AreEnemiesAlive() && !_inSpawn) || !wavesInfo.waves[_waveCount].IsWait)
            {
                StartCoroutine(WaveSpawn(wavesInfo.waves[_waveCount]));
                _waveCount++;
            }
        }

        while (EnemyManager.Instance.AreEnemiesAlive())
        {
            yield return new WaitForSeconds(_countdown);
        }

        GameManager.Instance.EndGame(true);
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