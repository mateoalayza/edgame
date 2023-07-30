using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

public class RectTransformAnimation : MonoBehaviour
{
    public RectTransform rectTransform;
    public float targetWidth;
    public float animationDuration = 1.0f;

    [TextArea]
    public string texto;

    public TextMeshProUGUI text_obj;
    public WordByWordText word;
    public float timeb;

    public GameObject but;
    public GameObject but_parent;


    // variables
    public List<GameObject> objectsToBounce = new List<GameObject>();
    public float bounceDuration = 1.0f;
    public float bounceStrength = 1.2f;
    public float intervalBetweenObjects = 0.5f;

    private void Start()
    {
        rectTransform.sizeDelta = new Vector2(0f, rectTransform.rect.height);
        text_obj.text = "";
        but.SetActive(false);
    }

    public void AnimateWidthThenExecute(bool onoff)
    {
        // Guardamos el ancho inicial.
        float initialWidth = rectTransform.rect.width;

        if(onoff is true)
        {
            targetWidth = 1602f;
            // Animamos el cambio del ancho a targetWidth.
            rectTransform.DOSizeDelta(new Vector2(targetWidth, rectTransform.rect.height), animationDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() => OnAnimationComplete());
        }
        else
        {
            but.SetActive(false);
            text_obj.text = "";
            targetWidth = 0f;
            // Animamos el cambio del ancho a targetWidth.
            rectTransform.DOSizeDelta(new Vector2(targetWidth, rectTransform.rect.height), animationDuration)
                .SetEase(Ease.OutQuad);
        }

        
    }

    private void OnAnimationComplete()
    {
        // Este es el método vacío que se ejecutará al completarse la animación.
        Debug.Log("La animación ha finalizado.");
        word.DisplayTextByLetter(text_obj, texto, timeb);
    }

    public void activate()
    {
        but.SetActive(true);
    }

    public void deactivate()
    {
        AnimateWidthThenExecute(false);
        objectsToBounce.Clear();

        for (int i = 0; i < but_parent.transform.childCount; i++)
        {
            objectsToBounce.Add(but_parent.transform.GetChild(i).gameObject);
        }

        StartCoroutine(BounceObjectsWithInterval());
    }

    public IEnumerator BounceObjectsWithInterval()
    {
        foreach (var obj in objectsToBounce)
        {
            obj.SetActive(true); // Activa el objeto
            obj.transform.DOScale(Vector3.zero, 0f); // Establece la escala inicial en cero

            // Realiza el efecto "bounce" en la escala usando DoTween
            obj.transform.DOScale(Vector3.one * bounceStrength, bounceDuration)
                .SetEase(Ease.OutBounce);

            yield return new WaitForSeconds(intervalBetweenObjects);
        }
    }


}
