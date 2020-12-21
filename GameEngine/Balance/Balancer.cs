using GameEngine.Balance.Tables;
using GameEngine.GameObjects;
using GameEngine.GameObjects.Actors.Enemies;
using GameEngine.GameObjects.Rooms;
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
			BaseRandomableTypes = new[] { typeof(Enemy), typeof(Item), typeof(Room) };
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

		private static readonly IEnumerable<Type> BaseRandomableTypes;
		private static readonly ILookup<Type, Type> AllDerivedTypes;

		public static readonly Random Rnd = new Random();

		public static T CreateRandomObject<T>() where T : class, IGameObject
		{
			Type[] types = GetDerivedTypes(typeof(T));
			Type t = GetRoll(types);

			T instance = t.GetConstructor(Type.EmptyTypes).Invoke(null) as T;
			return instance;
		}

		public static IEnumerable<T> CreateRandomObjects<T>(uint minimum, uint maximum) where T : class, IGameObject
		{
			Type[] types = GetDerivedTypes(typeof(T));

			uint amount = (uint)Rnd.Next((int)minimum, (int)maximum);
			T[] instances = new T[amount];
			for (int i = 0; i < amount; ++i)
			{
				Type t = GetRoll(types);
				instances[i] = t.GetConstructor(Type.EmptyTypes).Invoke(null) as T;
			}

			return instances.AsEnumerable();
		}

		public static IEnumerable<T> CreateRandomObjects<T>(uint amount) where T : class, IGameObject => CreateRandomObjects<T>(amount, amount);

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
			var pairs = types.Select(t => new { type = t, weight = WeightsTable.GetWeight(t) });
			var orderedPairs = pairs.OrderBy(p => p.weight).ToArray(); // 5 10 40 100
			var newPairs = new (Type type, uint newWeight)[orderedPairs.Length]; // 5 15 55 155
			for (int i = 0; i < orderedPairs.Length; ++i)
			{
				newPairs[i].type = orderedPairs[i].type;
				newPairs[i].newWeight = orderedPairs[i].weight;
				if (i > 0)
					newPairs[i].newWeight += newPairs[i - 1].newWeight;
			}

			uint max = newPairs.Max(p => p.newWeight);
			int roll = Rnd.Next(0, (int)max);
			for (uint i = 0; i < newPairs.Length; ++i)
			{
				if (roll < newPairs[i].newWeight)
					return newPairs[i].type;
			}

			throw new InvalidOperationException("Should not be possible");
		}
	}
}
