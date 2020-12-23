using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace GameEngine.Balance.Tables
{
	internal static class WeightsTable
	{
		static WeightsTable()
		{
			LoadedWeights = ReadJson();
		}

		private static Weights ReadJson()
		{
			string contents = System.IO.File.ReadAllText(filename);
			return JsonSerializer.Deserialize<Weights>(contents);
		}

		private const string filename = "weights.json";
		private static Weights LoadedWeights { get; }
			
		public static uint GetWeight(string typename)
		{
			var allWeights = LoadedWeights.All;
			bool found = allWeights.TryGetValue(typename, out uint weight);
			if (found)
				return weight;
			else
				throw new ArgumentException($"Type not found in {filename}");
		}

#pragma warning disable CA1812
		private class Weights
		{
			private Dictionary<string, uint> _all;
			public Dictionary<string, uint> All => _all ??= Enemies.Concat(Items).Concat(RoomContents).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
			public Dictionary<string, uint> Enemies { get; set; }
			public Dictionary<string, uint> Items { get; set; }
			public Dictionary<string, uint> RoomContents { get; set; }
		}
	}
}
