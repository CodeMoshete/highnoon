using UnityEngine;

public class FadeScreenOutAction : CustomAction
{
    public OVRScreenFade ScreenFade;
    public float Delay;
    public float Duration;
    private float totalDuration;
    private bool isInitialized;

    public override void Initiate()
    {
        if (ScreenFade != null)
        {
            totalDuration = Duration;
            isInitialized = true;
        }
    }

    public void Update()
    {
        if (isInitialized)
        {
            float dt = Time.deltaTime;
            if (Delay > 0)
            {
                Delay -= dt;
                return;
            }

            Duration -= dt;
            if (Duration <= 0f)
            {
                Duration = 0f;
                isInitialized = false;
            }

            float pct = 1f - (Duration / totalDuration);
            ScreenFade.SetFadeLevel(pct);
        }
    }
}
