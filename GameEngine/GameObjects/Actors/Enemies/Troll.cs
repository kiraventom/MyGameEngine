﻿using GameEngine.Balance;
using GameEngine.GameObjects.Usables.Abilities;

namespace GameEngine.GameObjects.Actors.Enemies
{
	public class Troll : Enemy
	{
		public Troll() : base()
		{
			Ability = new TrollRegenAbility();
		}

		public override string Name => "Тролль";
		public override Ability Ability { get; }

		internal override void ReceiveDamage(uint amount, IGameObject source)
		{
			base.ReceiveDamage(amount, source);
			Regenerate();
		}

		private void Regenerate()
		{
			if (IsAlive)
			{
				Ability.Use(this, this);
			}
		}
	}
}
