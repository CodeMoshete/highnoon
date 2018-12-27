using Events;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsoleLogic : MonoBehaviour
{
    [SerializeField]
    private Text debugField;
    private string output;

	void Awake ()
    {
        Service.Events.AddListener(EventId.DebugUiActivated, ToggleDebugUi);
        Application.logMessageReceived += HandleLog;
        gameObject.SetActive(false);
	}

	private void ToggleDebugUi(object cookie)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        output += logString + "\n";
        debugField.text = output;
        //stack = stackTrace;
    }
}
