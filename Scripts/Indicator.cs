using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum EIndicator
{
    None,
    Circle
}

public class Indicator : MonoBehaviour
{
    [SerializeField] private EIndicator type;
    public Material mat;
    public DecalProjector projector;

    private void Start()
    {
        if (projector == null)
            projector = GetComponent<DecalProjector>();

        projector.material = new Material(projector.material);

        if (mat == null)
            mat = projector.material;

        Initialize(new Vector3(11, 3, 20), 5, 10, new Vector3(90, 0, 180), 3.0f);
    }

    public void Initialize(Vector3 position, float size, float depth, Vector3 rotation, float duration)
    {
        Initialize(position, size, depth, Quaternion.Euler(rotation), duration);
    }

    public void Initialize(Vector3 position, float size, float depth, Quaternion rotation, float duration)
    {
        transform.position = position;
        projector.size = new Vector3(size, size, depth);
        mat.SetFloat("_Fill", 0.0f);
        mat.DOFloat(1.0f, "_Fill", duration);
    }
}
