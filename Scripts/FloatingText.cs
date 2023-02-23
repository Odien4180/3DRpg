using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public AnimationCurve opacityCurve;
    public AnimationCurve scaleCurve;
    public AnimationCurve heightCurve;

    public TextMeshProUGUI tmp;
    public float maxTime = 1.0f;
    private float time = 0.0f;

    private Vector3 originPos;
    [SerializeField]
    private Color originColor;

    public void Initialize(string text, Vector3 position, Color originColor, float randomPosRadius = 0.0f)
    {
        transform.position = Random.insideUnitSphere * randomPosRadius + position;
        originPos = transform.position;
        this.originColor = originColor;
        tmp.faceColor = originColor;
        tmp.text = text;
        time = 0.0f;
    }

    private void Update()
    {
        if (time > maxTime)
        {
            ObjectPoolManager.Instance.Push(gameObject);
        }

        tmp.color = new Color(1, 1, 1, opacityCurve.Evaluate(time));
        transform.localScale = Vector3.one * scaleCurve.Evaluate(time);
        transform.position = originPos + Vector3.up * heightCurve.Evaluate(time);

        time += Time.deltaTime;
    }
}
