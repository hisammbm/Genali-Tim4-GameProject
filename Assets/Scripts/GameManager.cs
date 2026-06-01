using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Score")]
    [SerializeField] private TMP_Text txtScore;
    [SerializeField] private TMP_Text txtHighScore;
    [SerializeField] private TMP_Text txtGameOverScore;
    private int currentScore = 0;
    private int highScore = 0;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        highScore = PlayerPrefs.GetInt("highScore", 0);
        txtScore.text = currentScore.ToString();
        txtGameOverScore.text = currentScore.ToString();
        txtHighScore.text = highScore.ToString();
    }

    public void AddScore(int score)
    {
        currentScore += score;
        txtScore.text = $"{currentScore}";
        txtGameOverScore.text = $"{currentScore}";

        if (highScore < currentScore)
        {
            PlayerPrefs.SetInt("highScore", currentScore);
            txtHighScore.text = $"{currentScore}";
        }
    }
}
