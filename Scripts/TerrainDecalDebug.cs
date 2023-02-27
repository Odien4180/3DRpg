using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TerrainDecalDebug : MonoBehaviour
{
    public enum EColor
    {
        Green,
        Red
    }

    public Material redMat;
    public Material greenMat;
    public DecalProjector decalProjector;

    private void Start()
    {
        if (decalProjector == null)
            decalProjector = GetComponent<DecalProjector>();
    }

    public void Set(Vector3 position, float width, float height, float depth, TerrainDecalDebug.EColor color)
    {
        transform.position = position;
        transform.rotation = Quaternion.Euler(90.0f, 0, 0);

        decalProjector.size = new Vector3(width, height, depth);

        if (color == EColor.Red)
            decalProjector.material = redMat;
        else if (color == EColor.Green)
            decalProjector.material = greenMat;
    }
}
