using UnityEngine;
using TMPro;
using System.Collections;

public class WordByWordText : MonoBehaviour
{
    private Coroutine displayCoroutine;
    public RectTransformAnimation rect;

    public void DisplayTextByLetter(TMP_Text textMeshPro, string text, float letterDelay = 0.05f)
    {
        if (displayCoroutine != null)
            StopCoroutine(displayCoroutine);

        displayCoroutine = StartCoroutine(AnimateText(textMeshPro, text, letterDelay));
    }

    public void StopDisplayingText(TMP_Text textMeshPro, string originalText)
    {
        if (displayCoroutine != null)
            StopCoroutine(displayCoroutine);

        textMeshPro.text = originalText;
    }

    private IEnumerator AnimateText(TMP_Text textMeshPro, string originalText, float letterDelay)
    {
        textMeshPro.text = string.Empty;

        for (int i = 0; i < originalText.Length; i++)
        {
            textMeshPro.text += originalText[i];
            yield return new WaitForSeconds(letterDelay);
        }

        rect.activate();
        displayCoroutine = null;
    }
}

