using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackBindingType(typeof(ConversationBox))]
[TrackClipType(typeof(ConversationClip))]
public class ConversationLine : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        foreach (var clip in GetClips())
        {
            var myAsset = clip.asset as ConversationClip;
            if (myAsset)
            {
                myAsset.start = clip.start;
                myAsset.end = clip.end;
            }
        }

        return base.CreateTrackMixer(graph, go, inputCount);
    }
}
