namespace GameEngine.GameObjects.Usables.Abilities
{
	public class ZombiePoisonAbility : Ability, IDamaging
	{
		public override string Name => "Яд зомби";
		public override string Description => "Трупный яд, сочащийся из разлагающегося тела зомби";

		public override uint MinPower => 0;
		public override uint MaxPower => 5;
	}
}
