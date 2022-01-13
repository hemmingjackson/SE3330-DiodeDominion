using Diode_Dominion.Engine.Events;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Diode_Dominion.DiodeDominion.Entities.AI.SpecificAIs;
using Diode_Dominion.DiodeDominion.Entities.Items;
using Diode_Dominion.DiodeDominion.Events;
using Diode_Dominion.Engine.Sprites;
using Diode_Dominion.DiodeDominion.World.Tiles;

namespace Diode_Dominion.DiodeDominion.Entities
{
	/// <summary>
	/// Defines the common behavior for an entity
	/// </summary>
	public class Entity
	{
		#region Fields

		/// <summary>
		/// Holds the value of the next Entity id
		/// </summary>
		private static int _incrementalIds = 1;

		/// <summary>
		/// Coordinates of the Entity
		/// </summary>
		private Vector2 coordinates;
		
		#endregion

		#region Properties

		/// <summary>
		/// Name of the BaseEntity in the game world
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Id of a specific entity. Must be unique.
		/// Do not allow the setting of this property outside of this class.
		/// Id will be handled internally
		/// </summary>
		public int Id { get; }

		/// <summary>
		/// Current health of an entity
		/// </summary>
		public double Health { get; set; }

		/// <summary>
		/// The max health of the BaseEntity. It's health should never go above this value
		/// </summary>
		public double MaxHealth { get; set; }

		/// <summary>
		/// Determines whether or not the entity is still interact-able in the game world.
		/// </summary>
		public bool Alive { get; set; } = true;

		/// <summary>
		/// Items that the BaseEntity is currently holding
		/// </summary>
		public List<Item> Items { get; }

		/// <summary>
		/// The sprite representation of the entity
		/// </summary>
		public Sprite EntitySprite { get; set; }

		/// <summary>
		/// AI that will dictate the general behavior
		/// </summary>
		public BaseAi BaseAi { get; set; }
		
		/// <summary>
		/// The coordinate location of the Entity
		/// </summary>
		public Vector2 Coordinate
		{
			get => coordinates;
			set => coordinates = value;
		}

		/// <summary>
		/// The speed at which the entity is able to move when it updates
		/// </summary>
		public float MovementSpeed { get; set; } = 1f;
		
		/// <summary>
		/// Returns whether the entity is attempting to complete a standard
		/// action task.
		/// </summary>
		public bool HasTask => BaseAi.ActionsRemaining() != 0;

		#endregion

		#region Constructors

		/// <summary>
		/// Generic BaseEntity creation. This is useful when finer control of the BaseEntity
		/// creation is needed after instantiation.
		/// </summary>
		public Entity()
		{
			Items = new List<Item>();

			Health = MaxHealth = 50.0d;
			Id = _incrementalIds++;
			Name = "Unknown";
			coordinates.X = 500;
			coordinates.Y = 500;
			EntitySprite = new Sprite(coordinates);
			//EntitySprite = new OverlaySprite(Coordinates);

			BaseAi = new BaseAi(this, null);
		}

		/// <summary>
		/// Explicit BaseEntity instantiation. Every field that must be set in an entity is here.
		/// </summary>
		/// <param name="name">Name of the BaseEntity in the game world</param>
		/// <param name="startingHealth">Max starting health of the BaseEntity</param>
		/// <param name="tiles"></param>
		public Entity(string name, double startingHealth, MapTile[,] tiles)
		{
			Items = new List<Item>();

			Health = MaxHealth = startingHealth;
			Id = _incrementalIds++;
			Name = name;
			coordinates.X = 500;
			coordinates.Y = 500;
			EntitySprite = new Sprite(coordinates);
			//EntitySprite = new OverlaySprite(Coordinates, entityTexture);
			BaseAi = new BaseAi(this, tiles);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Sets the texture of the entity
		/// </summary>
		/// 
		/// <param name="entityTexture"></param>
		internal void SetTexture(Texture2D entityTexture)
		{
			EntitySprite.Texture = entityTexture;
		}

		/// <summary>
		/// Add an item that can be held to an BaseEntity. 
		/// Can not hold the same item object more than once
		/// </summary>
		/// 
		/// <param name="holdable">Item that can be held by an BaseEntity</param>
		/// 
		/// <returns>Whether the item was added.</returns>
		public bool AddHoldable(Item holdable)
		{
			bool itemAdded = false;

			// Check if item is not already being held
			if (!Items.Contains(holdable))
			{
				Items.Add(holdable);
				itemAdded = true;
			}

			return itemAdded;
		}

		/// <summary>
		/// Removes a holdable from the BaseEntity
		/// </summary>
		/// 
		/// <param name="holdable">Held item to remove</param>
		/// 
		/// <returns>Whether the item was removed</returns>
		public bool RemoveHoldable(Item holdable)
		{
			return Items.Remove(holdable);
		}

		/// <summary>
		/// Removes a holdable from the BaseEntity based on the item's Id
		/// </summary>
		/// 
		/// <param name="holdableId">Id of the item</param>
		/// 
		/// <returns>Whether the item was removed from the list</returns>
		public bool RemoveHoldable(int holdableId)
		{
			bool itemRemoved = false;

			// Loop through looking for a matching item id
			for (int i = 0; i < Items.Count; i++)
			{
				// Check if item ids match
				if (Items[i].Id == holdableId)
				{
					Items.RemoveAt(i);
					itemRemoved = true;

					// Push it out of range once removed
					i = Items.Count + 1;
				}
			}
			return itemRemoved;
		}

		/// <summary>
		/// Returns the list of items that the BaseEntity is holding
		/// </summary>
		/// 
		/// <returns>List of held items</returns>
		public List<Item> Holdables()
		{
			return Items;
		}

		/// <summary>
		/// Updates the BaseEntity based on events that the BaseEntity can act upon.
		/// Can contain events that Entities do not care about. Only listen
		/// for events that the BaseEntity should care about.
		/// 
		/// Child classes must override this method for unique behavior.
		/// </summary>
		/// 
		/// <param name="eventFired">Event fired. Cast up to events that the Entity should care about</param>
		public virtual void Update(BaseEvent eventFired)
		{
			BaseAi.Update(eventFired);
		}


		internal virtual void Update(GameTime gameTime)
		{
			EntitySprite.Update(gameTime);
		}

		public virtual void UpdateInventoryEvent(UpdateInventoryEvent updateInventoryEvent) { }

		/// <summary>
		/// Allows for drawing of the entity
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		internal virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			EntitySprite.Draw(gameTime, spriteBatch);
		}

		/// <summary>
		/// Handle telling the AI to update the entity if there is
		/// anything that should occur
		/// </summary>
		public virtual void Update()
		{
			BaseAi.Update();
		}

		public override bool Equals(object obj)
		{
			return obj is Entity entity && entity.Id == Id;
		}

		public override int GetHashCode()
		{
			return Id * 1333444584;
		}

		#endregion
	}
}
