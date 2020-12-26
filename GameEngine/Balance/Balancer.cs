using GameEngine.Balance.Tables;
using GameEngine.GameObjects;
using GameEngine.GameObjects.Actors.Enemies;
using GameEngine.Rooms;
using GameEngine.GameObjects.Usables.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GameEngine.Balance
{
	internal static class Balancer
	{
		static Balancer()
		{
			static Type[] getDerivedTypes(Assembly assembly, Type baseType)
			{
				return assembly.GetTypes().Where(t => baseType.IsAssignableFrom(t) && t.IsClass && !t.IsAbstract && t != baseType).ToArray();
			}

			var assembly = Assembly.GetExecutingAssembly();
			BaseRandomableTypes = new[] { typeof(Enemy), typeof(Item) };
			var allDerivedTypes = new List<KeyValuePair<Type, Type>>();

			// заполняем AllDerivedTypes
			foreach (var type in BaseRandomableTypes)
			{
				var derivedTypes = getDerivedTypes(assembly, type);
				foreach (var derivedTpe in derivedTypes)
				{
					allDerivedTypes.Add(new KeyValuePair<Type, Type>(type, derivedTpe));
				}
			}

			AllDerivedTypes = allDerivedTypes.ToLookup(kvp => kvp.Key, kvp => kvp.Value);
		}

		public static readonly Random Rnd = new Random();

		private static readonly IEnumerable<Type> BaseRandomableTypes;
		private static readonly ILookup<Type, Type> AllDerivedTypes;

		internal static Enemy CreateRandomEnemy() => CreateRandomObject<Enemy>();
		internal static IEnumerable<Enemy> CreateRandomEnemies(uint amount) => CreateRandomObjects<Enemy>(amount);
		internal static IEnumerable<Enemy> CreateRandomEnemies(uint min, uint max) => CreateRandomObjects<Enemy>(min, max);

		internal static Item CreateRandomItem() => CreateRandomObject<Item>();
		internal static IEnumerable<Item> CreateRandomItems(uint amount) => CreateRandomObjects<Item>(amount);
		internal static IEnumerable<Item> CreateRandomItems(uint min, uint max) => CreateRandomObjects<Item>(min, max);

		internal static Room CreateRandomRoom()
		{
			var pairs = new Dictionary<string, uint>();
			foreach (var name in Enum.GetNames(typeof(Room.RoomContents)))
			{
				uint weight = WeightsTable.GetWeight(name);
				pairs.Add(name, weight);
			}

			var rolledName = GetRoll(pairs);
			var contents = (Room.RoomContents)Enum.Parse(typeof(Room.RoomContents), rolledName);
			return Room.CreateRoom(contents);
		}

		internal static T CreateRandomObject<T>() where T : class, IGameObject
		{
			Type[] types = GetDerivedTypes(typeof(T));
			return CreateRandomObject<T>(types);
		}

		internal static IEnumerable<T> CreateRandomObjects<T>(uint amount) where T : class, IGameObject => CreateRandomObjects<T>(amount, amount);

		internal static IEnumerable<T> CreateRandomObjects<T>(uint minimum, uint maximum) where T : class, IGameObject
		{
			Type[] types = GetDerivedTypes(typeof(T));
			uint amount = minimum == maximum ? minimum : (uint)Rnd.Next((int)minimum, (int)maximum);
			T[] instances = new T[amount];
			for (int i = 0; i < amount; ++i)
				instances[i] = CreateRandomObject<T>(types);

			return instances;
		}

		private static T CreateRandomObject<T>(Type[] types) where T: class, IGameObject
		{
			Type t = GetRoll(types);
			T instance = t.GetConstructor(Type.EmptyTypes).Invoke(null) as T;
			return instance;
		}

		private static Type[] GetDerivedTypes(Type type)
		{
			if (AllDerivedTypes.Contains(type))
			{
				var derivedTypes = AllDerivedTypes[type];
				return derivedTypes.ToArray();
			}
			else
			{
				throw new ArgumentException(
					$"Cannot create random objects of type {type.Name}.\n" +
					$"Supported types are {string.Join(", ", BaseRandomableTypes.Select(t => t.Name))}");
			}
		}

		private static Type GetRoll(Type[] types)
		{
			var pairs = types.ToDictionary(t => t, t => WeightsTable.GetWeight(t.Name));
			var type = GetRoll(pairs);
			return type;
		}

		private static T GetRoll<T>(Dictionary<T, uint> pairs) 
		{
			var orderedPairs = pairs.OrderBy(p => p.Value).ToArray(); // 5 10 40 100
			var newPairs = new (T key, uint newWeight)[orderedPairs.Length]; // 5 15 55 155
			for (int i = 0; i < orderedPairs.Length; ++i)
			{
				newPairs[i].key = orderedPairs[i].Key;
				newPairs[i].newWeight = orderedPairs[i].Value;
				if (i > 0)
					newPairs[i].newWeight += newPairs[i - 1].newWeight;
			}

			uint max = newPairs.Max(p => p.newWeight);
			int roll = Rnd.Next(0, (int)max);
			for (uint i = 0; i < newPairs.Length; ++i)
			{
				if (roll < newPairs[i].newWeight)
					return newPairs[i].key;
			}

			throw new InvalidOperationException("Should not be possible");
		}
	}
}
