namespace GameEngine.GameObjects.Usables.Items.Consumables
{
	public class Meat : Consumable, IHealing
	{
		public override string Name => "Мясо";

		public override uint MinPower => 20;
		public override uint MaxPower => 20;
	}
}
