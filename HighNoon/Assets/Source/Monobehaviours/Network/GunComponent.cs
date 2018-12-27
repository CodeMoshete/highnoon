﻿using Game.MonoBehaviors;
using Photon.Pun;
using Services;
using UnityEngine;

public class GunComponent : MonoBehaviourPun
{
    private const float HAMMER_THRESHOLD = 0.5f;

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

            if (isPullingHammer && pressEnded)
            {
                isPullingHammer = false;
                isReadyToFire = hammerStartPosition - primaryTouchpad.y > HAMMER_THRESHOLD;
            }

            bool isTriggerPressed = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger);
            if ((isTriggerPressed && isReadyToFire) || Input.GetKeyDown(KeyCode.F))
            {
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

        if (photonView.Owner.IsLocal)
        {
            RaycastHit hit;
            Ray ray = new Ray(Muzzle.position, Muzzle.forward);
            if (Physics.Raycast(ray, out hit, 25f, testLayer))
            {
                Debug.Log("Shot: " + hit.collider.name);
                PlayerComponent hitPlayer =
                    hit.collider.transform.parent.GetComponent<PlayerComponent>();

                string limb = hit.collider.gameObject.name;
                hitPlayer.photonView.RPC("ScoreHit", RpcTarget.Others, new object[] { limb });
            }
        }

        GameObject bulletTrail = Instantiate(Resources.Load<GameObject>("ProjectilePath"));
        bulletTrail.GetComponent<TimedDestroy>().Initialize(2f);
        bulletTrail.transform.position = Muzzle.position;
        bulletTrail.transform.rotation = Muzzle.rotation;
        bulletTrail.transform.localScale = new Vector3(1f, 1f, 300f);
    }
}
