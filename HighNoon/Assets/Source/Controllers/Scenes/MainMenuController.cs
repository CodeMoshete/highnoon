//MainMenuController.cs
//Controller class for the main menu. Essentially a game state.

using Controllers.Types;
using Events;
using Game.Controllers.Interfaces;
using Services;
using System;
using UnityEngine;
using Utils;

namespace Controllers
{
    public class MainMenuController : IStateController
	{
		private const string MAIN_MENU_ID = "MainMenuScenery";
        private const string START_BUTTON_ID = "Cube";

        private GameObject m_mainMenu;
        private SceneLoadedCallback m_onSceneLoaded;
        private Action onBattleStart;

		public void Load(SceneLoadedCallback onLoadedCallback, object passedParams)
		{
            MainMenuLoadParams loadParams = (MainMenuLoadParams)passedParams;
            onBattleStart = loadParams.OnGameStart;

            m_mainMenu = GameObject.Instantiate(Resources.Load<GameObject>(MAIN_MENU_ID));

            GameObject cubeObj = UnityUtils.FindGameObject(m_mainMenu, START_BUTTON_ID);
            cubeObj.GetComponent<LookAtObjectTrigger>().Camera = 
                Service.Rig.Eye.GetComponent<Camera>();

            Service.Events.AddListener(EventId.GameStartTriggered, SwitchToBattleState);

            m_onSceneLoaded = onLoadedCallback;
            m_onSceneLoaded();
        }

        private void SwitchToBattleState(object cookie)
        {
            Service.Events.RemoveListener(EventId.GameStartTriggered, SwitchToBattleState);
            onBattleStart();
        }

		public void Start()
		{
            m_mainMenu.SetActive(true);
		}

		public void Unload()
		{
			GameObject.Destroy(m_mainMenu);
			m_mainMenu = null;
		}
	}
}

