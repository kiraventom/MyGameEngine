using System;

namespace GameEngine.GameObjects
{
	public interface IGameObject
	{
		public abstract string Name { get; }
		public string Description { get; }
	}
}
