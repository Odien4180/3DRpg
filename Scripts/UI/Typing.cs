using UnityEngine;
using UnityEngine.Playables;

public class Typing : PlayableBehaviour
{
    private ConversationBox convBox;

    public ConversationData convData;
    private string checkText;

    private bool started = false;
    // called every frame the clip is active
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        convBox = playerData as ConversationBox;
        if (convBox == null)
            return;

        // given the current time, determine how much of the string will be displayed
        var progress = (float)(playable.GetTime() / playable.GetDuration());
        var subStringLength = Mathf.RoundToInt(Mathf.Clamp01(progress) * convData.line.Length);

        checkText = convData.line.Substring(0, subStringLength);
        char[] line = checkText.ToCharArray();

        convBox.lineText.text = checkText;
        convBox.nameText.text = convData.name;
    }

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        started = true;
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (started)
        {
            ConversationManager.Instance.Pause();
            started = false;
        }
    }


}
