using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

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
    [SerializeField] private bool ovelapText = false;
    [SerializeField] private GameObject speakBubble;

    [Header("TypingList")]
    [SerializeField] private List<DialogueList> dialogueList = new List<DialogueList>();

    [Header("SmokeSetting")]
    [SerializeField] private float smallFontSize;
    [SerializeField] private float fontSizeDownSpeed;
    [SerializeField] private float fontSizeUpSpeed;
    private bool isFontToSmallCorutineRunning = false;

    private void Awake()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        SmokeInteractable.OnSmokeBreakStarted += FontSizeToSmall;

        StartCoroutine(PopTest());
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
        //textBubble.text = "";
        string text;
        text = dialogueList[angryStage].dialogueObject.dialogueTexts[Random.Range(0, dialogueList[angryStage].dialogueObject.dialogueTexts.Count)];
        speakBubble.SetActive(true);

        if (!dialogueList[angryStage].stackable)
        {
            StartCoroutine(Co_TypingEffect(text, textBubble));
        }
        else
        {
            StartCoroutine(Co_TypingEffect(text, textBubble, true));
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
        while(tmp.fontSize <= smallFontSize)
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
        StopAllCoroutines();
        tmp.text = "";

        speakBubble.SetActive(false);
    }

    IEnumerator PopTest()
    {
        yield return new WaitForSeconds(5f);
        PopBubble();
    }
    #endregion
}
