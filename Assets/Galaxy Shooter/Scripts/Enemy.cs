using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    private float _speed;

    private bool _alive;

    private UIManager _uiManager;

    private GameManager _gameManager;

    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _explosionAudio;

    [SerializeField]
    private GameObject[] _thrusters;

    // Use this for initialization
    void Start () {
        _alive = true;

        _audioSource = GetComponent<AudioSource>();

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
        Movement();
	}

    void Movement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6.67f && _alive)
        {
            if (_gameManager.GameRunning())
            {
                transform.position = new Vector3(Random.Range(-7.5f, 7.5f), 6.6f, 0f);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_alive)
        {
            return;
        }
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage();
            }
            Die();
        }
        else if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            Die();
        }
    }

    private void Die()
    {
        foreach (GameObject thruster in _thrusters)
        {
            Destroy(thruster);
        }
        Animator deathAnimation = GetComponent<Animator>();
        deathAnimation.Play("EnemyExplosion");
        _alive = false;
        _uiManager.IncrementScore(1);
        _audioSource.PlayOneShot(_explosionAudio);
        Destroy(gameObject, 3.0f);
        
        //Destroy(gameObject, deathAnimation.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        //Instantiate(_deathAnimation, transform.position, Quaternion.identity);
        //Destroy(gameObject);
    }
}
