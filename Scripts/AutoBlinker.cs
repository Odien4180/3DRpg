using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoBlinker : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMesh;
    public string blendShapeName = "Fcl_EYE_Close";
    private int blendShapeIndex;

    public float blinkInterval = 5.0f;
    public float blinkCloseDuration = 0.06f;
    public float blinkOpeningSeconds = 0.03f;
    public float blinkClosingSeconds = 0.01f;

    private Coroutine blinkCoroutine;

    private void Awake()
    {
        blendShapeIndex = GetBlendsShapeIndex(blendShapeName);
    }

    private int GetBlendsShapeIndex(string blendShapeName)
    {
        Mesh mesh = skinnedMesh.sharedMesh;
        return mesh.GetBlendShapeIndex(blendShapeName);
    }

    private IEnumerator BlinkCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(blinkInterval);

            var value = 0f;
            var closeSpeed = 1.0f / blinkClosingSeconds;
            while(value < 1)
            {
                skinnedMesh.SetBlendShapeWeight(blendShapeIndex, value * 100);
                value += Time.deltaTime * closeSpeed;
                yield return null;
            }
            skinnedMesh.SetBlendShapeWeight(blendShapeIndex, 100);

            yield return new WaitForSeconds(blinkCloseDuration);

            value = 1.0f;
            var openSpeed = 1.0f / blinkOpeningSeconds;
            while (value > 0)
            {
                skinnedMesh.SetBlendShapeWeight(blendShapeIndex, value * 100);
                value -= Time.deltaTime * openSpeed;
                yield return null;
            }
            skinnedMesh.SetBlendShapeWeight(blendShapeIndex, 0);
        }
    }

    private void OnEnable()
    {
        blinkCoroutine = StartCoroutine(BlinkCoroutine());
    }

    private void OnDisable()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
    }
}
