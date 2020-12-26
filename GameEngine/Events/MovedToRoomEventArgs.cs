using GameEngine.GameObjects.Actors;
using GameEngine.Rooms;
using System;

namespace GameEngine.Events
{
	public class MovedToRoomEventArgs : EventArgs
	{
		public MovedToRoomEventArgs(Player player, Room room, bool isNew)
		{
			Player = player;
			Room = room;
			IsRoomNew = isNew;
		}

		public Player Player { get; }
		public Room Room { get; }
		public bool IsRoomNew { get; } // был ли игрок в комнате раньше
	}
}
