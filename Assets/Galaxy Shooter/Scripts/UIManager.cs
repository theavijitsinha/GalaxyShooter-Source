using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    [SerializeField]
    private UnityEngine.UI.Image _titleImage;

    [SerializeField]
    private Sprite[] _livesSprites;

    [SerializeField]
    private UnityEngine.UI.Image _livesImage;

    [SerializeField]
    private UnityEngine.UI.Text _scoreText;

    private const string SCORE_TEMPLATE = "Score: {0:D3}";
    private int _score;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

    }

    public void UpdateLives(int lives)
    {
        _livesImage.sprite = _livesSprites[lives];
    }

    public void IncrementScore(int points)
    {
        _score += points;
        UpdateScore();
    }

    public void ResetScore()
    {
        _score = 0;
        UpdateScore();
    }

    private void UpdateScore()
    {
        _scoreText.text = string.Format(SCORE_TEMPLATE, _score);
    }

    public void ShowTitle(bool enabled)
    {
        _titleImage.gameObject.SetActive(enabled);
    }
}
