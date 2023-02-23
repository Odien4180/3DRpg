using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcInteraction : InteractionBase
{
    public string convName;

    public override void Interaction(QuadMapUnit unit)
    {
        ConversationManager.Instance.Initialize(convName).Forget();
    }
}
