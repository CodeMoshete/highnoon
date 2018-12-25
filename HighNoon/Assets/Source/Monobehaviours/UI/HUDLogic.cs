using Events;
using Services;
using UnityEngine;
using UnityEngine.UI;

public class HUDLogic : MonoBehaviour
{
    public GameObject TriggerPressContainer;

    public Text TextPromptContainer;
    public Animator TextPromptAnimator;

    public void Start()
    {
        Service.Events.AddListener(EventId.ShowTriggerPrompt, ShowTriggerPress);
        Service.Events.AddListener(EventId.ShowPromptText, ShowPromptText);
        Service.Events.AddListener(EventId.HidePromptText, HidePromptTextFromEvent);
    }

    public void ShowTriggerPress(object cookie)
    {
        bool show = (bool)cookie;
        TriggerPressContainer.SetActive(show);
    }

    public void ShowPromptText(object cookie)
    {
        PromptTextActionData promptData = (PromptTextActionData)cookie;
        TextPromptContainer.text = promptData.Prompt;
        TextPromptAnimator.SetBool("IsVisible", true);
        if (promptData.Duration > 0)
        {
            Service.Timers.CreateTimer(promptData.Duration, HidePromptTextFromTimer, null);
        }
    }

    public void HidePromptTextFromEvent(object cookie)
    {
        TextPromptAnimator.SetBool("IsVisible", false);
    }

    public void HidePromptTextFromTimer(object cookie)
    {
        TextPromptAnimator.SetBool("IsVisible", false);
    }
}
