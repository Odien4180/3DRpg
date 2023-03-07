using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

[RequireComponent(typeof(Button))]
public class ButtonModule : MonoBehaviour
{
    public Button button;
    public RectTransform rt;

    public Vector3 scale;
    public Ease scaleEase;
    public float scaleTime;

    private Vector3 scaleOrigin;

    private Tween currentScaleTween;

    private IDisposable clickEvent;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (rt == null)
            rt = button?.GetComponent<RectTransform>();

        scaleOrigin = rt.localScale;
    }

    private void Start()
    {
        button.OnClickAsObservable().Subscribe(_ => TouchEvent()).AddTo(this);
    }

    private void TouchEvent()
    {
        SetOrigin();

        currentScaleTween?.Kill();
        currentScaleTween = rt.DOScale(scale, scaleTime).SetUpdate(true).SetEase(scaleEase)
            .OnComplete(SetOrigin);
    }

    private void SetOrigin()
    {
        rt.localScale = scaleOrigin;
    }

    public void SetClickEvent(Action<Unit> action)
    {
        clickEvent?.Dispose();
        clickEvent = button.OnClickAsObservable().Subscribe(action).AddTo(this);
    }
}
