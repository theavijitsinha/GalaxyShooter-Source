using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {
    enum PowerupType { TripleShot, SpeedBoost, Shield };

    [SerializeField]
    private float _speed;
    [SerializeField]
    private PowerupType _powerupId;

    [SerializeField]
    private AudioClip _powerupAudio;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -6.67f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player != null && player.isAlive())
            {
                switch (_powerupId)
                {
                    case PowerupType.TripleShot:
                        player.TripleShotPowerupOn();
                        break;
                    case PowerupType.SpeedBoost:
                        player.SpeedBoostPowerupOn();
                        break;
                    case PowerupType.Shield:
                        player.ShieldPowerupOn();
                        break;
                }
                AudioSource.PlayClipAtPoint(_powerupAudio, Camera.main.transform.position);
                Destroy(gameObject);
            }
        }
    }
}
