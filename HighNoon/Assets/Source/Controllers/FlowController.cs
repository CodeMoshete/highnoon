//FlowController
//Controller class that strings any and all game states together.

using Controllers;
using Controllers.Types;
using Game.Factories;

namespace Game.Controllers
{
	public class FlowController
	{
		private StateFactory sceneFactory;

		public FlowController()
		{
			sceneFactory = new StateFactory();
		}

		public void StartGame()
		{
			LoadMainMenu();
		}

		public void LoadMainMenu()
		{
            MainMenuLoadParams loadParams = new MainMenuLoadParams(StartBattle);
			sceneFactory.LoadScene<MainMenuController>(OnSceneLoaded, loadParams);
		}

        public void StartBattle()
        {
            MainMenuLoadParams loadParams = new MainMenuLoadParams(LoadMainMenu);
            sceneFactory.LoadScene<BattleController>(OnSceneLoaded, loadParams);
        }

		public void OnSceneLoaded()
		{
			// Intentionally empty for now...
		}
	}
}

