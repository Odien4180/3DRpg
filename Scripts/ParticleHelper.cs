using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ParticleHelper : MonoBehaviour
{
    public VisualEffect effect;

    public void Initialize(Vector3 position, Quaternion quaternion)
    {
        transform.position = position;
        transform.rotation = quaternion;
        effect.Play();
    }
}
