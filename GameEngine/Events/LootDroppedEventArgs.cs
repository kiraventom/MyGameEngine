using GameEngine.GameObjects.Actors;
using GameEngine.GameObjects.Usables.Items;
using System;
using System.Collections.Generic;

namespace GameEngine.Events
{
	public class LootDroppedEventArgs : EventArgs
	{
		public LootDroppedEventArgs(Actor dropper, IEnumerable<Item> loot)
		{
			this.Dropper = dropper;
			this.Loot = loot;
		}

		public Actor Dropper { get; }
		public IEnumerable<Item> Loot { get; }
	}
}
