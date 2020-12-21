namespace GameEngine.GameObjects.Usables.Abilities
{
	public class TrollRegenAbility : Ability, IHealing
	{
		public override string Name => "Регенерация тролля";

		public override uint MinPower => 5;
		public override uint MaxPower => 10;
	}
}
