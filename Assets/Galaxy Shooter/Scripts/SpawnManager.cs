using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

    [SerializeField]
    private GameObject _enemyPrefab;

    [SerializeField]
    private GameObject[] _powerups;

    private bool _enemySpawnActive;
    private bool _powerupSpawnActive;

	// Use this for initialization
	void Start () {
        _enemySpawnActive = false;
        _powerupSpawnActive = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartEnemySpawn()
    {
        StartCoroutine(SpawnEnemy());
    }

    public void StopEnemySpawn()
    {
        _enemySpawnActive = false;
    }

    IEnumerator SpawnEnemy()
    {
        _enemySpawnActive = true;
        while (_enemySpawnActive)
        {
            if (_enemySpawnActive)
            {
                Instantiate(_enemyPrefab, new Vector3(Random.Range(-7.5f, 7.5f), 6.6f, 0f), Quaternion.identity);
            }
            yield return new WaitForSeconds(5.0f);
        }
    }

    public void StartPowerupSpawn()
    {
        StartCoroutine(SpawnPowerup());
    }

    public void StopPowerUpSpawn()
    {
        _powerupSpawnActive = false;
    }

    IEnumerator SpawnPowerup()
    {
        _powerupSpawnActive = true;
        while (_powerupSpawnActive)
        {
            int powerupId = Random.Range(0, _powerups.Length);
            float randomTime = Random.Range(10.0f, 20.0f);
            yield return new WaitForSeconds(randomTime);
            if (_powerupSpawnActive)
            {
                Instantiate(_powerups[powerupId], new Vector3(Random.Range(-7.5f, 7.5f), 6.6f, 0f), Quaternion.identity);
            }
        }
    }
}
