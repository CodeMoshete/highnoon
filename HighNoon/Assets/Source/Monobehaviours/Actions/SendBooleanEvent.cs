using Events;
using Services;

public class SendBooleanEvent : CustomAction
{
    public EventId Event;
    public bool Value;

    public override void Initiate()
    {
        Service.Events.SendEvent(Event, Value);
    }
}
