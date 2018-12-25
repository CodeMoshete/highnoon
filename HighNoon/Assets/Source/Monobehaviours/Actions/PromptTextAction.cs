using Events;
using Services;

public class PromptTextAction : CustomAction
{
    public string PromptText;
    public float Duration;

    public override void Initiate()
    {
        PromptTextActionData evtData = new PromptTextActionData(PromptText, Duration);
        Service.Events.SendEvent(EventId.ShowPromptText, evtData);
    }
}
