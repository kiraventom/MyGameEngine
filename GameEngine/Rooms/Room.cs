using GameEngine.Balance.Tables;
using GameEngine.GameObjects.Actors;
using GameEngine.GameObjects.Actors.Enemies;
using GameEngine.GameObjects.Usables.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameEngine.Rooms
{
	public class Room
	{
		private Room(Enemy enemy = null, IEnumerable<Item> loot = null)
		{
			Enemy = enemy;
			if (Enemy is not null)
			{
				Enemy.LootDropped += this.Enemy_LootDropped;
				Actor.AcquireRoomToActor(this, Enemy);
			}

			_loot = new List<Item>(loot ?? Enumerable.Empty<Item>());
		}

		[Flags]
		internal enum RoomContents { None = 0b00, HasEnemy = 0b01, HasLoot = 0b10, HasEnemyAndLoot = 0b11 } 

		public Room Next { get; private set; }
		public Room Previous { get; private set; }
		public Enemy Enemy { get; }
		public IReadOnlyList<Item> Loot => _loot;
		public bool HasLoot => this.Loot is not null && this.Loot.Any();
		public bool HasEnemy => this.Enemy is not null && this.Enemy.IsAlive;
		internal bool IsEntrance => Previous is null;
		private List<Item> _loot;

		public event EventHandler<Events.LootedEventArgs> Looted;

		public static uint GetDepth(Room room)
		{
			uint depth = 0;
			while (room.Previous is not null)
			{
				room = room.Previous;
				++depth;
			}

			return depth;
		}

		internal static void LinkRooms(Room first, Room second)
		{
			first.Next = second;
			second.Previous = first;
		}

		internal static Room CreateRoom(RoomContents contents)
		{
			Enemy enemy = null;
			List<Item> loot = null;
			if (contents.HasFlag(RoomContents.HasEnemy))
			{
				enemy = Balance.Balancer.CreateRandomEnemy();
			}
			if (contents.HasFlag(RoomContents.HasLoot))
			{
				loot = Balance.Balancer.CreateRandomItems(0, 3).ToList();
			}

			return new Room(enemy, loot);
		}

		public IReadOnlyList<Item> TakeLoot()
		{
			Looted.Invoke(this, new Events.LootedEventArgs(this));
			var loot = new List<Item>(_loot);
			_loot = null;
			return loot.AsReadOnly();
		}

		private void Enemy_LootDropped(object sender, Events.LootDroppedEventArgs e)
		{
			foreach (var item in e.Loot)
				this._loot.Add(item);
		}
	}
}
