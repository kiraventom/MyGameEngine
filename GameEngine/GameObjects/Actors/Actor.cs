using GameEngine.GameObjects.Usables.Items;
using GameEngine.Events;
using System.Collections.Specialized;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GameEngine.GameObjects.Rooms;

namespace GameEngine.GameObjects.Actors
{
	public abstract class Actor : IGameObject 
	{
		protected Actor() : base()
		{
			CurrentHealth = Stats.BaseHealth;
			Inventory = new ObservableCollection<Item>();
			Inventory.CollectionChanged += (s, ea) =>
			{
				InventoryChangedEventArgs icea;
				icea = ea.Action switch
				{
					NotifyCollectionChangedAction.Add => new InventoryChangedEventArgs(ea.NewItems[0] as Item, ea.Action, this),
					NotifyCollectionChangedAction.Remove => new InventoryChangedEventArgs(ea.OldItems[0] as Item, ea.Action, this),

					_ => throw new NotImplementedException()
				};

				this.InvChanged?.Invoke(this, icea);
			};

			this.StartingInventory.ToList().ForEach(i => Inventory.Add(i));
		}

		// IGameObject
		public abstract string Name { get; }

		// Overridable
		public virtual uint Level => Stats.Level;
		protected virtual IEnumerable<Item> StartingInventory => Enumerable.Empty<Item>();

		// Properties
		public Balance.Stats Stats => Balance.Tables.StatsTable.GetStats(this.GetType());
		public bool IsAlive => CurrentHealth > 0;
		public uint CurrentHealth { get; private set; }
		internal ObservableCollection<Item> Inventory { get; }
		public Room Room { get; private set; }

		// Events
		public event EventHandler<HealthChangedEventArgs> HealthChanged;
		public event EventHandler<DefeatedEventArgs> Defeated;
		public event EventHandler<AttackedEventArgs> Attacked;
		public event EventHandler<InventoryChangedEventArgs> InvChanged;

		// Methods
		internal static void AcquireRoomToActor(Room room, Actor actor)
		{
			if (room is null)
				throw new ArgumentNullException(nameof(room));

			actor.Room = room;
		}

		public IReadOnlyList<Item> GetInventory() => Inventory.ToList();

		internal virtual void Attack(Actor defender) => this.Attack(defender, this.Stats.GetStrenght());

		protected void Attack(Actor defender, uint strenght)
		{
			if (defender is null)
				throw new ArgumentNullException(nameof(defender));

			var ea = new AttackedEventArgs(this, defender, strenght);
			Attacked.Invoke(this, ea);
			defender.ReceiveDamage(strenght, this);
		}

		internal virtual void ReceiveDamage(uint amount, IGameObject source) => ChangeHealth(-(int)amount, source);

		internal virtual void GainHealth(uint amount, IGameObject source) => ChangeHealth((int)amount, source);

		private void ChangeHealth(int change, IGameObject source)
		{
			int newHealth = (int)CurrentHealth + change;
			if (newHealth < 0)
				newHealth = 0;
			if (newHealth > Stats.BaseHealth)
				newHealth = (int)Stats.BaseHealth;

			CurrentHealth = (uint)newHealth;
			HealthChanged.Invoke(this, new HealthChangedEventArgs(change, this, source));

			if (!IsAlive)
				Defeated.Invoke(this, new DefeatedEventArgs(this, source));
		}
	}
}
