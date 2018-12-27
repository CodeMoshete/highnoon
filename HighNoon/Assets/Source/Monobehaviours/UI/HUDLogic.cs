using Services;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HUDLogic : MonoBehaviour
{
    [SerializeField]
    private Text CountdownText;

    private int numSeconds;
    private bool isCountingDown;
    private Action onCountdownDone;

    public void ShowCountdown(int secondsToCount, Action onDone)
    {
        if (!isCountingDown)
        {
            gameObject.SetActive(true);
            isCountingDown = true;
            onCountdownDone = onDone;
            numSeconds = secondsToCount;
            CheckTime(null);
        }
    }

    private void CheckTime(object cookie)
    {
        if (numSeconds > 0)
        {
            CountdownText.text = numSeconds.ToString();
            numSeconds--;
            Service.Timers.CreateTimer(1f, CheckTime, null);
        }
        else
        {
            isCountingDown = false;
            gameObject.SetActive(false);
        }
    }
}
