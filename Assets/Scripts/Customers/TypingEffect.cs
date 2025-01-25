using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogueList
{
    public int angryStage;
    public DialogueObject dialogueObject;
    public bool stackable = false;
}

public class TypingEffect : MonoBehaviour
{
    TextMeshProUGUI tmp;

    [Header("TypingSetting")]
    [SerializeField] private float typingSpped = 0f;
    [SerializeField] private GameObject speakBubble;

    [Header("TypingList")]
    [SerializeField] private List<DialogueList> dialogueList = new List<DialogueList>();

    [Header("SmokeSetting")]
    [SerializeField] private float smallFontSize;
    [SerializeField] private float fontSizeDownSpeed;
    [SerializeField] private float fontSizeUpSpeed;
    private bool isFontToSmallCorutineRunning = false;

    [Header("ExistingTasks")]
    private IEnumerator currentTalkingCoroutine;
    private string existingCompletionText;

    [Header("RedBubble")]
    [SerializeField] private float shakeValue;
    [SerializeField] private float shakePower;
    [SerializeField] private float chageColorRedSpeed;
    [SerializeField] private Image bubbleImage;
    [SerializeField] private Animation bubblePop;
    [SerializeField] private Color baseColor;
    [SerializeField] private Color redColor;
    [SerializeField] private AnimationCurve lerpCurve;

    private void Awake()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        bubbleImage = GetComponentInChildren<Image>();
    }

    private void OnEnable()
    {
        SmokeInteractable.OnSmokeBreakStarted += FontSizeToSmall;
    }

    private void OnDisable()
    {
        SmokeInteractable.OnSmokeBreakStarted -= FontSizeToSmall;
    }

    private void Update()
    {
        if (!isFontToSmallCorutineRunning)
        {
            FontSizeUp();
        }
    }

    #region TypingEffect
    public void StartTypingEffect(TextMeshProUGUI textBubble, int angryStage, AllDishes allDishes)
    {
        if (angryStage < 1 || angryStage > dialogueList.Count)
        {
            Debug.LogWarning("angryStage is out of range.");
            return;
        }

        string text;
        angryStage--;
        text = dialogueList[angryStage].dialogueObject.dialogueTexts[Random.Range(0, dialogueList[angryStage].dialogueObject.dialogueTexts.Count)];
        text = ReplacePlaceholders(text, allDishes);
        speakBubble.SetActive(true);

        if (currentTalkingCoroutine != null && !dialogueList[angryStage].stackable)
        {
            StopAllCoroutines();
            textBubble.text = "";
        }
        else
        {
            StopAllCoroutines();
            textBubble.text = existingCompletionText;
            existingCompletionText = textBubble.text + "\n" + text;
        }

        if (!dialogueList[angryStage].stackable)
        {
            currentTalkingCoroutine = Co_TypingEffect(text, textBubble);
            StartCoroutine(currentTalkingCoroutine);
        }
        else
        {
            currentTalkingCoroutine = Co_TypingEffect(text, textBubble, true);
            StartCoroutine(currentTalkingCoroutine);
        }
    }

    IEnumerator Co_TypingEffect(string text, TextMeshProUGUI textBubble)
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < text.Length; i++)
        {
            stringBuilder.Append(text[i]);
            textBubble.text = stringBuilder.ToString();
            yield return new WaitForSeconds(typingSpped);
        }
        currentTalkingCoroutine = null;
    }

    IEnumerator Co_TypingEffect(string text, TextMeshProUGUI textBubble, bool stackalbe)
    {
        StringBuilder stringBuilder = new StringBuilder();
        string originalText = textBubble.text;
        Debug.Log("CurrentText : " + text);
        for (int i = 0; i < text.Length; i++)
        {
            stringBuilder.Append(text[i]);
            textBubble.text = originalText + "\n" + stringBuilder.ToString();
            yield return new WaitForSeconds(typingSpped);
        }
        textBubble.text += "... ";
        Debug.Log("Text Done");

        currentTalkingCoroutine = null;
    }
    #endregion

    #region FontSizeEffect
    public void FontSizeToSmall()
    {
        StartCoroutine(Co_FontSizeToSmall());
    }

    IEnumerator Co_FontSizeToSmall()
    {
        isFontToSmallCorutineRunning = true;
        while (tmp.fontSize <= smallFontSize)
        {
            tmp.fontSize = Mathf.Lerp(tmp.fontSize, smallFontSize, fontSizeDownSpeed);
            yield return null;
        }
        isFontToSmallCorutineRunning = false;
    }

    public void FontSizeUp()
    {
        tmp.fontSize += fontSizeUpSpeed;
    }
    #endregion

    private string ReplacePlaceholders(string text, AllDishes allDishes)
    {
        RestaurantMenuItem mainDish = GetRandomItem(allDishes.allMainDish);
        RestaurantMenuItem sideDish = GetRandomItem(allDishes.allSideDish);
        RestaurantMenuItem drink = GetRandomItem(allDishes.allDrink);

        string mDish = mainDish.ItemName;
        string sDish = sideDish.ItemName;
        string dDrink = drink.ItemName;

        text = text.Replace("{01}", mDish);
        text = text.Replace("{02}", sDish);
        text = text.Replace("{00}", dDrink);

        return text;
    }

    private RestaurantMenuItem GetRandomItem(List<RestaurantMenuItem> items)
    {
        if (items == null || items.Count == 0)
        {
            return null;
        }
        return items[Random.Range(0, items.Count)];
    }
    
    #region PopBubble
    public void PopBubble()
    {
        bubblePop.Play();
        currentTalkingCoroutine = null;
        Debug.Log("PopBubble");
        StopAllCoroutines();
        tmp.text = "";

        speakBubble.SetActive(false);
    }
    public void ChangeColorToRed(float angry)
    {
        float normalizedAngry = Mathf.Clamp01(angry / 300f);
        float curveValue = lerpCurve.Evaluate(normalizedAngry);

        bubbleImage.color = Color.Lerp(redColor, baseColor, curveValue);

        if (angry <= shakeValue)
        {
            ApplyShake(normalizedAngry);
        }
    }

    private Coroutine shakeCoroutine;

    private void ApplyShake(float intensity)
    {
        if (shakeCoroutine != null) return; 
        shakeCoroutine = StartCoroutine(ShakeRoutine(intensity));
    }

    private IEnumerator ShakeRoutine(float intensity)
    {
        Vector3 originalPosition = speakBubble.transform.localPosition;
        float shakeDuration = 0.5f; 
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            // Calculate shake amount based on intensity and shakePower
            float shakeAmount = Mathf.Lerp(0, shakePower, 1f - intensity);

            // Generate random offset
            float offsetX = Random.Range(-shakeAmount, shakeAmount);
            float offsetY = Random.Range(-shakeAmount, shakeAmount);

            // Apply shake effect
            speakBubble.transform.localPosition = new Vector3(
                originalPosition.x + offsetX,
                originalPosition.y + offsetY,
                originalPosition.z
            );

            // Wait before applying the next shake
            yield return new WaitForSeconds(0.05f);

            elapsed += 0.05f;
        }

        // Reset position and stop shaking
        speakBubble.transform.localPosition = originalPosition;
        shakeCoroutine = null;
    }
    #endregion
}
