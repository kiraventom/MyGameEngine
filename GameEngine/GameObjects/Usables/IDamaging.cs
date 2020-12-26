using GameEngine.GameObjects.Actors;
using System;
using System.Text;

namespace GameEngine.GameObjects.Usables
{
	public interface IDamaging : IUsable
	{
		protected Action<Actor, IGameObject, uint> DamagingEffect => (_, usedAt, pow) => (usedAt as Actor)?.ReceiveDamage(pow, this);
		protected string DamagingDescription
		{
			get
			{
				StringBuilder sb = new StringBuilder("Наносит ");
				sb.Append(MinPower);
				if (MinPower != MaxPower)
				{
					sb.Append(" - ");
					sb.Append(MaxPower);
				}
				sb.Append(" урона");
				return sb.ToString();
			}
		}

		Action<Actor, IGameObject, uint> IUsable.BasicEffect => this.DamagingEffect;
		string IUsable.UsableDescription => this.DamagingDescription;
	}
}
