//MainMenuController.cs
//Controller class for the main menu. Essentially a game state.

using Controllers.Interfaces;
using Events;
using Game.Controllers.Interfaces;
using Photon.Pun;
using Services;
using UnityEngine;
using Utils;

namespace Controllers
{
    public class MainMenuController : IStateController, IUpdateObserver
	{
		private const string MAIN_MENU_ID = "DuelScenery";
        private const string START_BUTTON_ID = "StartButton";
        private const string PLAYER_1_POS = "Player1Position";
        private const string PLAYER_2_POS = "Player2Position";

        private GameObject m_mainMenu;
        private SceneLoadedCallback m_onSceneLoaded;

        private GameObject playerWeapon;

		public void Load(SceneLoadedCallback onLoadedCallback, object passedParams)
		{
            m_mainMenu = GameObject.Instantiate(Resources.Load<GameObject>(MAIN_MENU_ID));

            GameObject cubeObj = UnityUtils.FindGameObject(m_mainMenu, START_BUTTON_ID);
            cubeObj.GetComponent<LookAtObjectTrigger>().Camera = 
                Service.Rig.Eye.GetComponent<Camera>();

            Service.Events.AddListener(EventId.GameStartTriggered, ConnectToRoom);


            m_onSceneLoaded = onLoadedCallback;
            m_onSceneLoaded();
        }

        private void ConnectToRoom(object cookie)
        {
            Service.Events.RemoveListener(EventId.GameStartTriggered, ConnectToRoom);
            Service.Network.Connect(OnConnected);
        }

        private void OnConnected()
        {
            Debug.Log("Joined Room");

            string startPosName = Service.Network.IsMaster ? PLAYER_1_POS : PLAYER_2_POS;
            Service.Rig.Body.transform.position = 
                UnityUtils.FindGameObject(m_mainMenu, startPosName).transform.position;

            float startRotation = Service.Network.IsMaster ? 180f : 0f;
            Service.Rig.Body.transform.eulerAngles = new Vector3(0f, startRotation, 0f);

            GameObject playerView = PhotonNetwork.Instantiate(
                "PlayerView", 
                Service.Rig.Body.transform.position,
                Service.Rig.Body.transform.rotation);

            playerWeapon = PhotonNetwork.Instantiate(
                "GunModel",
                Service.Rig.Hand.transform.position,
                Service.Rig.Hand.transform.rotation);

            if (PhotonNetwork.PlayerList.Length > 1)
            {
                InitGame(null);
            }
            else
            {
                Service.Events.AddListener(EventId.NetPlayerConnected, InitGame);
            }
        }

        private void InitGame(object cookie)
        {
            playerWeapon.GetComponent<GunComponent>().Init();
        }

		public void Start()
		{
            Service.Rig.Body.transform.position =
                UnityUtils.FindGameObject(m_mainMenu, PLAYER_1_POS).transform.position;

            Service.Rig.Body.transform.eulerAngles = new Vector3(0f, 180f, 0f);

            m_mainMenu.SetActive(true);
			Service.FrameUpdate.RegisterForUpdate(this);
		}

        private void StartRound()
        {

        }

		private void StartClicked()
		{
			GameObject.Destroy(m_mainMenu);
		}

		public void Update(float dt)
		{
			//Put update code in here.
		}

		public void Unload()
		{
			Service.FrameUpdate.UnregisterForUpdate(this);
			GameObject.Destroy(m_mainMenu);
			m_mainMenu = null;
		}
	}
}

