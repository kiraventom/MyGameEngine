namespace GameEngine.GameObjects.Usables.Items.Consumables
{
	public class Bread : Consumable, IHealing
	{
		public override string Name => "Хлеб";

		public override uint MinPower => 10;
		public override uint MaxPower => 10;
	}
}
