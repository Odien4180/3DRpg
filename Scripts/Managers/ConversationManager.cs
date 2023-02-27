using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class ConversationManager : Singleton<ConversationManager>
{
    public TimelineHelper timelineHelper;
    private bool isEnd = false;
    public bool IsEnd { get => isEnd; set => isEnd = value; }
    public async UniTask Initialize(string conversationName)
    {
        CCHGameManager.Instance.SwitchActionMap("UI");

        if (timelineHelper == null)
        {
            timelineHelper = await AddressableManager.Instance.
                InstantiateAddressableAsync<TimelineHelper>("UI", "ConversationBox.prefab");
        }

        var playable = await AddressableManager.Instance.
            LoadAddressableAsync<PlayableAsset>("Timeline", conversationName);

        timelineHelper.Initialize(playable as TimelineAsset);
        var convBox = timelineHelper.GetComponent<ConversationBox>();
        timelineHelper.Add("Conversation Line", convBox);
        timelineHelper.Add("Activation Track", convBox.gameObject);
        timelineHelper.Binding();
        isEnd = false;
        timelineHelper.gameObject.SetActive(true);
        Play();
    }

    public void Pause()
    {
        timelineHelper.timeline.Pause();
    }

    public void Play()
    {
        if (timelineHelper.timeline.time >= timelineHelper.timeline.duration)
            End();
        else
            timelineHelper.timeline.Play();
    }

    public void End()
    {
        timelineHelper.gameObject.SetActive(false);
        CCHGameManager.Instance.SwitchActionMap("Player");
    }
}
