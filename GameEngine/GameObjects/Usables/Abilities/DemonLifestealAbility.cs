using GameEngine.GameObjects.Actors;

namespace GameEngine.GameObjects.Usables.Abilities
{
	public class DemonLifestealAbility : Ability, IHealing
	{
		public DemonLifestealAbility() { }

		public override string Name => "Демоническое вытягивание жизни";
		public override uint MinPower => AmountToSteal;
		public override uint MaxPower => AmountToSteal;

		private uint AmountToSteal { get; set; }

		internal void Use(Actor user, IGameObject usedAt, uint amount)
		{
			AmountToSteal = amount;
			base.Use(user, usedAt);
		}
	}
}
