using UnityEngine.UIElements;

public class ToolkitLooping : VisualElement
{
    public new class UxmlFactory : UxmlFactory<ToolkitLooping, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlStringAttributeDescription loopStyleName =
            new UxmlStringAttributeDescription { name = "loopStyleName" };
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            var ate = ve as ToolkitLooping;
            ate.loopStyleName = loopStyleName.GetValueFromBag(bag, cc);
        }
    }

    public string loopStyleName { get; set; }

    private bool rewind = true;

    public ToolkitLooping() : base()
    {
        rewind = true;
    }

    public void LoopStart()
    {
        RegisterCallback<TransitionEndEvent>(PlayNext);
        PlayNext(null);
    }

    public void LoopStop()
    {
        UnregisterCallback<TransitionEndEvent>(PlayNext);
        RemoveFromClassList(loopStyleName);
        rewind = true;
    }

    public void PlayNext(TransitionEndEvent e)
    {
        if (rewind)
        {
            AddToClassList(loopStyleName);
            rewind = false;
        }
        else
        {
            RemoveFromClassList(loopStyleName);
            rewind = true;
        }
    }
}
