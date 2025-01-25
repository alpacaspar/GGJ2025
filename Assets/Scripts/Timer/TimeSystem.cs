using System.Collections;
using TMPro;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    public static TimeSystem instance;

    [Header("Timer")]
    [SerializeField] private float currentTime = 0;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Penalty")]
    [SerializeField] private int maxPenalty;
    [SerializeField] private int currentPenalty;
    [SerializeField] private TextMeshProUGUI penaltyText;

    [Header("Score")]
    [SerializeField] private int currentScore;
    [SerializeField] private float scoreUpdateValue;
    [SerializeField] private float scoreUpdateTimeValue;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake()
    {
        instance = this;

        currentPenalty = maxPenalty;
    }

    private void Start()
    {
        currentScore = 0;
        currentTime = 0;
        StartCoroutine(Co_UpdateScore());
    }

    private void Update()
    {
        UpdateTime();
    }

    private void UpdateTime()
    {
        if (currentPenalty <= 0) return;

        currentTime += Time.deltaTime;
        if(timerText != null) timerText.text = currentTime.ToString("N2");
    }

    IEnumerator Co_UpdateScore()
    {
        while (currentPenalty >= maxPenalty)
        {
            yield return new WaitForSeconds(scoreUpdateTimeValue);
            currentScore += (int)scoreUpdateValue;
            if(scoreText != null) scoreText.text = currentScore.ToString();
        }
    }

    public void GetPenalty()
    {
        currentPenalty++;
        if(penaltyText != null) penaltyText.text = currentPenalty.ToString();
    }
}
