using GameEngine.GameObjects.Usables.Abilities;

namespace GameEngine.GameObjects.Actors.Enemies
{
	public abstract class Enemy : Actor
	{
		protected Enemy() : base()
		{
			Ability = new EmptyAbility();
		}

		public virtual Ability Ability { get; }
	}
}
