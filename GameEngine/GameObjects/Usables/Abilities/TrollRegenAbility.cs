namespace GameEngine.GameObjects.Usables.Abilities
{
	public class TrollRegenAbility : Ability, IHealing
	{
		public override string Name => "Регенерация тролля";
		public override string Description => "Толстая кожа тролля зарастает на глазах";

		public override uint MinPower => 5;
		public override uint MaxPower => 10;
	}
}
