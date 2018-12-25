using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class DebugMouseControls : MonoBehaviour 
{
	public float Sensitivity = 1f;

	private Transform bodyObject;
	private Transform cameraObject;
	private Vector3 lastMousePos;

	private bool isActive;

	void Start () 
	{
		bodyObject = GameObject.Find ("OVRPlayerController").transform;
		cameraObject = UnityUtils.FindGameObject(bodyObject.gameObject, "OVRCameraRig").transform;
		lastMousePos = Input.mousePosition;
	}
	
	void Update () 
	{
		if (Input.GetKey (KeyCode.LeftControl) && Input.GetKeyDown (KeyCode.D))
		{
			isActive = !isActive;
		}

		if (isActive)
		{
			if (Input.GetMouseButtonDown (1))
			{
				lastMousePos = Input.mousePosition;
			}
			else
			if (Input.GetMouseButton (1))
			{
				Vector3 mouseDelta = lastMousePos - Input.mousePosition;
				lastMousePos = Input.mousePosition;
				float dt = Time.deltaTime;
				Vector3 euler = bodyObject.eulerAngles;
				euler.y += dt * -mouseDelta.x * Sensitivity;
				bodyObject.eulerAngles = euler;

				euler = cameraObject.eulerAngles;
				euler.x += dt * mouseDelta.y * Sensitivity;
				cameraObject.eulerAngles = euler;
			}
		}
	}
}
