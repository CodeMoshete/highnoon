//MainMenuController.cs
//Controller class for the main menu. Essentially a game state.

using Game.Controllers.Interfaces;
using UnityEngine;
using Controllers.Interfaces;
using Services;
using Utils;
using Events;
using Photon.Pun;

namespace Controllers
{
	public class MainMenuController : IStateController, IUpdateObserver
	{
		private const string MAIN_MENU_ID = "MainMenuScenery";
		private const string CUBE_ID = "Models/Cube2";

		private GameObject m_mainMenu;
        private SceneLoadedCallback m_onSceneLoaded;

		public void Load(SceneLoadedCallback onLoadedCallback, object passedParams)
		{
            m_mainMenu = GameObject.Instantiate(Resources.Load<GameObject>(MAIN_MENU_ID));

            GameObject cubeObj = UnityUtils.FindGameObject(m_mainMenu, "Cube");
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
            GameObject playerView = PhotonNetwork.Instantiate(
                "PlayerView", 
                Service.Rig.Body.transform.position,
                Service.Rig.Body.transform.rotation);
        }

		public void Start()
		{
            Service.Rig.Body.transform.position = new Vector3(0f, 1f, 0f);
			m_mainMenu.SetActive(true);
			Service.FrameUpdate.RegisterForUpdate(this);
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

