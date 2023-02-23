using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimelineHelper : MonoBehaviour
{ 
    [Serializable]
    public class TimelineDictionary : SerializableDictionary<string, UnityEngine.Object> { }

    public PlayableDirector timeline;

    [SerializeField] private TimelineDictionary timelineDictionary;

    private void Awake()
    {
        if (timeline == null)
            timeline = GetComponent<PlayableDirector>();
    }
    public void Initialize(TimelineAsset ta)
    {
        timeline.playableAsset = ta;
    }

    public void Add(string key, UnityEngine.Object value)
    {
        if (timelineDictionary.ContainsKey(key))
            return;

        timelineDictionary.Add(key, value);
    }

    public void Clear()
    {
        timeline.playableAsset = null;
        timelineDictionary.Clear();
    }

    public void Binding()
    {
        TimelineAsset ta = timeline.playableAsset as TimelineAsset;
        IEnumerable<TrackAsset> tracks = ta.GetOutputTracks();

        foreach(var track in tracks)
        {
            if (timelineDictionary.TryGetValue(track.name, out var bindingTarget))
            {
                timeline.SetGenericBinding(track, bindingTarget);
            }   
        }
    }
}
