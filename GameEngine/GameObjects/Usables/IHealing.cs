using GameEngine.GameObjects.Actors;
using System;

namespace GameEngine.GameObjects.Usables
{
	public interface IHealing : IUsable
	{
		Action<Actor, IGameObject, uint> IUsable.BasicEffect => this.HealingEffect;
		protected Action<Actor, IGameObject, uint> HealingEffect => (_, usedAt, pow) => (usedAt as Actor)?.GainHealth(pow, this);
	}
}
