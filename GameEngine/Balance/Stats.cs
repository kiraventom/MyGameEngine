namespace GameEngine.Balance
{
	public class Stats
	{
		public uint Level { get; set; }
		public uint BaseHealth { get; set; }
		public uint MinStrenght { get; set; }
		public uint MaxStrenght { get; set; }

		public uint GetStrenght() => (uint)Balancer.Rnd.Next((int)MinStrenght, (int)MaxStrenght);
	}
}
