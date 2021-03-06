﻿using GameEngine.GameObjects.Usables.Abilities;

namespace GameEngine.GameObjects.Actors.Enemies
{
	public class Demon : Enemy
	{
		public Demon() : base()
		{
			DemonAbility = new DemonLifestealAbility();
		}

		// Overrides
		public override string Name => "Демон";
		public override string Description => "Монстр из самых недр ада, высасывающий жизнь каждой атакой. Погоди, вроде же вампиры должны так делать?";
		public override Ability Ability => DemonAbility;

		// Properties
		private DemonLifestealAbility DemonAbility { get; }

		//Methods
		internal override void Attack(Actor defender)
		{
			var strenght = this.Stats.GetStrenght();
			this.Attack(defender, strenght);
			DemonAbility.Use(this, this, strenght);
		}
	}
}
