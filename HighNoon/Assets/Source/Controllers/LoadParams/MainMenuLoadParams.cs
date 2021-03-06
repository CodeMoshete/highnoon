//MainMenuLoadParams.cs
//Struct-like class for enforcing and transporting parameters to a scene controller

using System;

namespace Controllers.Types
{
	public class MainMenuLoadParams
	{
		// Eventually we will pass our selected ship through to the next state.
		public Action OnGameStart { get; private set; }

		public MainMenuLoadParams (Action onGameStart)
		{
			OnGameStart = onGameStart;
		}
	}
}

