using GameEngine.Balance;
using GameEngine.GameObjects.Usables.Abilities;

namespace GameEngine.GameObjects.Actors.Enemies
{
	public class Zombie : Enemy
	{
		public Zombie() : base()
		{
			Ability = new ZombiePoisonAbility();
		}

		public override string Name => "Зомби";
		public override string Description => "С каждой атакой отравляет тебя трупным ядом. Берегись!";
		public override Ability Ability { get; }

		internal override void Attack(Actor defender) 
		{
			base.Attack(defender);
			Poison(defender);
		}

		private void Poison(Actor poisoned)
		{
			if (poisoned.IsAlive)
			{
				Ability.Use(this, poisoned);
			}
		}
	}
}
