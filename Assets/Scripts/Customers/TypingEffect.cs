using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class TypingEffect : MonoBehaviour
{
    [SerializeField] private float typingSpped = 0f;
    [SerializeField] private DialogueObject dialogueList;

    public string StartTypingEffect(TextMeshProUGUI textBubble)
    {
        string text;
        text = dialogueList.dialogueTexts[Random.Range(0, dialogueList.dialogueTexts.Count)];

        StartCoroutine(Co_TypingEffect(text, textBubble));

        return text;
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
}
