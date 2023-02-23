using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    public Renderer _renderer;
    public Material matDissolve;
    public float dissolveTime = 1.0f;
    public float phaseTime = 1.0f;
    public float maxNoiseValue = 1.0f;
    public float minNoiseValue = 0.0f;
    public float curNoiseVal;

    public bool active = false;

    private Material matOrigin;


    private void Awake()
    {
        matOrigin = _renderer.material;
        
        matDissolve.SetFloat("_NoiseValue", curNoiseVal);

        gameObject.SetActive(active);        
    }

    public bool Dissolve(Action onComplete = null)
    {
        if (active == false)
            return false;

        DOTween.Kill(_renderer.material);
        _renderer.material = matDissolve;
        _renderer.material.SetFloat("_NoiseValue", curNoiseVal);

        float noiseVal = _renderer.material.GetFloat("_NoiseValue");

        _renderer.material.DOFloat(minNoiseValue, "_NoiseValue",
            (noiseVal - minNoiseValue) / (maxNoiseValue - minNoiseValue) * dissolveTime)
            .OnUpdate(() =>
            {
                curNoiseVal = _renderer.material.GetFloat("_NoiseValue");
            })
            .OnComplete(() =>
            {
                onComplete?.Invoke();
                gameObject.SetActive(false);
            });

        active = false;

        return true;
    }

    public bool Phase()
    {
        if (active)
        {
            Debug.Log("No Phase");
            return false;
        }

        DOTween.Kill(_renderer.material);
        _renderer.material = matDissolve;
        active = true;
        gameObject.SetActive(true);
        _renderer.material.SetFloat("_NoiseValue", curNoiseVal);

        float noiseVal = _renderer.material.GetFloat("_NoiseValue");

        _renderer.material.DOFloat(maxNoiseValue, "_NoiseValue",
            (maxNoiseValue - noiseVal) / (maxNoiseValue - minNoiseValue) * phaseTime)
            .OnUpdate(() =>
            {
                curNoiseVal = _renderer.material.GetFloat("_NoiseValue");
            })
            .OnComplete(() =>
            {
                _renderer.material = matOrigin;
            });
        
        return true;
    }
}
