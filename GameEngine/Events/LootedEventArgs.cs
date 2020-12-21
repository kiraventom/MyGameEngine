using GameEngine.GameObjects.Rooms;
using System;

namespace GameEngine.Events
{
	public class LootedEventArgs : EventArgs
	{
		public LootedEventArgs(LootRoom lootRoom)
		{
			Room = lootRoom;
		}

		public LootRoom Room { get; }
	}
}
