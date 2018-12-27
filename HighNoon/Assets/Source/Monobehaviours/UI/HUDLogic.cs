using Services;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HUDLogic : MonoBehaviour
{
    private string WIN_TEXT = "You Win!";
    private string LOSE_TEXT = "You Lose!";

    [SerializeField]
    private Text CountdownText;

    [SerializeField]
    private Text MessageText;

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
            onCountdownDone();
        }
    }

    public void ShowGameOver(bool win, Action onDone)
    {
        if (!isCountingDown)
        {
            CountdownText.text = string.Empty;
            MessageText.text = win ? WIN_TEXT : LOSE_TEXT;
            gameObject.SetActive(true);
            onCountdownDone = onDone;
            isCountingDown = true;
            Service.Timers.CreateTimer(5f, HideGameOverScreen, null);
        }
    }

    private void HideGameOverScreen(object cookie)
    {
        gameObject.SetActive(false);
        isCountingDown = false;
        onCountdownDone();
    }
}
