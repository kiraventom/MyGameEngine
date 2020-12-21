namespace GameEngine.GameObjects.Rooms
{
	public abstract class Room : IGameObject
	{
		protected Room() { }

		public abstract string Name { get; }

		public override string ToString() => this.Name;
	}
}
