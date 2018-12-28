using Controllers.Interfaces;
using Services;
using System;

public class OculusGoTransitionWrapper : ITransitionWrapper, IUpdateObserver
{
    private enum TransitionType
    {
        In,
        Out,
    }

    private OVRScreenFade screenFade;
    private float duration;
    private float timeLeft;
    private bool isTransitioning;
    private TransitionType transitionType;
    private Action onAnimationDone;

    public OculusGoTransitionWrapper()
    {
        Service.FrameUpdate.RegisterForUpdate(this);
        screenFade = Service.Rig.Eye.GetComponent<OVRScreenFade>();
    }

    public void PlayTransitionIn(float duration, Action onAnimationDone)
    {
        StartTransition(duration, onAnimationDone, TransitionType.In);
    }

    public void PlayTransitionOut(float duration, Action onAnimationDone)
    {
        StartTransition(duration, onAnimationDone, TransitionType.Out);
    }

    public void SetActive(bool active)
    {
        // Intentionally left empty.
    }

    private void StartTransition(float duration, Action onAnimationDone, TransitionType type)
    {
        if (!isTransitioning)
        {
            this.onAnimationDone = onAnimationDone;
            this.duration = duration;
            timeLeft = duration;
            transitionType = type;
            isTransitioning = true;
        }
    }

    public void Update(float dt)
    {
        if (isTransitioning)
        {
            timeLeft -= dt;
            if (timeLeft <= 0f)
            {
                timeLeft = 0f;
                isTransitioning = false;
                onAnimationDone();
            }

            float pct = timeLeft / duration;
            pct = transitionType == TransitionType.Out ? 1 - pct : pct;
            screenFade.SetFadeLevel(pct);
        }
    }
}
