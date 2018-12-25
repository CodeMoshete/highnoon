using Events;
using Services;

public class SendStringEvent : CustomAction
{
    public EventId Event;
    public string Value;

    public override void Initiate()
    {
        Service.Events.SendEvent(Event, Value);
    }
}
