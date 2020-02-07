using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private GameObject _shield;
    [SerializeField]
    private GameObject _thruster;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleLaserPrefab;
    [SerializeField]
    private GameObject[] _engineDamages;

    [SerializeField]
    private float _fireRate;
    private float _nextFire = 0.0f;

    [SerializeField]
    private float _speed;
    [SerializeField]
    private bool _speedBoostEnabled;

    [SerializeField]
    private bool _tripleShotEnabled;

    [SerializeField]
    private bool _shieldEnabled;

    [SerializeField]
    private int _health;
    private bool _alive;

    private UIManager _uiManager;

    private GameManager _gameManager;

    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _laserAudio;
    [SerializeField]
    private AudioClip _explosionAudio;

    private Animator _animator;

    // Use this for initialization
    void Start () {
        _alive = true;

        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _uiManager.UpdateLives(_health);

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!_alive)
        {
            return;
        }
        Movement();
        TurnAnimation();

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            Shoot();
        }
    }

    private void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float currentSpeed = _speedBoostEnabled ? 1.5f * _speed : _speed;

        transform.Translate(Vector3.right * currentSpeed * horizontalInput * Time.deltaTime);
        transform.Translate(Vector3.up * currentSpeed * verticalInput * Time.deltaTime);

        if (transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
        else if (transform.position.y < -4.2f)
        {
            transform.position = new Vector3(transform.position.x, -4.2f, transform.position.z);
        }
        if (transform.position.x > 9f)
        {
            transform.position = new Vector3(-9f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < -9f)
        {
            transform.position = new Vector3(9f, transform.position.y, transform.position.z);
        }
    }

    private void TurnAnimation()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _animator.SetBool("TurnLeft", true);
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            _animator.SetBool("TurnRight", true);
        }
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            _animator.SetBool("TurnLeft", false);
        }
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            _animator.SetBool("TurnRight", false);
        }

    }

    private void Shoot()
    {
        if (Time.time > _nextFire)
        {
            _audioSource.PlayOneShot(_laserAudio);
            if (_tripleShotEnabled)
            {
                Instantiate(_tripleLaserPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0.0f, 0.88f, 0.0f), Quaternion.identity);
            }
            _nextFire = Time.time + _fireRate;
        }
    }

    private void Die()
    {
        _animator.Play("PlayerExplosion");
        _alive = false;
        _audioSource.PlayOneShot(_explosionAudio);
        _shield.SetActive(false);
        _thruster.SetActive(false);
        _engineDamages[0].SetActive(false);
        _engineDamages[1].SetActive(false);
        Destroy(gameObject, 3.0f);
        _gameManager.StopGame();
    }

    IEnumerator TripleShotPowerupOffRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _tripleShotEnabled = false;
    }

    public void TripleShotPowerupOn()
    {
        _tripleShotEnabled = true;
        StartCoroutine(TripleShotPowerupOffRoutine());
    }

    IEnumerator SpeedBoostPowerupOffRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speedBoostEnabled = false;
    }

    public void SpeedBoostPowerupOn()
    {
        _speedBoostEnabled = true;
        StartCoroutine(SpeedBoostPowerupOffRoutine());
    }

    IEnumerator ShieldPowerupOffRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _shieldEnabled = false;
        _shield.SetActive(false);
    }

    public void ShieldPowerupOn()
    {
        _shieldEnabled = true;
        _shield.SetActive(true);
        StartCoroutine(ShieldPowerupOffRoutine());
    }

    public void TakeDamage()
    {
        if (_shieldEnabled)
        {
            _shieldEnabled = false;
            _shield.SetActive(false);
        }
        else
        {
            _health -= 1;
            _uiManager.UpdateLives(_health);
            if (_health <= 0)
            {
                Die();
            }
            else
            {
                int engineToDamage = Random.Range(0, 2);
                if (_engineDamages[engineToDamage].activeSelf)
                {
                    _engineDamages[1 - engineToDamage].SetActive(true);
                }
                else
                {
                    _engineDamages[engineToDamage].SetActive(true);
                }
            }
        }
    }

    public bool isAlive()
    {
        return _alive;
    }
}
