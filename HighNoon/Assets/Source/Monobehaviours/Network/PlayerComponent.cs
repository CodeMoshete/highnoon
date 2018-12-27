using Events;
using Photon.Pun;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : MonoBehaviourPun
{
    private const int BASE_HEALTH = 100;

    private readonly Dictionary<string, int> damageAmounts;

    public int Health { get; private set; }

    public PlayerComponent()
    {
        damageAmounts = new Dictionary<string, int>();
        damageAmounts.Add("LegShinLeft", 30);
        damageAmounts.Add("LegThighLeft", 40);
        damageAmounts.Add("LegShinRight", 30);
        damageAmounts.Add("LegThighRight", 40);
        damageAmounts.Add("Torso", 70);
        damageAmounts.Add("ElbowLeft", 20);
        damageAmounts.Add("ShoulderLeft", 30);
        damageAmounts.Add("ElbowRight", 20);
        damageAmounts.Add("ShoulderRight", 30);
        damageAmounts.Add("Head", 100);

        Health = BASE_HEALTH;
    }

    void Start ()
    {
        Health = BASE_HEALTH;
	}
    
    [PunRPC]
    public void ScoreHit(string limb)
    {
        if (damageAmounts.ContainsKey(limb))
        {
            Health -= damageAmounts[limb];
        }
        else
        {
            Health -= 20;
        }

        if (Health <= 0)
        {
            Service.Events.SendEvent(EventId.PlayerKilled, photonView.Owner.UserId);
        }

        Debug.Log("Health now at: " + Health);
    }
}
