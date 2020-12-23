using GameEngine.GameObjects.Actors;
using GameEngine.GameObjects.Rooms;
using GameEngine.GameObjects.Usables.Items;
using GameEngine.GameObjects.Usables.Items.Consumables;
using System.Linq;

namespace GameEngine
{
	public sealed class Engine
	{
		public Engine() { }

		public enum ActionRequest { MoveForward, MoveBackwards, Attack, Loot, Use }

		public Player Player { get; } = new Player();
		public bool IsGameRunning => Player.IsAlive;

		public Item SelectedItem
		{
			get => _selectedItem;
			set => _selectedItem = Player.GetInventory().FirstOrDefault(i => i.GetType() == value.GetType());
		}

		private Item _selectedItem;

		public void Tick(ActionRequest action)
		{
			if (!IsGameRunning)
				return;

			switch (action)
			{
				case ActionRequest.Attack:
					ProcessFight();
					break;

				case ActionRequest.Loot:
					Player.GatherLoot();
					break;

				case ActionRequest.MoveForward:
					Player.MoveToNextRoom();
					break;

				case ActionRequest.MoveBackwards:
					Player.MoveToPrevRoom();
					break;

				case ActionRequest.Use:
					if (this.SelectedItem is not null && this.SelectedItem is Consumable c)
						c.Use(Player, Player);
					break;

				default:
					throw new System.NotImplementedException();
			}
		}

		private void ProcessFight()
		{
			if (Player.IsInFight)
			{
				var enemy = (Player.Room as EnemyRoom).Enemy;
				Player.Attack(enemy);
				if (enemy.IsAlive)
					enemy.Attack(Player);
			}
		}
	}
}
