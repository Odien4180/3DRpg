using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class ConversationClip : PlayableAsset
{
    public double start;
    public double end;
    public string conv_tag;
    public int conv_id;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<Typing>.Create(graph);
        Typing typing = playable.GetBehaviour();
        typing.convData = Conversations.Get(conv_tag, conv_id);

        return playable;
    }
}
