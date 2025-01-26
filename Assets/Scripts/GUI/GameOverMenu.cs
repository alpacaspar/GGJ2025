using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class GameOverMenu : Menu
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject mainMenuObject;
    [SerializeField] private float updateDuration;

    protected override void OnEnable()
    {
        base.OnEnable();

        ScoreTextUpdate();
    }

    public void Btn_Retry()
    {
        GameManager.Instance.CurrentState = CurrentState.InGame;
        this.gameObject.SetActive(false);
    }

    public void Btn_BackToMenu()
    {
        GameManager.Instance.CurrentState = CurrentState.Menu;
        this.gameObject.SetActive(false);
        mainMenuObject.SetActive(true);
    }

    #region ScoreTextUpdate
    private IEnumerator UpdateScoreCoroutine(int targetScore, float duration)
    {
        int startScore = 0; // 점수를 0부터 시작
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // 점수를 부드럽게 증가시키기 위해 Lerp 사용
            int currentScore = Mathf.RoundToInt(Mathf.Lerp(startScore, targetScore, t));
            scoreText.text = currentScore.ToString();

            yield return null; // 다음 프레임까지 대기
        }

        // 최종적으로 목표 점수로 설정
        scoreText.text = targetScore.ToString();
    }

    public void ScoreTextUpdate()
    {
        if (TimeSystem.instance == null) return;

        int targetScore = TimeSystem.instance.currentScore;
        StartCoroutine(UpdateScoreCoroutine(targetScore, updateDuration));
    }
    #endregion
}