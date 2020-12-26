using GameEngine.Events;
using GameEngine.GameObjects.Actors;
using System;

namespace GameEngine.GameObjects.Usables
{
	public abstract class Usable : IUsable
	{
		protected Usable() { }

		public abstract string Name { get; }
		public abstract string Description { get; }
		public abstract uint MinPower { get; }
		public abstract uint MaxPower { get; }
		protected virtual Action<Actor, IGameObject, uint> Effect => null;

		uint IUsable.MinPower => this.MinPower;
		uint IUsable.MaxPower => this.MaxPower;
		string IGameObject.Description => Description;

		void IUsable.Use(Actor user, IGameObject usedAt) => this.Use(user, usedAt);

		public event EventHandler<UsedEventArgs> Used;

		internal virtual void Use(Actor user, IGameObject usedAt)
		{
			this.Used.Invoke(this, new UsedEventArgs(user, usedAt, this));
			var power = this.GetPower();
			(this as IUsable)?.BasicEffect?.Invoke(user, usedAt, power);
			this.Effect?.Invoke(user, usedAt, power);
		}

		public uint GetPower() => (uint)Balance.Balancer.Rnd.Next((int)MinPower, (int)MaxPower);
	}
}
