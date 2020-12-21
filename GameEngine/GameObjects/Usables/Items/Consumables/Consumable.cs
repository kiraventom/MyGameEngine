namespace GameEngine.GameObjects.Usables.Items.Consumables
{
	public abstract class Consumable : Item
	{
		protected Consumable()
		{
			this.Used += (s, ea) => ea.User.Inventory.Remove(this);
		}
	}
}
