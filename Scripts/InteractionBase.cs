using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionBase : MonoBehaviour
{
    public float interactionDistance = 3.0f;
    public string interactionName = string.Empty;
    public abstract void Interaction(QuadMapUnit unit);
}
