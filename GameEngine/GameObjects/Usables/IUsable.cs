using GameEngine.GameObjects.Actors;

namespace GameEngine.GameObjects.Usables
{
	public interface IUsable : IGameObject
	{
		public uint MinPower { get; }
		public uint MaxPower { get; }

		internal void Use(Actor user, IGameObject usedAt);
	}
}
