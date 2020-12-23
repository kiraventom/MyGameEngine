using GameEngine.Balance;
using GameEngine.GameObjects.Actors.Enemies;

namespace GameEngine.GameObjects.Rooms
{
	public class EnemyRoom : Room
	{
		public EnemyRoom()
		{
			Enemy = Balancer.CreateRandomObject<Enemy>();
			Actors.Actor.AcquireRoomToActor(this, Enemy);
		}

		public override string Name => "Комната с противником";

		public Enemy Enemy { get; }
	}
}
