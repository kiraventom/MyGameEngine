using GameEngine.GameObjects.Actors;
using System;

namespace GameEngine.GameObjects.Usables
{
	public interface IUsable : IGameObject
	{
		public uint MinPower { get; }
		public uint MaxPower { get; }

		internal virtual Action<Actor, IGameObject, uint> BasicEffect => null;
		internal void Use(Actor user, IGameObject usedAt);
	}
}
