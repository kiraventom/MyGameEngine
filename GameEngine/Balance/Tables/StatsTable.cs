using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace GameEngine.Balance.Tables
{
	internal static class StatsTable
	{
		static StatsTable()
		{
			LoadedStats = ReadJson();
		}

		private static AllStats ReadJson()
		{
			string contents = System.IO.File.ReadAllText(filename);
			return JsonSerializer.Deserialize<AllStats>(contents);
		}

		private const string filename = "stats.json";
		private static AllStats LoadedStats { get; }

		public static Stats GetStats(Type t)
		{
			var allStats = LoadedStats.All;
			bool found = allStats.TryGetValue(t.Name, out Stats stats);
			if (found)
				return stats;
			else
				throw new ArgumentException($"Type not found in {filename}");
		}

#pragma warning disable CA1812
		private class AllStats
		{
			private Dictionary<string, Stats> _all;
			public Dictionary<string, Stats> All => _all ??= Enemies.Concat(Allies).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
			public Dictionary<string, Stats> Enemies { get; set; }
			public Dictionary<string, Stats> Allies { get; set; }
		}
	}
}
