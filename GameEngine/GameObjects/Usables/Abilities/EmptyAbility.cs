using GameEngine.GameObjects.Actors;
using System;

namespace GameEngine.GameObjects.Usables.Abilities
{
	public class EmptyAbility : Ability
	{
		internal EmptyAbility() { }

		public override string Name => "Нет способности";

		public override uint MinPower => 0;
		public override uint MaxPower => 0;
	}
}
