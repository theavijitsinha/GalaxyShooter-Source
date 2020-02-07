using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private bool _gameStarted;

    [SerializeField]
    private GameObject _playerPrefab;

    private UIManager _uiManager;
    private SpawnManager _spawnManager;

	// Use this for initialization
	void Start () {
        _gameStarted = false;

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space) && !_gameStarted)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        _gameStarted = true;
        _uiManager.ShowTitle(false);
        _uiManager.ResetScore();
        _spawnManager.StartEnemySpawn();
        _spawnManager.StartPowerupSpawn();
        Instantiate(_playerPrefab, Vector3.zero, Quaternion.identity);
    }

    public void StopGame()
    {
        _gameStarted = false;
        _uiManager.ShowTitle(true);
        _spawnManager.StopEnemySpawn();
        _spawnManager.StopPowerUpSpawn();
    }

    public bool GameRunning()
    {
        return _gameStarted;
    }
}
