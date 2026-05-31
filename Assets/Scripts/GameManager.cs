using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Score")]
    [SerializeField] private TMP_Text txtScore;
    private int currentScore = 0;

    private void Awake()
    {
        instance = this;
    }

    public void AddScore(int score)
    {
        currentScore += score;
        txtScore.text = $"{currentScore}";
    }
}
