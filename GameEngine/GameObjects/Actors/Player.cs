using GameEngine.Balance;
using GameEngine.Rooms;
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
			AcquireRoomToActor(Room.CreateRoom(Room.RoomContents.None), this);
		}

		//Overrides
		public override string Name => "Игрок";
		//protected override IEnumerable<Item> StartingInventory => Balancer.CreateRandomItems(0, 2);
		protected override IEnumerable<Item> StartingInventory => Balancer.CreateRandomItems(100);
		public override uint Level => XP / 10;

		// Properties
		public bool IsInFight => Room.Enemy is not null && Room.Enemy.IsAlive;
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
				bool isNextRoomNew = this.Room.Next is null;
				nextRoom = isNextRoomNew ? CreateNewRoom() : this.Room.Next;
				AcquireRoomToActor(nextRoom, this);
				Moved.Invoke(this, new Events.MovedToRoomEventArgs(this, this.Room, isNextRoomNew));
			}
		}

		internal void MoveToPrevRoom()
		{
			if (!this.IsInFight && !this.Room.IsEntrance)
			{
				AcquireRoomToActor(this.Room.Previous, this);
				Moved.Invoke(this, new Events.MovedToRoomEventArgs(this, this.Room, false));
			}
		}

		private Room CreateNewRoom()
		{
			var newRoom = Balancer.CreateRandomRoom();
			Room.LinkRooms(this.Room, newRoom);
			if (newRoom.Enemy is not null)
			{
				newRoom.Enemy.Defeated += (_, _) =>
				{
					var levelBefore = Level;
					this.XP += newRoom.Enemy.Level;
					if (levelBefore != this.Level)
						ReachedNewLevel.Invoke(this, new Events.NewLevelEventArgs(this, levelBefore));
				};
			}

			return newRoom;
		}

		internal void GatherLoot()
		{
			if (!this.IsInFight && Room is not null && Room.Loot.Count > 0)
			{
				var loot = Room.TakeLoot();
				foreach (var item in loot)
				{
					this.Inventory.Add(item);
				}
			}
		}
	}
}
