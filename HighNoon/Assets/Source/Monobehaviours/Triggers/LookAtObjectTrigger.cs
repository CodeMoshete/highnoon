using UnityEngine;

public class LookAtObjectTrigger : MonoBehaviour
{
    public Camera Camera;
    public CustomAction OnLookAt;
    public CustomAction OnLookAway;
    public CustomAction OnInteract;

    private bool isColliding;
    private int testLayer;

    public void Start()
    {
        testLayer = LayerMask.GetMask("Interactible");
    }

    public void Update ()
    {
        RaycastHit hit;
        if (Camera != null)
        {
            Ray ray = new Ray(Camera.transform.position, Camera.transform.forward);
            if (Physics.Raycast(ray, out hit, 5f, testLayer) && 
                hit.collider.gameObject == gameObject)
            {
                if (!isColliding)
                {
                    if (OnLookAt != null)
                    {
                        OnLookAt.Initiate();
                    }
                    isColliding = true;
                    Debug.Log("LookAt");
                }
            }
            else if (isColliding)
            {
                if (OnLookAway != null)
                {
                    OnLookAway.Initiate();
                }
                isColliding = false;
            }
        }

        if (OnInteract != null && isColliding)
        {
            bool isTriggerPressed = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger);
            if (isTriggerPressed || Input.GetKeyDown(KeyCode.I))
            {
                OnInteract.Initiate();
                Debug.Log("Interact");
            }
        }
    }

    public void OnDisable()
    {
        isColliding = false;
    }
}
