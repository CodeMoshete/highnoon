// StateFactory.cs
// Controller class for Loading and Unloading game scenes

using Game.Controllers.Interfaces;
using GameObjectWrappers;
using UnityEngine;

namespace Game.Factories
{
    public class StateFactory
	{
        private const float TRANSITION_DURATION = 2f;
		private const string TRANSITION_SCREEN_ID = "GUI/TransitionScreen";

		private IStateController newScene;
		private IStateController currentScene;

		private SceneLoadedCallback onNewSceneLoaded;

		private bool isTransitionDone;
		private bool isSceneLoaded;

		private ITransitionWrapper m_transitionScreen;

		public StateFactory()
		{
            //Don't show transition screen if it's our first load.
            isTransitionDone = true;
			isSceneLoaded = false;

			GameObject transitionScreen = 
				GameObject.Instantiate(Resources.Load<GameObject>(TRANSITION_SCREEN_ID)) as GameObject;

#if UNITY_ANDROID
            m_transitionScreen = new OculusGoTransitionWrapper();
#else
            m_transitionScreen = new TransitionWrapper(transitionScreen);
#endif
        }

		public void LoadScene<T>(SceneLoadedCallback callback, object passedParams) where T : IStateController, new()
		{
			onNewSceneLoaded = callback;

			//Don't show transition screen if it's our first load.
			if(!isTransitionDone)
			{
				m_transitionScreen.SetActive(true);
				m_transitionScreen.PlayTransitionOut(TRANSITION_DURATION, onTransitionShown);
			}

			newScene = new T();
			newScene.Load(onSceneLoaded, passedParams);
		}

		private void onTransitionShown()
		{
			isTransitionDone = true;
			if(isSceneLoaded)
			{
				onSceneReady();
			}
		}

		private void OnTransitionHidden()
		{
			m_transitionScreen.SetActive(false);
		}

		private void onSceneLoaded()
		{
			isSceneLoaded = true;
			if(isTransitionDone)
			{
				onSceneReady();
			}
		}

		public void onSceneReady()
		{
			isTransitionDone = isSceneLoaded = false;

			if(currentScene != null)
			{
				currentScene.Unload();
			}

			currentScene = newScene;
			currentScene.Start();
			m_transitionScreen.PlayTransitionIn(TRANSITION_DURATION, OnTransitionHidden);

			if(onNewSceneLoaded != null)
			{
				onNewSceneLoaded();
			}
		}

		public void StartScene<T>() where T : IStateController
		{
			if(currentScene is T)
			{
				currentScene.Start();
			}
		}
	}
}
