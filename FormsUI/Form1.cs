using GameEngine.GameObjects.Actors;
using GameEngine.GameObjects.Actors.Enemies;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FormsUI
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
			this.Load += this.Form1_Load;
			SubscribeToPlayer(Engine.Player);
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			for (int i = 0; i < this.MainTLP.RowCount; ++i)
			{
				for (int j = 0; j < this.MainTLP.ColumnCount; ++j)
				{
					var canvas = new Canvas() { Margin = new Padding(0), Tag = (i, j), Dock = DockStyle.Fill };
					this.MainTLP.Controls.Add(canvas, j, i);
				}
			}

			this.InventoryLB.DisplayMember = "Name";

			DrawPlayer();
			DrawPlayerHealth();
			DrawPlayerInfo();

			var groupedInv = Engine.Player.GetInventory().GroupBy(item => item.GetType());
			foreach (var group in groupedInv)
			{
				var displayable = new DisplayableItem(group.First(), group.Count());
				DisplayableInventory.Add(displayable);
			}

			InventoryLB.DataSource = this.DisplayableInventory;
			InventoryLB.SelectedIndexChanged += this.InventoryLB_SelectedIndexChanged;
			InventoryLB.MouseDoubleClick += this.InventoryLB_MouseDoubleClick;
			EnemyCanvas.Click += AttackBt_Click;
			RoomCanvas.Click += MoveForwardBt_Click;
		}

		private readonly BindingList<DisplayableItem> DisplayableInventory = new BindingList<DisplayableItem>();

		private IEnumerable<Canvas> GetCanvas() => this.MainTLP.Controls.Cast<Control>().OfType<Canvas>();

		private Canvas PlayerCanvas => this.GetCanvas().FirstOrDefault(c => c.Tag.Equals((1, 0)));
		private Canvas PlayerHealthCanvas => this.GetCanvas().FirstOrDefault(c => c.Tag.Equals((0, 0)));
		private Canvas EnemyCanvas => this.GetCanvas().FirstOrDefault(c => c.Tag.Equals((1, 2)));
		private Canvas EnemyHealthCanvas => this.GetCanvas().FirstOrDefault(c => c.Tag.Equals((0, 2)));
		private Canvas RoomCanvas => this.GetCanvas().FirstOrDefault(c => c.Tag.Equals((1, 1)));

		private GameEngine.Engine Engine = new GameEngine.Engine();

		private void MoveForwardBt_Click(object sender, EventArgs e)
		{
			Tick(GameEngine.Engine.ActionRequest.MoveForward);
		}

		private void AttackBt_Click(object sender, EventArgs e)
		{
			Tick(GameEngine.Engine.ActionRequest.Attack);
		}

		private void InventoryLB_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (InventoryLB.SelectedIndex >= 0)
			{
				this.DescriptionRTB.Text = (InventoryLB.SelectedItem as DisplayableItem).Item.GetDescription();
			}
		}

		private void InventoryLB_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (InventoryLB.SelectedItem is not null)
			{
				Engine.SelectedItem = (InventoryLB.SelectedItem as DisplayableItem).Item;
				Tick(GameEngine.Engine.ActionRequest.Use);
			}
		}

		////////////////////////////////////////////////////////

		private void Player_Moved(object sender, GameEngine.Events.MovedToRoomEventArgs e)
		{
			if (e.IsRoomNew)
			{
				if (e.Room.HasEnemy)
				{
					DrawEnemy();
					DrawEnemyHealth();
					SubscribeToEnemy(e.Room.Enemy);
				}
			}
		}

		private void Player_InvChanged(object sender, GameEngine.Events.InventoryChangedEventArgs e)
		{
			var newDisplayable = new DisplayableItem(e.Item, 1);
			var displayable = DisplayableInventory.FirstOrDefault(di => di.Item.GetType() == newDisplayable.Item.GetType());
			switch (e.ChangeType)
			{
				case GameEngine.Events.InventoryChangedEventArgs.Type.Add:
					if (displayable is null)
						DisplayableInventory.Add(newDisplayable);
					else
						++displayable.Amount;
					break;

				case GameEngine.Events.InventoryChangedEventArgs.Type.Remove:
					if (displayable.Amount == 1)
						DisplayableInventory.Remove(displayable);
					else
						--displayable.Amount;
					break;
			}

			InventoryLB.DataSource = null;
			InventoryLB.DataSource = DisplayableInventory;
		}

		////////////////////////////////////////////////////////

		private void DrawPlayerInfo()
		{
			InfoLB.Items.Clear();
			InfoLB.Items.Add($"Имя: {Engine.Player.Name}");
			InfoLB.Items.Add($"Сила: {Engine.Player.Stats.MinStrenght} - {Engine.Player.Stats.MaxStrenght}");
			InfoLB.Items.Add($"Уровень: {Engine.Player.Level}");
			InfoLB.Items.Add($"Глубина: {GameEngine.Rooms.Room.GetDepth(Engine.Player.Room)}");
		}

		private void DrawPlayer()
		{
			var image = Properties.Resources.PlayerImg;
			PlayerCanvas.Draw(g => g.DrawImage(image, new Rectangle(new Point(0, 0), PlayerCanvas.Size)));
		}

		private void DrawEnemy()
		{
			if (Engine.Player.IsInFight)
			{
				string name = Engine.Player.Room.Enemy.Name;
				Font font = new Font(FontFamily.GenericSansSerif, 30);
				EnemyCanvas.Draw(g => g.DrawString(name, font, Brushes.DarkRed, new Rectangle(new Point(0, 0), EnemyCanvas.Size)));
			}
		}

		private void DrawPlayerHealth()
		{
			int fullCanvasWidth = PlayerHealthCanvas.Width;
			double percent = Engine.Player.CurrentHealth / (double)Engine.Player.Stats.BaseHealth;
			int currentHealthWidth = (int)(fullCanvasWidth * percent);
			Rectangle fullRectangle = new Rectangle(0, 0, fullCanvasWidth, PlayerHealthCanvas.Height);
			Rectangle currentRectangle = new Rectangle(0, 0, currentHealthWidth, PlayerHealthCanvas.Height);
			var fullBrush = new SolidBrush(Color.Green);
			var currentBrush = new SolidBrush(Color.LawnGreen);
			PlayerHealthCanvas.Draw(g =>
			{
				g.FillRectangle(fullBrush, fullRectangle);
				g.FillRectangle(currentBrush, currentRectangle);
				fullBrush.Dispose();
				currentBrush.Dispose();
			});
		}

		private void DrawEnemyHealth()
		{
			var enemy = Engine.Player.Room.Enemy;
			if (enemy is null)
			{
				EnemyHealthCanvas.Draw(null);
			}
			else
			{
				int fullCanvasWidth = EnemyHealthCanvas.Width;
				double percent = enemy.CurrentHealth / (double)enemy.Stats.BaseHealth;
				int currentHealthWidth = (int)(fullCanvasWidth * percent);
				Rectangle fullRectangle = new Rectangle(0, 0, fullCanvasWidth, EnemyHealthCanvas.Height);
				Rectangle currentRectangle = new Rectangle(0, 0, currentHealthWidth, EnemyHealthCanvas.Height);
				var fullBrush = new SolidBrush(Color.DarkRed);
				var currentBrush = new SolidBrush(Color.Red);
				EnemyHealthCanvas.Draw(g =>
				{
					g.FillRectangle(fullBrush, fullRectangle);
					g.FillRectangle(currentBrush, currentRectangle);
					fullBrush.Dispose();
					currentBrush.Dispose();
				});
			}
		}

		////////////////////////////////////////////////////////

		private void Tick(GameEngine.Engine.ActionRequest action)
		{
			Engine.Tick(action);
			DrawPlayerInfo();
		}

		private void SubscribeToPlayer(Player p)
		{
			p.Moved += this.Player_Moved;
			p.HealthChanged += (_, _) => DrawPlayerHealth();
			p.Defeated += (_, _) => this.Close();
			p.InvChanged += this.Player_InvChanged;
		}

		private void SubscribeToEnemy(Enemy e)
		{
			e.HealthChanged += (_, _) => DrawEnemyHealth();
			e.Defeated += (_, _) =>
			{
				this.EnemyCanvas.Draw(null);
				this.EnemyHealthCanvas.Draw(null);
			};
		}
	}
}
