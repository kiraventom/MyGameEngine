using GameEngine;
using GameEngine.Events;
using GameEngine.GameObjects.Actors.Enemies;
using GameEngine.Rooms;
using GameEngine.GameObjects.Usables;
using GameEngine.GameObjects.Usables.Items;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UI
{
	class Program
	{
		static Engine engine;

		static int _selectedInventoryIndex = -1;
		static int SelectedInventoryIndex
		{
			get => _selectedInventoryIndex;
			set
			{
				_selectedInventoryIndex = value;
				if (value >= 0)
				{
					var distInv = GetDistinctInventory();
					engine.SelectedItem = distInv.Any() ? distInv[SelectedInventoryIndex] : null;
				}
				else
				{
					engine.SelectedItem = null;
				}
			}
		}

		static void Main()
		{
			engine = new Engine();
			Subscribe();

			Console.CursorVisible = false;
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;

			Console.Clear();

			if (engine.Player.GetInventory().Any())
			{
				if (SelectedInventoryIndex == -1)
					SelectedInventoryIndex = 0;
			}
			else
			{
				if (SelectedInventoryIndex != -1)
					SelectedInventoryIndex = -1;
			}

			DrawInfo();
			DrawControls();
			DrawInventory();
			DrawInventorySelector();
			DrawMap();

			while (true)
			{
				Console.CursorVisible = false;
				Console.BackgroundColor = ConsoleColor.Black;
				Console.ForegroundColor = ConsoleColor.White;

				SelectedInventoryIndex = SelectedInventoryIndex; // that should be fixed

				var cki = Console.ReadKey(true);

				if (!engine.IsGameRunning)
					return;

				Engine.ActionRequest? action = (cki.Key) switch
				{
					ConsoleKey.A => engine.Player.IsInFight ? Engine.ActionRequest.Attack : null,
					ConsoleKey.Tab => !engine.Player.IsInFight && engine.Player.Room.HasLoot ? Engine.ActionRequest.Loot : null,
					ConsoleKey.W => !engine.Player.IsInFight ? Engine.ActionRequest.MoveForward : null,
					ConsoleKey.S => !engine.Player.IsInFight ? Engine.ActionRequest.MoveBackwards : null,
					ConsoleKey.E => engine.Player.GetInventory().Any() ? Engine.ActionRequest.Use : null,
					_ => null
				};

				if (action is null)
				{
					if (cki.Key == ConsoleKey.UpArrow || cki.Key == ConsoleKey.DownArrow)
					{
						Console.Clear();
						var distinctInv = GetDistinctInventory();
						if (distinctInv.Any())
						{
							if (SelectedInventoryIndex == -1)
								SelectedInventoryIndex = 0;

							if (cki.Key == ConsoleKey.UpArrow)
							{
								if (SelectedInventoryIndex == 0)
									SelectedInventoryIndex = distinctInv.Count - 1;
								else
									--SelectedInventoryIndex;

							}
							if (cki.Key == ConsoleKey.DownArrow)
							{
								if (SelectedInventoryIndex == distinctInv.Count - 1)
									SelectedInventoryIndex = 0;
								else
									++SelectedInventoryIndex;
							}

							DrawInventorySelector();
						}
					}
					else
					{
						continue;
					}
				}
				else
				{
					Console.Clear();
					Console.SetCursorPosition(5, 5);
					engine.Tick(action.Value);

					if (action.Value == Engine.ActionRequest.Use)
					{
						var distInv = GetDistinctInventory();
						if (distInv.Count <= SelectedInventoryIndex)
							SelectedInventoryIndex = distInv.Count - 1;
					}
				}

				if (engine.Player.GetInventory().Any())
				{
					if (SelectedInventoryIndex == -1)
						SelectedInventoryIndex = 0;
				}
				else
				{
					if (SelectedInventoryIndex != -1)
						SelectedInventoryIndex = -1;
				}

				DrawInfo();
				DrawControls();
				DrawInventory();
				DrawInventorySelector();
				DrawMap();
			}
		}

		private static IList<Item> GetDistinctInventory()
		{
			return engine.Player.GetInventory().Distinct(new ItemEqualityComparer()).OrderBy(i => i.Name).ToList();
		}

		static void Subscribe()
		{
			if (engine.Player is not null)
			{
				engine.Player.Attacked += Actor_Attacked;
				engine.Player.Defeated += Actor_Defeated;
				engine.Player.HealthChanged += Actor_HealthChanged;
				engine.Player.Moved += Player_Moved;
				engine.Player.InvChanged += Player_InvChanged;
				engine.Player.ReachedNewLevel += Player_NewLevelReached;

				var inv = engine.Player.GetInventory();
				foreach (var item in inv)
					item.Used += Usable_Used;
			}
		}

		private static void Player_NewLevelReached(object sender, NewLevelEventArgs e)
		{
			Console.ForegroundColor = ConsoleColor.Magenta;
			Console.WriteLine($"{e.Player.Name} получил новый уровень!");
			Console.SetCursorPosition(5, Console.CursorTop);
			Console.WriteLine($"Здоровье восстановлено");
			Console.SetCursorPosition(5, Console.CursorTop);
			Console.ForegroundColor = ConsoleColor.White;
		}

		private static void Player_InvChanged(object sender, InventoryChangedEventArgs e)
		{
			if (e.ChangeType == InventoryChangedEventArgs.Type.Add)
				e.Item.Used += Usable_Used;
			else
			if (e.ChangeType == InventoryChangedEventArgs.Type.Remove)
				e.Item.Used -= Usable_Used;
		}

		private static void Usable_Used(object sender, UsedEventArgs e)
		{
			Console.WriteLine($"{e.User.Name} использовал {e.Usable.Name}");
			Console.SetCursorPosition(5, Console.CursorTop);
		}

		private static void Actor_Attacked(object sender, AttackedEventArgs e)
		{
			Console.WriteLine($"{e.Attacker.Name} атаковал {e.Defender.Name}");
			Console.SetCursorPosition(5, Console.CursorTop);
		}

		private static void Actor_Defeated(object sender, DefeatedEventArgs e)
		{
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine($"{e.Dead.Name} пал в бою с {e.Killer.Name}");
			Console.SetCursorPosition(5, Console.CursorTop);
			Console.ForegroundColor = ConsoleColor.White;
		}

		private static void Actor_HealthChanged(object sender, HealthChangedEventArgs e)
		{
			if (e.Change > 0)
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine($"{e.Actor.Name} восстановил {e.Change} здоровья благодаря {e.Source.Name}");
				Console.SetCursorPosition(5, Console.CursorTop);
			}

			if (e.Change < 0)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"{e.Actor.Name} получил {- e.Change} урона от {e.Source.Name}");
				Console.SetCursorPosition(5, Console.CursorTop);
			}

			Console.ForegroundColor = ConsoleColor.White;
		}

		private static void Player_Moved(object sender, MovedToRoomEventArgs e)
		{
			Console.Write($"{e.Player.Name} вошёл в комнату");
			if (e.Room.HasEnemy)
			{
				Console.Write($". Противник: ");
				if (e.IsRoomNew)
				{
					e.Room.Enemy.Attacked += Actor_Attacked;
					e.Room.Enemy.Defeated += Actor_Defeated;
					e.Room.Enemy.HealthChanged += Actor_HealthChanged;
					e.Room.Enemy.Ability.Used += Usable_Used;
					e.Room.Enemy.LootDropped += Enemy_LootDropped;
					if (e.Room.Enemy is Rogue r)
					{
						r.RogueStole += Rogue_RogueStole;
					}
				}
				if (e.Room.Enemy.IsAlive)
					Console.Write(e.Room.Enemy.Name);
				else
					Console.Write("<мёртв>");
			}
			if (e.Room.HasLoot)
			{
				Console.Write(". Лут ");
				
				if (e.Room.Loot is null)
					Console.Write("<собран>");
			}

			if (e.IsRoomNew)
				e.Room.Looted += Room_Looted;
				

			Console.WriteLine();
			Console.SetCursorPosition(5, Console.CursorTop);
		}

		private static void Enemy_LootDropped(object sender, LootDroppedEventArgs e)
		{
			if (!e.Loot.Any())
				return;

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine($"Из {e.Dropper.Name} выпало {e.Loot.Count()} предметов!");
			Console.SetCursorPosition(5, Console.CursorTop);
			Console.ForegroundColor = ConsoleColor.White;
		}

		private static void Rogue_RogueStole(object sender, RogueStoleEventArgs e)
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine($"{e.Stealer.Name} украл у {e.StolenFrom.Name} {e.Stolen.Name}!");
			Console.SetCursorPosition(5, Console.CursorTop);
			Console.ForegroundColor = ConsoleColor.White;
		}

		private static void Room_Looted(object sender, LootedEventArgs e)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			if (e.Room.Loot.Any())
			{
				Console.WriteLine($"После тщательных поисков вы обнаруживаете:");
				Console.SetCursorPosition(5, Console.CursorTop);
				foreach (var item in e.Room.Loot)
				{
					Console.WriteLine($"\t- {item.Name}:");
					Console.SetCursorPosition(5, Console.CursorTop);
					Console.WriteLine($"\t {item.GetDescription()}");
					Console.SetCursorPosition(5, Console.CursorTop);

				}
			}
			else
			{
				Console.WriteLine($"Поиски ни к чему не привели.");
				Console.SetCursorPosition(5, Console.CursorTop);
			}
			
			Console.ForegroundColor = ConsoleColor.White;
		}

		static void DrawInfo()
		{
			Console.SetCursorPosition(0, 0);
			const int nameWidth = 10;
			const int healthWidth = 9;
			const int strenghtWidth = 7;
			const int levelWidth = 9;
			const int depthWidth = 9;
			Console.WriteLine($"{"Имя", nameWidth}{"Здоровье", healthWidth}{"Сила", strenghtWidth}{"Уровень",levelWidth}{"Глубина",depthWidth}");

			var p = engine.Player;
			Console.WriteLine(
				$"{p.Name, nameWidth}" +
				$"{$"{p.CurrentHealth}/{p.Stats.BaseHealth}", healthWidth}" +
				$"{$"{p.Stats.MinStrenght}-{p.Stats.MaxStrenght}", strenghtWidth}" +
				$"{p.Level, levelWidth}" +
				$"{Room.GetDepth(p.Room),depthWidth}");

			if (p.IsInFight)
			{
				var e = p.Room.Enemy;
				Console.Write(
					$"{e.Name,nameWidth}" +
					$"{$"{e.CurrentHealth}/{e.Stats.BaseHealth}",healthWidth}" +
					$"{$"{e.Stats.MinStrenght}-{e.Stats.MaxStrenght}",strenghtWidth}" +
					$"{e.Level,levelWidth}");
			}
		}
		
		static void DrawControls()
		{
			const string inventoryControls = "W/S - движение, A - атака, Tab - собрать лут, стрелки - выбрать предмет, E - использовать";
			Console.SetCursorPosition(3, Console.WindowTop + Console.WindowHeight - 1);
			Console.Write(inventoryControls);
		}

		static void DrawMap()
		{
			var playerRoom = engine.Player.Room;
			Console.SetCursorPosition(0, 10);
			Console.Write(GetRoomChar(playerRoom));

			var room = playerRoom;
			for (int i = 0; i < 3; ++i)
			{
				room = room?.Next;
				Console.SetCursorPosition(0, 9 - i);
				Console.Write(GetRoomChar(room));
			}

			room = playerRoom;
			for (int i = 0; i < 3; ++i)
			{
				room = room.Previous;
				if (room is null)
					return;

				Console.SetCursorPosition(0, 11 + i);
				Console.Write(GetRoomChar(room));
			}
		}

		static char GetRoomChar(Room room)
		{
			if (engine.Player.Room == room)
				return 'P';

			return room switch
			{
				Room r when r.HasEnemy => 'E',
				Room r when r.HasLoot => 'L',
				null => '?',
				_ => '|'
			};
		}

		static void DrawInventory()
		{
			const string invName = "Инвентарь:";
			Console.SetCursorPosition(Console.WindowLeft + Console.WindowWidth - 2 - invName.Length, 4);
			Console.Write(invName);

			var inv = engine.Player.GetInventory();
			if (inv.Any())
			{
				var distinctInv = GetDistinctInventory();

				for (int i = 0; i < distinctInv.Count; ++i)
				{
					var item = distinctInv.ElementAt(i);

					var allItems = inv.Where(i => i.GetType() == item.GetType());
					string fullItemName;
					if (allItems.Count() > 1)
						fullItemName = allItems.Count() + "x " + allItems.First().Name;
					else
						fullItemName = allItems.First().Name;

					Console.SetCursorPosition(Console.WindowLeft + Console.WindowWidth - 2 - fullItemName.Length, 5 + i);
					Console.Write(fullItemName);
				}
			}
		}

		static void DrawInventorySelector()
		{
			if (SelectedInventoryIndex >= 0)
			{
				Console.SetCursorPosition(Console.WindowLeft + Console.WindowWidth - 1, 5 + SelectedInventoryIndex);
				Console.Write('<');
			}
		}
	}
}
