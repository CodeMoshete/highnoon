using Controllers.Interfaces;
using Events;
using Services;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class OculusGoControls : IUpdateObserver
{
    public Text ConsoleLine;
	public float Sensitivity = 30f;

	private OVRPlayerController bodyObject;
	private Transform cameraObject;
	private Vector3 lastMousePos;

	private bool isMouseCameraActive;
    private bool isDebugUiActive;

    private FrameTimeUpdateService updateManager;

	public OculusGoControls()
    {
#if UNITY_EDITOR
        isMouseCameraActive = true;
#endif

        bodyObject = Service.Rig.Body.GetComponent<OVRPlayerController>();
		cameraObject = UnityUtils.FindGameObject(bodyObject.gameObject, "TrackingSpace").transform;

        updateManager = Service.FrameUpdate;
        updateManager.RegisterForUpdate(this);
    }
	
    public void Destroy()
    {
        updateManager.UnregisterForUpdate(this);
    }

	public void Update (float dt) 
	{
		Vector2 primaryTouchpad = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
		bool isPressed = OVRInput.Get (OVRInput.Button.PrimaryTouchpad);

        Vector3 euler = bodyObject.transform.eulerAngles;

        bool isTriggerPressed = OVRInput.GetDown (OVRInput.Button.PrimaryIndexTrigger);
        if (Input.GetKeyDown(KeyCode.U) || OVRInput.GetDown(OVRInput.Button.Back))
        {
            isDebugUiActive = !isDebugUiActive;
            Service.Events.SendEvent(EventId.DebugUiActivated, isDebugUiActive);
		}

		if (isMouseCameraActive)
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
					euler = bodyObject.transform.eulerAngles;
					euler.y += dt * -mouseDelta.x * Sensitivity;
					bodyObject.transform.eulerAngles = euler;

					euler = cameraObject.eulerAngles;
					euler.x += dt * mouseDelta.y * Sensitivity;
					cameraObject.eulerAngles = euler;
				}
		}

    }

    private void Log(string message)
    {
        if (ConsoleLine != null)
        {
            ConsoleLine.text = message;
        }
        Debug.Log(message);
    }
}
