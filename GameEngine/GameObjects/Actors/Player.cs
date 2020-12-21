using GameEngine.Balance;
using GameEngine.GameObjects.Rooms;
using GameEngine.GameObjects.Usables.Items;
using System;
using System.Collections.Generic;

namespace GameEngine.GameObjects.Actors
{
	public class Player : Actor
	{
		public Player()
		{
			CurrentRoom = new EmptyRoom();
			this.ReachedNewLevel += (_, e) => this.GainHealth(e.NewLevel * 10, this);
		}

		//Overrides
		public override string Name => "Игрок";
		protected override IEnumerable<Item> StartingInventory => Balancer.CreateRandomObjects<Item>(0, 2);
		public override uint Level => XP / 10;

		// Properties
		public bool IsInFight => CurrentRoom is EnemyRoom enemyRoom && enemyRoom.Enemy.IsAlive;
		public Room CurrentRoom { get; private set; }
		private uint XP { get; set; }

		// Events
		public event EventHandler<Events.NewLevelEventArgs> ReachedNewLevel;
		public event EventHandler<Events.MovedToRoomEventArgs> Moved;

		// Methods
		internal void MoveToNextRoom()
		{
			if (!this.IsInFight)
			{
				var room = Balancer.CreateRandomObject<Room>();
				Moved.Invoke(this, new Events.MovedToRoomEventArgs(this, room));
				this.CurrentRoom = room;
				if (CurrentRoom is EnemyRoom er)
					er.Enemy.Defeated += (_, _) =>
					{
						var levelBefore = Level;
						this.XP += er.Enemy.Level;
						if (levelBefore != this.Level)
							ReachedNewLevel.Invoke(this, new Events.NewLevelEventArgs(this, levelBefore));
					};
			}
		}

		internal void Loot()
		{
			if (CurrentRoom is LootRoom lootRoom && lootRoom.Loot is not null)
			{
				var loot = lootRoom.TakeLoot();
				foreach (var item in loot)
				{
					this.Inventory.Add(item);
				}
			}
		}
	}
}
