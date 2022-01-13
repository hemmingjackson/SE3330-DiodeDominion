using Diode_Dominion.DiodeDominion.Entities.Items;
using Diode_Dominion.DiodeDominion.World.Tiles;
using Diode_Dominion.Engine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Diode_Dominion.DiodeDominion.Entities.StaticEntities.Stockpiles
{
	/// <summary>
	/// Class used to hold the data for which items are stored in a specific stockpile
	/// "tile". Allows a user to add and remove item from a stockpile
	/// </summary>
	public class StockpileZone 
	{
		#region Fields	

		/// <summary>
		/// Defines the maximum amount of different item that can be stored in a tile at once
		/// </summary>
		private const int MaxItemCap = 4;

		/// <summary>
		/// Defines halfway point in a tile. Used to determine where an item should be placed
		/// Used since we can't reference tile sprite width without crashing unit tests
		/// </summary>
		private const int HalfWayPoint = 50;

		/// <summary>
		/// Array of items to hold the different items stored on this stockpile
		/// </summary>
		public Item[] StockpileItems { get; }

		/// <summary>
		/// Reference to the tile that the stockpile is on
		/// </summary>
		private readonly MapTile tileLocation;

		/// <summary>
		/// Sprite of stockpile zone
		/// </summary>
		private readonly Sprite sprite;

		/// <summary>
		/// Width of stockpile zone sprite;
		/// </summary>
		private readonly int width;

		/// <summary>
		/// Height of stockpile zone sprite
		/// </summary>
		private readonly int height;

		/// <summary>
		/// Opacity of stockpile zone sprite
		/// </summary>
		private const float Opacity = 0.25f;

		#endregion

		/// <summary>
		/// Constructor for a stockpile zone. Instantiates the stockpile items to be able to store 
		/// 4 different items
		/// </summary>
		/// <param name="tile"></param>
		public StockpileZone(MapTile tile)
		{
			tileLocation = tile;
			StockpileItems = new Item[MaxItemCap];

			width = tile.TileSprite?.Width ?? 0;
			height = tile.TileSprite?.Height ?? 0;

			sprite = new Sprite(tile.TileSprite?.Origin ?? new Vector2(0, 0));
		}

		/// <summary>
		/// Adds an item to the stockpile
		/// </summary>
		/// <param name="newItem">Item to be stored</param>
		/// <returns>Boolean true if the item is added, else false if 
		/// the zone is full</returns>
		public bool AddItem(Item newItem)
		{
			for (int i = 0; i < StockpileItems.Length; i++)
			{
				if(StockpileItems[i] == null)
				{
					StockpileItems[i] = newItem;
					//Determines the coordinate of item in the stockpile
					int xCord = 0;
					int yCord = 0;
					if ((i + 1) % 2 == 0)
						xCord = HalfWayPoint;
					if (i > 1)
						yCord = HalfWayPoint;
					StockpileItems[i].EntitySprite.Origin = new Vector2(tileLocation.Location.X + xCord, tileLocation.Location.Y + yCord);
					StockpileItems[i].Coordinates.X = StockpileItems[i].EntitySprite.Origin.X;
					StockpileItems[i].Coordinates.Y = StockpileItems[i].EntitySprite.Origin.Y;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Removes an item from a stockpile zone based on the id
		/// </summary>
		/// <param name="id">Id of the item</param>
		public void RemoveItem(int id)
		{
			for (int i = 0; i < StockpileItems.Length; i++)
			{
				if (StockpileItems[i]?.Id == id)
				{
					StockpileItems[i] = null;
				}
			}
		}

		/// <summary>
		/// Passes game time and sprite batch so that items in the stockpiles can be drawn
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			// Make certain the texture is set
			if (sprite.Texture != null)
			{
				sprite.Draw(gameTime, spriteBatch);
			}
			else
			{
				Color[] colors = new Color[width * height];
				for (int i = 0; i < colors.Length; i++)
				{
					colors[i] = Color.Beige * Opacity;
				}

				sprite.Texture = new Texture2D(Settings.SpriteBatch.GraphicsDevice, width, height);
				sprite.Texture.SetData(colors);
			}
			
			foreach(Item item in StockpileItems)
			{
				item?.Draw(gameTime, spriteBatch);
			}
				
		}

		/// <summary>
		/// Returns a boolean true is the passed tile is "touching" the stockpile zone.
		/// A "Touching" tile is defined to be any tile that is adjacent to the stockpile zone
		/// </summary>
		/// <param name="tile"></param>
		/// <returns></returns>
		public bool IsTouching(MapTile tile)
		{
			if (IsSameTileLocation(tile.Location.X, tile.Location.Y))
				return true;
			if (tile.Location.X == tileLocation.Location.X)
			{
				if (tile.Location.Y == tileLocation.Location.Y - tileLocation.Location.Height || tile.Location.Y == tileLocation.Location.Y + tileLocation.Location.Height)
					return true;
			}
			else if (tile.Location.Y == tileLocation.Location.Y)
			{
				if (tile.Location.X == tileLocation.Location.X - tileLocation.Location.Width || tile.Location.X == tileLocation.Location.X + tileLocation.Location.Width)
					return true;
			}
			return false; 
		}
		/// <summary>
		/// Returns a boolean true if the tile that is passed is in the same location as the stockpile zone location
		/// </summary>
		/// <returns></returns>
		public bool IsSameTileLocation(int xCord, int yCord)
		{
			return (xCord == tileLocation.Location.X && yCord == tileLocation.Location.Y);
		}
	}
}
