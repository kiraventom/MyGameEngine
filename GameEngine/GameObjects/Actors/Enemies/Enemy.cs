using GameEngine.Balance;
using GameEngine.GameObjects.Usables.Abilities;
using System.Collections.Generic;

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
