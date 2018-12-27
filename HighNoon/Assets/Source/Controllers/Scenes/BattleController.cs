//MainMenuController.cs
//Controller class for the main menu. Essentially a game state.

using Controllers.Types;
using Events;
using Game.Controllers.Interfaces;
using Photon.Pun;
using Services;
using System;
using UnityEngine;
using Utils;

namespace Controllers
{
    public class BattleController : IStateController
    {
        private const string BATTLE_SCENERY_ID = "DuelScenery";
        private const string HUD_ID = "GUI/Gui_DuelStartScreen";
        private const string START_BUTTON_ID = "StartButton";
        private const string PLAYER_1_POS = "Player1Position";
        private const string PLAYER_2_POS = "Player2Position";

        private GameObject battleScenery;
        private HUDLogic hudScreen;
        private SceneLoadedCallback m_onSceneLoaded;
        private Action onLoadBattle;
        private Action onBattleEnd;

        private GameObject playerWeapon;
        private GameObject playerView;

        public void Load(SceneLoadedCallback onLoadedCallback, object passedParams)
        {
            MainMenuLoadParams loadParams = (MainMenuLoadParams)passedParams;
            onBattleEnd = loadParams.OnGameStart;

            battleScenery = GameObject.Instantiate(Resources.Load<GameObject>(BATTLE_SCENERY_ID));
            hudScreen =
                GameObject.Instantiate(Resources.Load<GameObject>(HUD_ID)).GetComponent<HUDLogic>();

            Service.Network.Connect(OnConnected);

            m_onSceneLoaded = onLoadedCallback;
            m_onSceneLoaded();
        }

        private void OnConnected()
        {
            Debug.Log("Joined Room");

            string startPosName = Service.Network.IsMaster ? PLAYER_1_POS : PLAYER_2_POS;
            Service.Rig.Body.transform.position =
                UnityUtils.FindGameObject(battleScenery, startPosName).transform.position;

            float startRotation = Service.Network.IsMaster ? 180f : 0f;
            Service.Rig.Body.transform.eulerAngles = new Vector3(0f, startRotation, 0f);

            playerView = PhotonNetwork.Instantiate(
                "PlayerView",
                Service.Rig.Body.transform.position,
                Service.Rig.Body.transform.rotation);
            playerView.SetActive(false);

            playerWeapon = PhotonNetwork.Instantiate(
                "GunModel",
                Service.Rig.Hand.transform.position,
                Service.Rig.Hand.transform.rotation);

            if (PhotonNetwork.PlayerList.Length > 1)
            {
                StartCountdown(null);
            }
            else
            {
                Service.Events.AddListener(EventId.NetPlayerConnected, StartCountdown);
            }
        }

        private void StartCountdown(object cookie)
        {
            hudScreen.ShowCountdown(3, StartGame);
        }

        private void StartGame()
        {
            playerWeapon.GetComponent<GunComponent>().Init();
            Service.Events.AddListener(EventId.PlayerKilled, OnPlayerKilled);
        }

        private void OnPlayerKilled(object cookie)
        {
            bool localPlayerWon = (string)cookie != PhotonNetwork.LocalPlayer.UserId;
            hudScreen.ShowGameOver(localPlayerWon, OnBattleDone);
        }

        private void OnBattleDone()
        {
            onBattleEnd();
        }

        public void Start()
        {
            Service.Rig.Body.transform.position =
                UnityUtils.FindGameObject(battleScenery, PLAYER_1_POS).transform.position;

            Service.Rig.Body.transform.eulerAngles = new Vector3(0f, 180f, 0f);

            battleScenery.SetActive(true);
        }

        public void Unload()
        {
            GameObject.Destroy(battleScenery);
            battleScenery = null;
            GameObject.Destroy(hudScreen.gameObject);
            hudScreen = null;
            PhotonNetwork.Destroy(playerView);
            playerView = null;
            PhotonNetwork.Destroy(playerWeapon);
            playerWeapon = null;
            Service.Network.Disconnect();
        }
    }
}

