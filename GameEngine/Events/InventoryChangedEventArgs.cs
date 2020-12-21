using GameEngine.GameObjects.Actors;
using GameEngine.GameObjects.Usables.Items;
using System;
using System.Collections.Specialized;

namespace GameEngine.Events
{
	public class InventoryChangedEventArgs : EventArgs
	{
		public InventoryChangedEventArgs(Item item, NotifyCollectionChangedAction action, Actor actor)
		{
			Item = item;
			Actor = actor;
			ChangeType = action == NotifyCollectionChangedAction.Add ? Type.Add : Type.Remove;
		}

		public Actor Actor { get; }
		public Item Item { get; }
		public Type ChangeType { get; }

		public enum Type { Add, Remove }
	}
}
