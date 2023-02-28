using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class InteractionPopUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform maskTransform;

    public float moveYvalue = 10.0f;
    public float fadeDuration = 0.15f;

    private RectTransform rectTransform;

    private Vector3 originPos;

    private Sequence tween;

    public void Initialize(string name)
    {
        text.text = name;
        canvasGroup.alpha = 0.0f;
        rectTransform = transform as RectTransform;
        
        originPos = rectTransform.localPosition;
        rectTransform.localPosition = originPos - new Vector3(0, moveYvalue, 0);
    }

    public void FadeIn()
    {
        tween = DOTween.Sequence();

        tween.Append(canvasGroup.DOFade(1.0f, fadeDuration).SetEase(Ease.Linear))
            .Join(rectTransform.DOAnchorPosY(moveYvalue, fadeDuration)).SetEase(Ease.Linear)
            .Append(DOTween.To(() => maskTransform.offsetMax, x => maskTransform.offsetMax = x, 
            new Vector2(400.0f, maskTransform.offsetMax.y), 1.0f));
    }

    public void FadeOut()
    {
        tween?.Kill();

        tween = DOTween.Sequence();

        tween.Append(canvasGroup.DOFade(0.0f, fadeDuration).SetEase(Ease.Linear))
            .Join(rectTransform.DOAnchorPosY(moveYvalue + moveYvalue, fadeDuration)).SetEase(Ease.Linear)
            .OnComplete(() => Destroy(gameObject));
    }
}
