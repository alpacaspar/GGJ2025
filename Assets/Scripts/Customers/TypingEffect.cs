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
    [Header("TypingSetting")]
    [SerializeField] private float typingSpped = 0f;
    [SerializeField] private bool ovelapText = false;

    [Header("TypingList")]
    [SerializeField] private List<DialogueList> dialogueList = new List<DialogueList> ();

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
}
