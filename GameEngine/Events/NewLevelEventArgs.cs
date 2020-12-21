using GameEngine.GameObjects.Actors;
using System;

namespace GameEngine.Events
{
	public class NewLevelEventArgs : EventArgs
	{
		public NewLevelEventArgs(Player player, uint oldLevel)
		{
			if (player is null)
				throw new ArgumentNullException(nameof(player));

			Player = player;
			OldLevel = oldLevel;
			NewLevel = player.Level;
		}

		public Player Player { get; }
		public uint OldLevel { get; }
		public uint NewLevel { get; }
	}
}
