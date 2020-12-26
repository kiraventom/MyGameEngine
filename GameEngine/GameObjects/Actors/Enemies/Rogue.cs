using GameEngine.Balance;
using GameEngine.Events;
using GameEngine.GameObjects.Usables;
using GameEngine.GameObjects.Usables.Abilities;
using GameEngine.GameObjects.Usables.Items.Consumables;
using System;
using System.Linq;

namespace GameEngine.GameObjects.Actors.Enemies
{
	public class Rogue : Enemy
	{
		public Rogue() : base() => RogueAbility = new RogueStealingAbility();

		// Overrides
		public override string Name => "Вор";
		public override string Description => "Следи за карманами, этот парень с каждой атакой имеет шанс свистнуть у тебя что-нибудь.";
		public override Ability Ability => RogueAbility;

		// Properties
		private RogueStealingAbility RogueAbility { get; }

		// Events
		public event EventHandler<RogueStoleEventArgs> RogueStole;

		// Overrides
		internal override void Attack(Actor defender)
		{
			base.Attack(defender);
			if (Balancer.Rnd.NextDouble() < 0.35)
			{
				this.RogueAbility.Use(this, defender);
				RogueStole.Invoke(this, new RogueStoleEventArgs(this, defender, RogueAbility.Stolen));
			}
		}

		internal override void ReceiveDamage(uint amount, IGameObject source)
		{
			base.ReceiveDamage(amount, source);
			if (this.IsAlive && source is Actor actor && actor.Stats.MaxStrenght >= this.CurrentHealth)
				Consume();
		}

		// Methods
		private void Consume()
		{
			var consumables = this.GetInventory().OfType<Consumable>();
			var consumable = consumables.FirstOrDefault(c => c is IHealing);
			consumable?.Use(this, this);
		}
	}
}
