using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HitData
{
    public Vector3 position;
    public int damage;
}

public interface IHittable
{
    void Hit(HitData hitData);
}
