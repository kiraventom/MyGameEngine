namespace GameEngine.GameObjects.Actors.Enemies
{
	public class Skeleton : Enemy
	{
		public Skeleton() : base() { }

		public override string Name => "Скелет";
		public override string Description => "В общем-то, он похож на тебя. На тебя, если бы я не добавил в игру еду.";
	}
}
