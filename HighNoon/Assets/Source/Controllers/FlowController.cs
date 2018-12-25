//FlowController
//Controller class that strings any and all game states together.

using Controllers;
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
			sceneFactory.LoadScene<MainMenuController>(OnSceneLoaded, null);
		}

		public void OnSceneLoaded()
		{
			// Intentionally empty for now...
		}
	}
}

