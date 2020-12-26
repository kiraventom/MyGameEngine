using GameEngine.Rooms;
using System;

namespace GameEngine.Events
{
	public class LootedEventArgs : EventArgs
	{
		public LootedEventArgs(Room room)
		{
			Room = room;
		}

		public Room Room { get; }
	}
}
