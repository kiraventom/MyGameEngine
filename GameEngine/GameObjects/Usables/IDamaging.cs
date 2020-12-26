using GameEngine.GameObjects.Actors;
using System;

namespace GameEngine.GameObjects.Usables
{
	public interface IDamaging : IUsable
	{
		Action<Actor, IGameObject, uint> IUsable.BasicEffect => this.DamagingEffect;
		protected Action<Actor, IGameObject, uint> DamagingEffect => (_, usedAt, pow) => (usedAt as Actor)?.ReceiveDamage(pow, this);
	}
}
