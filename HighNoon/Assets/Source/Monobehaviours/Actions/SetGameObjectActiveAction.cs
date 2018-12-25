using System.Collections.Generic;
using UnityEngine;

public class SetGameObjectActiveAction : CustomAction
{
    public List<GameObject> Targets;
    public bool SetActive;

    public override void Initiate()
    {
        for (int i = 0, count = Targets.Count; i < count; ++i)
        {
            Targets[i].SetActive(SetActive);
        }
    }
}
