using GameEngine.GameObjects;
using GameEngine.GameObjects.Actors;
using System;

namespace GameEngine.Events
{
	public class HealthChangedEventArgs : EventArgs
	{
		public HealthChangedEventArgs(int change, Actor actor, IGameObject source)
		{
			Change = change;
			Actor = actor;
			Source = source;
		}

		public int Change { get; }
		public Actor Actor { get; }
		public IGameObject Source { get; }
		public Type ChangeType => Change switch { > 0 => Type.Heal, < 0 => Type.Damage, _ => Type.None };

		public enum Type { None, Damage, Heal }
	}
}
