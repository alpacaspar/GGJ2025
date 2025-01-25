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
    public void StartTypingEffect(TextMeshProUGUI textBubble, int angryStage)
    {
        //textBubble.text = "";
        string text;
        text = dialogueList[angryStage].dialogueObject.dialogueTexts[Random.Range(0, dialogueList[angryStage].dialogueObject.dialogueTexts.Count)];

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
}
