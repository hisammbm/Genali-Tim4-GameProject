using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Score")]
    [SerializeField] private TMP_Text txtScore;
    private int currentScore = 0;

    [Header("Buff")]
    [SerializeField] List<GameObject> buffs = new List<GameObject>();
    private int selectedBuff = 0;

    private void Awake()
    {
        instance = this;
    }

    public void AddScore(int score)
    {
        currentScore += score;
        txtScore.text = $"Score: {currentScore}";
    }

    public void GiveBuff(int wave)
    {
        if(wave%3 == 0)
        {

        }
    }
}
