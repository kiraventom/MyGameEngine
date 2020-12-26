namespace GameEngine.GameObjects.Usables.Items.Consumables
{
	public class HealingPotion : Consumable, IHealing
	{
		public override string Name => "Зелье лечения";
		public override string Description => "Каноническая бутылочка с красной жидкостью.";

		public override uint MinPower => 75;
		public override uint MaxPower => 75;
	}
}
