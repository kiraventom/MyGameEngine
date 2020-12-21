using GameEngine.GameObjects.Actors;
using GameEngine.GameObjects.Actors.Enemies;
using GameEngine.GameObjects.Usables.Items;
using System;

namespace GameEngine.Events
{
	public class AttackedEventArgs : EventArgs
	{
		public AttackedEventArgs(Actor attacker, Actor defender, uint damageDealt)
		{
			this.Attacker = attacker;
			this.Defender = defender;
			this.DamageDealt = damageDealt;
		}

		public Actor Attacker { get; }
		public Actor Defender { get; }
		public uint DamageDealt { get; }
	}

	public class RogueStoleEventArgs : EventArgs
	{
		public RogueStoleEventArgs(Rogue stealer, Actor stolenFrom, Item stolen)
		{
			this.Stealer = stealer;
			this.StolenFrom = stolenFrom;
			this.Stolen = stolen;
		}

		public Rogue Stealer { get; }
		public Actor StolenFrom { get; }
		public Item Stolen { get; }
	}
}
