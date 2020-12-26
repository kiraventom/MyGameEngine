using GameEngine.Events;
using GameEngine.GameObjects.Usables.Abilities;
using System;

namespace GameEngine.GameObjects.Actors.Enemies
{
	public abstract class Enemy : Actor
	{
		protected Enemy() : base()
		{
			Ability = new EmptyAbility();
			this.Defeated += this.Enemy_Defeated;
		}

		private void Enemy_Defeated(object sender, DefeatedEventArgs e)
		{
			LootDropped.Invoke(this, new LootDroppedEventArgs(this, this.GetInventory()));
			var allItems = this.GetInventory();
			foreach (var item in allItems)
			{
				this.Inventory.Remove(item);
			}
		}

		public virtual Ability Ability { get; }

		public event EventHandler<LootDroppedEventArgs> LootDropped;
	}
}
