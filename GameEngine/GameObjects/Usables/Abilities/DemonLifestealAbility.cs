using GameEngine.GameObjects.Actors;

namespace GameEngine.GameObjects.Usables.Abilities
{
	public class DemonLifestealAbility : Ability, IHealing
	{
		public DemonLifestealAbility() { }

		public override string Name => "Вытягивание жизни";
		protected override string Description => "Нечеловеческая способность подпитывать свою мощь, истощая противника";
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
