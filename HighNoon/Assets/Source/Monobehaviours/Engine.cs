//Engine.cs
//Description: This serves as the entry point for the program

using UnityEngine;
using System.Collections;
using Controllers;
using Game.Controllers;
using Services;
using Events;

public class Engine : MonoBehaviour
{
	private FlowController gameFlow;
	private DebugCameraController debugCam;

	public void Start ()
	{
        gameObject.AddComponent<NetService>();
        SpawnPlayer();

		gameFlow = new FlowController();
		gameFlow.StartGame();
		debugCam = new DebugCameraController ();
	}

    private void SpawnPlayer()
    {
        CameraRigService rigService = new CameraRigService(
            Instantiate(Resources.Load<GameObject>("OVRPlayerController")),
            Instantiate(Resources.Load<GameObject>("TrackedRemote")),
            Instantiate(Resources.Load<GameObject>("OVRCameraRigBackground")),
            0.001f);
    }

	public void Update()
	{
		Service.FrameUpdate.Update(this);
	}

	public void OnApplicationQuit()
	{
		Service.Events.SendEvent (EventId.ApplicationExit, null);
	}
}
