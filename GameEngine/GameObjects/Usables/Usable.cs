using GameEngine.Events;
using GameEngine.GameObjects.Actors;
using System;

namespace GameEngine.GameObjects.Usables
{
	public abstract class Usable : IUsable
	{
		protected Usable() { }

		public abstract string Name { get; }
		public abstract uint MinPower { get; }
		public abstract uint MaxPower { get; }
		internal virtual Action<Actor, IGameObject, uint> Effect => null;

		private Action<Actor, IGameObject, uint> _basicEffect => (user, usedAt, power) => //redesign
		{
			if (this is IHealing)
				(usedAt as Actor)?.GainHealth(power, this);
			else
			if (this is IDamaging)
				(usedAt as Actor)?.ReceiveDamage(power, this);
		};

		uint IUsable.MinPower => this.MinPower;
		uint IUsable.MaxPower => this.MaxPower;
		void IUsable.Use(Actor user, IGameObject usedAt) => this.Use(user, usedAt);

		public event EventHandler<UsedEventArgs> Used;

		internal virtual void Use(Actor user, IGameObject usedAt)
		{
			this.Used.Invoke(this, new UsedEventArgs(user, usedAt, this));
			this._basicEffect.Invoke(user, usedAt, this.GetPower());
			this.Effect?.Invoke(user, usedAt, this.GetPower());
		}

		public uint GetPower() => (uint)Balance.Balancer.Rnd.Next((int)MinPower, (int)MaxPower);
	}
}
