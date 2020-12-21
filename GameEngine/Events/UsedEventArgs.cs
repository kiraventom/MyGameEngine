using GameEngine.GameObjects;
using GameEngine.GameObjects.Actors;
using GameEngine.GameObjects.Usables;
using System;

namespace GameEngine.Events
{
	public class UsedEventArgs : EventArgs
	{
		public UsedEventArgs(Actor user, IGameObject usedAt, Usable usable)
		{
			this.User = user;
			this.UsedAt = usedAt;
			this.Usable = usable;
		}

		public Actor User { get; }
		public IGameObject UsedAt { get; }
		public Usable Usable { get; }
	}
}
