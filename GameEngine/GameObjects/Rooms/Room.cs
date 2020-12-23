namespace GameEngine.GameObjects.Rooms
{
	public abstract class Room : IGameObject
	{
		protected Room()
		{

		}

		public abstract string Name { get; }
		public override string ToString() => this.Name;

		public uint Depth
		{
			get
			{
				uint depth = 0;
				var room = this;
				while (room.PreviousRoom is not null)
				{
					room = room.PreviousRoom;
					++depth;
				}

				return depth;
			}
		}

		internal bool IsEntrance => PreviousRoom is null;

		internal static void LinkRooms(Room first, Room second)
		{
			first.NextRoom = second;
			second.PreviousRoom = first;
		}

		public Room PreviousRoom { get; private set; }
		public Room NextRoom { get; private set; }

		public static Room GetFirstRoom(Room room)
		{
			while (room.PreviousRoom is not null)
			{
				room = room.PreviousRoom;
			}

			return room;
		}

		public static Room GetLastRoom(Room room)
		{
			while (room.NextRoom is not null)
			{
				room = room.NextRoom;
			}

			return room;
		}
	}
}
