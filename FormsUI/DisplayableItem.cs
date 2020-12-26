using GameEngine.GameObjects.Usables.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormsUI
{
	class DisplayableItem
	{
		public DisplayableItem(Item item, int amount) 
		{
			this.Amount = amount;
			this.Item = item;
		}

		public int Amount { get; set; }

		public Item Item;

		public override string ToString()
		{
			return this.Item.Name + (Amount > 1 ? " x" + Amount.ToString() : string.Empty);
		}
	}
}
