using UnityEngine;

public class SetGameObjectRotationAction : CustomAction
{
    public Transform Target;
    public Vector3 EulerAngles;

    public override void Initiate()
    {
        Target.eulerAngles = EulerAngles;
    }
}
