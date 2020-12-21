using GameEngine.GameObjects.Actors;
using GameEngine.GameObjects.Rooms;
using System;

namespace GameEngine.Events
{
	public class MovedToRoomEventArgs : EventArgs
	{
		public MovedToRoomEventArgs(Player player, Room room)
		{
			Player = player;
			Room = room;
		}

		public Player Player { get; }
		public Room Room { get; }
	}
}
