using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Diode_Dominion.DiodeDominion.Entities.AI.SpecificAIs;
using Diode_Dominion.DiodeDominion.World.Tiles;

namespace Diode_Dominion.DiodeDominion.Entities.Items
{
	/// <summary>
	/// Base item class that dictates common behavior 
	/// </summary>
	public class Item : Entity
	{
		#region Fields

		private const int OutOfBoundsLocation = -5000;
		private const int DefaultCoordinate = 500;
		private const int DropX = 40;
		private const int DropY = 25;

		public Vector2 Coordinates;
		public bool IsInInventory;

		#endregion

		#region Properties

		/// <summary>
		/// Returns whether the items is a type of tool
		/// </summary>
		public bool IsTool { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public ItemType ItemType { get; set; }

		/// <summary>
		/// Holds a reference to the entity that is holding this item
		/// </summary>
		public Entity HoldingEntity { get; private set; }
		#endregion

		#region Constructor

		/// <summary>
		/// Default constructor
		/// </summary>
		public Item()
		{
			// Force a new AI of this object type
			BaseAi = new ItemAI(this, null);
		}

		public Item(string name, double startingHealth, MapTile[,] tiles) : base(name, startingHealth, null)
		{
			Coordinates.X = DefaultCoordinate;
			Coordinates.Y = DefaultCoordinate;

			// Force a new AI of this object type
			BaseAi = new ItemAI(this, tiles);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Determines whether the items are equal
		/// </summary>
		/// 
		/// <param name="item">Item to check if equal to</param>
		/// <returns></returns>
		public virtual bool Equals(Item item)
		{
			return Id == item.Id;
		}


		/// <summary>
		/// Method to draw each time
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		internal override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			// Only draw if it is not in an inventory
			if (!IsInInventory)
			{
				EntitySprite.Draw(gameTime, spriteBatch);
			}
		}

		/// <summary>
		/// Method to detect when an entity is picked up
		/// </summary>
		/// <param name="entity"></param>
		public void Pickup(Entity entity)
		{
			if (!IsInInventory)
			{
				HoldingEntity = entity;
				IsInInventory = true;

				// Set the location of it somewhere nothing should be able to get to
				EntitySprite.Origin = new Vector2(OutOfBoundsLocation, OutOfBoundsLocation);
				entity.AddHoldable(this);
			}
		}

		/// <summary>
		/// Method to drop the item from the entities inventory
		/// </summary>
		public void Drop()
		{
			// Do not attempt to remove item when no one is holding it
			if (IsInInventory)
			{
				

				// Set the new location to where the entity that was holding it is
				EntitySprite.Origin = new Vector2(HoldingEntity.EntitySprite.Origin.X + DropX,
					HoldingEntity.EntitySprite.Origin.Y + DropY);

				IsInInventory = false;

				// Remove this item from the entity
				HoldingEntity.Holdables().Remove(this);
				HoldingEntity = null;
			}
		}

		#endregion
	}
}