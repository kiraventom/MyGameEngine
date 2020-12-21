﻿namespace GameEngine.GameObjects.Usables.Items.Consumables
{
	public class Apple : Consumable, IHealing
	{
		public override string Name => "Яблоко";

		public override uint MinPower => 5;
		public override uint MaxPower => 5;
	}
}
