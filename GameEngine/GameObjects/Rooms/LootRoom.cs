using GameEngine.Balance;
using GameEngine.GameObjects.Usables.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.GameObjects.Rooms
{
	public class LootRoom : Room
	{
		public LootRoom()
		{
			_loot = Balancer.CreateRandomObjects<Item>(0, 3).ToList();
		}

		public override string Name => "Комната с сундуком";

		public event EventHandler<Events.LootedEventArgs> Looted;

		public IReadOnlyList<Item> TakeLoot()
		{
			Looted.Invoke(this, new Events.LootedEventArgs(this));
			var loot = new List<Item>(_loot);
			_loot = null;
			return loot.AsReadOnly();
		}

		private List<Item> _loot;
		public IReadOnlyList<Item> Loot => _loot;
	}
}
