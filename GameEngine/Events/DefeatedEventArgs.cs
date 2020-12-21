using GameEngine.GameObjects;
using GameEngine.GameObjects.Actors;
using System;

namespace GameEngine.Events
{
	public class DefeatedEventArgs : EventArgs
	{
		public DefeatedEventArgs(Actor dead, IGameObject killer)
		{
			Dead = dead;
			Killer = killer;
		}

		public Actor Dead { get; }
		public IGameObject Killer { get; }
	}
}
