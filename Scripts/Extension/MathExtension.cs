using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathExtension
{
    //���� �Ÿ� �̳� ���� ��ġ ��ȯ
    public static Vector3 RandomPointInCircle(this Vector3 position, float radius)
    {
        Vector3 vec = new Vector3(0, 0, 1);
        var randVec = Quaternion.AngleAxis(Random.Range(0, 360.0f), Vector3.up) * vec;

        float randLength = Random.Range(0, radius);

        return randVec * randLength + position;
    }
}
