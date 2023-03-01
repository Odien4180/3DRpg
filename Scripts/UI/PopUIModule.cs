using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUIModule : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public RectTransform rt;
    

    [Header("이동")]
    private Vector2 originPos;
    public bool doMove;
    public Vector2 translate;
    public float translateTime;
    public Ease translateEase;

    private Tween translateTween;

    [Header("알파값")]
    private float originAlpha;
    public bool doFade;
    public float alpha;
    public float alphaTime;
    public Ease alphaEase;

    private Tween alphaTween;

    private void Awake()
    {
        if (doMove)
            originPos = rt.anchoredPosition;
        if (doFade)
            originAlpha = canvasGroup.alpha;
    }

    private void OnEnable()
    {
        if (doMove)
        {
            translateTween?.Kill();
            rt.localPosition = originPos + translate;
            translateTween = rt.DOAnchorPos(originPos, translateTime).SetUpdate(true).SetEase(translateEase);
        }

        if (doFade)
        {
            alphaTween?.Kill();
            canvasGroup.alpha = alpha;
            canvasGroup.DOFade(originAlpha, alphaTime).SetUpdate(true).SetEase(alphaEase);
        }
    }
}
