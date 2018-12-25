using Controllers.Interfaces;
using Services;
using UnityEngine;
using Utils;

public class CameraRigService : IUpdateObserver
{
    public GameObject Body { get; private set; }
    public GameObject Head { get; private set; }
    public GameObject HeadBackground { get; private set; }
    public GameObject Eye { get; private set; }
    public GameObject Hand { get; private set; }

    private float backgroundScale;
    private Vector3 lastPlayerPosition;
    private Transform cameraObject;

    private OculusGoControls controls;

    public CameraRigService(
        GameObject baseRig, 
        GameObject controller = null, 
        GameObject headBg = null,
        float headBgScale = 1f)
    {
        Body = baseRig;
        Head = UnityUtils.FindGameObject(Body, "OVRCameraRig");
        HeadBackground = headBg;
        Eye = UnityUtils.FindGameObject(Head, "CenterEyeAnchor");
        Hand = controller;
        GameObject handAnchor = UnityUtils.FindGameObject(Head, "RightControllerAnchor");
        Hand.transform.SetParent(handAnchor.transform);

        backgroundScale = headBgScale;
        HeadBackground.transform.localScale = 
            new Vector3(backgroundScale, backgroundScale, backgroundScale);
        cameraObject = UnityUtils.FindGameObject(Body, "TrackingSpace").transform;
        lastPlayerPosition = cameraObject.transform.position;

        Service.Rig = this;

        controls = new OculusGoControls();

        Service.FrameUpdate.RegisterForUpdate(this);
    }

    public void Update(float dt)
    {
        Vector3 moveDist =
                (cameraObject.position - lastPlayerPosition) * backgroundScale;
        HeadBackground.transform.localPosition += moveDist;
        HeadBackground.transform.localRotation = cameraObject.transform.rotation;
        lastPlayerPosition = cameraObject.transform.position;
    }
}
