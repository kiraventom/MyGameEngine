using GameEngine.GameObjects.Actors;
using System;
using System.Text;

namespace GameEngine.GameObjects.Usables
{
	public interface IHealing : IUsable
	{
		Action<Actor, IGameObject, uint> IUsable.BasicEffect => this.HealingEffect;
		protected string HealingDescription
		{
			get
			{
				StringBuilder sb = new StringBuilder("Восстанавливает ");
				sb.Append(MinPower);
				if (MinPower != MaxPower)
				{
					sb.Append(" - ");
					sb.Append(MaxPower);
				}
				sb.Append(" здоровья");
				return sb.ToString();
			}
		}


		protected Action<Actor, IGameObject, uint> HealingEffect => (_, usedAt, pow) => (usedAt as Actor)?.GainHealth(pow, this);
		string IUsable.UsableDescription => this.HealingDescription;
	}
}
