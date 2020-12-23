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
			this.ReachedNewLevel += (_, e) => this.GainHealth(e.NewLevel * 10, this);
			AcquireRoomToActor(new EmptyRoom(), this);
		}

		//Overrides
		public override string Name => "Игрок";
		protected override IEnumerable<Item> StartingInventory => Balancer.CreateRandomObjects<Item>(0, 2);
		public override uint Level => XP / 10;

		// Properties
		public bool IsInFight => Room is EnemyRoom enemyRoom && enemyRoom.Enemy.IsAlive;
		private uint XP { get; set; }

		// Events
		public event EventHandler<Events.NewLevelEventArgs> ReachedNewLevel;
		public event EventHandler<Events.MovedToRoomEventArgs> Moved;

		// Methods
		internal void MoveToNextRoom()
		{
			if (!this.IsInFight)
			{
				Room nextRoom;
				bool isNextRoomNew = this.Room.NextRoom is null;
				nextRoom = isNextRoomNew ? CreateNewRoom() : this.Room.NextRoom;
				AcquireRoomToActor(nextRoom, this);
				Moved.Invoke(this, new Events.MovedToRoomEventArgs(this, this.Room, isNextRoomNew));
			}
		}

		internal void MoveToPrevRoom()
		{
			if (!this.IsInFight && this.Room.PreviousRoom is not null)
			{
				AcquireRoomToActor(this.Room.PreviousRoom, this);
				Moved.Invoke(this, new Events.MovedToRoomEventArgs(this, this.Room, false));
			}
		}

		private Room CreateNewRoom()
		{
			var newRoom = Balancer.CreateRandomObject<Room>();
			Room.LinkRooms(this.Room, newRoom);
			if (newRoom is EnemyRoom er)
				er.Enemy.Defeated += (_, _) =>
				{
					var levelBefore = Level;
					this.XP += er.Enemy.Level;
					if (levelBefore != this.Level)
						ReachedNewLevel.Invoke(this, new Events.NewLevelEventArgs(this, levelBefore));
				};

			return newRoom;
		}

		internal void GatherLoot()
		{
			if (Room is LootRoom lootRoom && lootRoom.Loot is not null)
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
