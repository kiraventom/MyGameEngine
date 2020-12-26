using GameEngine.GameObjects.Actors;
using GameEngine.GameObjects.Actors.Enemies;
using GameEngine.GameObjects.Usables.Items;
using System;

namespace GameEngine.GameObjects.Usables.Abilities
{
	public class RogueStealingAbility : Ability
	{
		public override string Name => "Ловкость воришки";
		protected override string Description => "Проворный проныра проникает в пожитки и присваивает полезности";

		public override uint MinPower => 0;
		public override uint MaxPower => 0;

		public Item Stolen { get; private set; }

		protected override Action<Actor, IGameObject, uint> Effect => (user, usedAt, _) =>
		{
			if (usedAt is Player stolenFrom && user is Rogue stealer)
			{
				var enemyInv = stolenFrom.GetInventory();
				if (enemyInv.Count == 0)
					return;

				var index = Balance.Balancer.Rnd.Next(0, enemyInv.Count);
				var rndItem = enemyInv[index];
				stealer.Inventory.Add(rndItem);
				stolenFrom.Inventory.Remove(rndItem);
				Stolen = rndItem;
			}
		};
	}
}
