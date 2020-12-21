using System;
using System.Collections.Generic;

namespace GameEngine.GameObjects.Usables.Items
{
	public class ItemEqualityComparer : IEqualityComparer<Item>
	{
		public bool Equals(Item x, Item y) => x is not null && y is not null && x.Name == y.Name;
		public int GetHashCode(Item obj) => obj is not null ? obj.Name.GetHashCode() : 0;
	}

	public abstract class Item : Usable
	{
		
	}
}
