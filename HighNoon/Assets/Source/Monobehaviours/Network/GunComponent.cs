using Game.MonoBehaviors;
using Photon.Pun;
using Services;
using UnityEngine;

public class GunComponent : MonoBehaviourPun
{
    private const float HAMMER_THRESHOLD = 0.5f;
    private const float HAMMER_MAX_TILT = 60f;

    public Transform Hammer;
    public Transform Trigger;
    public Transform Muzzle;

    private bool gameRunning;

    private bool isReadyToFire;
    private bool isPullingHammer;
    private float hammerStartPosition;
    private Transform gunLocator;

    private int testLayer;

    public void Start()
    {
        gunLocator = Service.Rig.Hand.transform;
        testLayer = LayerMask.GetMask("Limbs");
    }

    public void Init()
    {
        gameRunning = true;
    }

    public void Pause()
    {
        gameRunning = false;
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (gameRunning)
        {
            Vector2 primaryTouchpad = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
            bool pressStarted = OVRInput.GetDown(OVRInput.Button.PrimaryTouchpad);
            bool pressEnded = OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad);

            if (!isReadyToFire && pressStarted)
            {
                isPullingHammer = true;
                hammerStartPosition = primaryTouchpad.y;
            }

            if (isPullingHammer)
            {
                float dragAmt = hammerStartPosition - primaryTouchpad.y;
                float hammerTilt = Mathf.Clamp01(dragAmt * (1 / HAMMER_THRESHOLD)) * HAMMER_MAX_TILT;

                if (pressEnded)
                {
                    isPullingHammer = false;
                    isReadyToFire = dragAmt > HAMMER_THRESHOLD;
                    hammerTilt = isReadyToFire ? HAMMER_MAX_TILT : 0f;
                    if (isReadyToFire)
                    {
                        GameObject.Instantiate(Resources.Load<GameObject>("Audio/HammerClick"));
                    }
                }

                Hammer.localEulerAngles = new Vector3(-hammerTilt, 0f, 0f);
            }

            bool isTriggerPressed = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger);
            Trigger.localEulerAngles = new Vector3(isTriggerPressed ? 30f : 0f, 0f, 0f);

            if ((isTriggerPressed && isReadyToFire) || Input.GetKeyDown(KeyCode.F))
            {
                isReadyToFire = false;
                Hammer.localEulerAngles = Vector3.zero;
                photonView.RPC("Shoot", RpcTarget.All);
            }
        }

        transform.position = gunLocator.position;
        transform.rotation = gunLocator.rotation;
    }

    [PunRPC]
    public void Shoot()
    {
        Debug.Log("Bang!");
        GameObject.Instantiate(Resources.Load<GameObject>("Audio/Gunshot"));

        bool didHit = false;
        RaycastHit hit;
        Ray ray = new Ray(Muzzle.position, Muzzle.forward);
        if (Physics.Raycast(ray, out hit, 25f, testLayer))
        {
            didHit = true;
            if (photonView.Owner.IsLocal)
            {
                Debug.Log("Shot: " + hit.collider.name);
                PlayerComponent hitPlayer =
                    hit.collider.transform.parent.GetComponent<PlayerComponent>();

                string limb = hit.collider.gameObject.name;
                hitPlayer.photonView.RPC("ScoreHit", RpcTarget.All, new object[] { limb });
            }
        }

        string resourcePath = didHit ? "ProjectilePathHit" : "ProjectilePath";
        GameObject bulletTrail = Instantiate(Resources.Load<GameObject>(resourcePath));
        bulletTrail.GetComponent<TimedDestroy>().Initialize(2f);
        bulletTrail.transform.position = Muzzle.position;
        bulletTrail.transform.rotation = Muzzle.rotation;
        bulletTrail.transform.localScale = new Vector3(1f, 1f, 300f);
    }
}
