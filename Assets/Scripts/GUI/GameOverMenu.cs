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
        int startScore = 0; // ������ 0���� ����
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // ������ �ε巴�� ������Ű�� ���� Lerp ���
            int currentScore = Mathf.RoundToInt(Mathf.Lerp(startScore, targetScore, t));
            scoreText.text = currentScore.ToString();

            yield return null; // ���� �����ӱ��� ���
        }

        // ���������� ��ǥ ������ ����
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