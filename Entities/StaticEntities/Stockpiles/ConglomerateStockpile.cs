using Diode_Dominion.DiodeDominion.World.Tiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Diode_Dominion.DiodeDominion.Entities.Items;

namespace Diode_Dominion.DiodeDominion.Entities.StaticEntities.Stockpiles
{
	/// <summary>
	/// Class used to hold any stockpile zones that are created in the game world.
	/// Each time a stockpile is created via the UI
	/// </summary>
	public class ConglomerateStockpile
	{
		#region Fields

		/// <summary>
		/// Any stockpile zone contained in the area
		/// </summary>
		public List<StockpileZone> StockpileZones { get; }
		/// <summary>
		/// Center location for the stockpile
		/// </summary>

		public Vector2 CenterLocation { get;}

		#endregion

		/// <summary>
		/// Default constructor for a conglomerate stockpile.
		/// Checks mouse selection against what tiles are selected. Any valid 
		/// tiles that are selected are added to a list.
		/// </summary>
		public ConglomerateStockpile(IEnumerable<MapTile> selectedTiles)
		{
			float minX = float.MaxValue;
			float minY = float.MaxValue;
			float maxX = 0;
			float maxY = 0;

			StockpileZones = new List<StockpileZone>();
			foreach(MapTile tile in selectedTiles)
			{
				if(tile.TypeOfTile == TileType.DIRT)
				{
					StockpileZones.Add(new StockpileZone(tile));

					// Find the min x and y position
					if (tile.Location.X < minX)
					{
						minX = tile.Location.X;
					}

					if (tile.Location.Y < minY)
					{
						minY = tile.Location.Y;
					}

					// Find the max x and y position
					if (tile.Location.X > maxX)
					{
						maxX = tile.Location.X;
					}

					if (tile.Location.Y > maxY)
					{
						maxY = tile.Location.Y;
					}
				}
			}

			// Find the true center and set the location
			float centerX = (maxX - minX) / 2 + minX;
			float centerY = (maxY - minY) / 2 + minY;

			CenterLocation = new Vector2(centerX, centerY);
		}
		/// <summary>
		/// Looks for the first available spot in a stockpile and adds the item
		/// </summary>
		/// <param name="item"></param>
		public void AddItem(Item item)
		{
			StockpileZone zone = null;

			// Look for the first open stockpile zone
			foreach (StockpileZone stockpileZone in StockpileZones
				.SelectMany(stockpileZone => stockpileZone.StockpileItems,
					(stockpileZone, itemInStockpile) => new {stockpileZone, itemInStockpile})
				.Where(t => zone == null && t.itemInStockpile == null)
				.Select(t => t.stockpileZone))
			{
				zone = stockpileZone;
				zone.AddItem(item);
			}
		}

		/// <summary>
		/// Passes game time and sprite batch so that items in the stockpiles can be drawn
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			foreach (StockpileZone zone in StockpileZones)
			{
				zone.Draw(gameTime, spriteBatch);
			}
				
		}
		/// <summary>
		/// Returns a boolean true if any tiles passed in the list are touching a stockpile 
		/// zone tile. 
		/// </summary>
		/// <param name="selectedTiles"></param>
		/// <returns></returns>
		public bool IsTouchingStockpile(IEnumerable<MapTile> selectedTiles)
		{
			//Iterate through map tiles passed
			foreach(MapTile tile in selectedTiles)
			{
				//Iterate through zones in a stockpile
				foreach(StockpileZone zone in StockpileZones)
				{
					if (zone.IsTouching(tile))
						return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Adds tiles to the a current stockpile zone. Determines which tiles are already part of the stockpile
		/// If a tile is already part of the stockpile, it does not get added
		/// </summary>
		public void AddTiles(IEnumerable<MapTile> addedTiles)
		{
			IEnumerable<MapTile> mapTiles = addedTiles.ToList();

			for (int i = 0; i < mapTiles.ToList().Count; i++)
			{
				//Iterate through the list of tiles to add
				bool notColliding = true;
				for (int j = 0; j < StockpileZones.Count && notColliding; j++)
				{
					if (StockpileZones[j].IsSameTileLocation(mapTiles.ToList()[i].Location.X, mapTiles.ToList()[i].Location.Y))
						notColliding = false;
				}
				if (notColliding && mapTiles.ToList()[i].TypeOfTile == TileType.DIRT)
					StockpileZones.Add(new StockpileZone(mapTiles.ToList()[i]));
			}
		}
		/// <summary>
		/// Retrieves the items in a stockpile
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Item> GetItems()
		{
			List<Item> itemsToReturn = new List<Item>();
			foreach(StockpileZone zone in StockpileZones)
			{
				foreach(Item item in zone.StockpileItems)
				{
					if (item != null)
						itemsToReturn.Add(item);
				}
			}
			return itemsToReturn;
		}
		/// <summary>
		/// Returns if the stockpile is full
		/// </summary>
		/// <returns></returns>
		public bool IsFull()
		{
			int countOfItems = 0;
			foreach(StockpileZone zone in StockpileZones)
			{
				foreach(Item item in zone.StockpileItems)
				{
					if (item != null)
						countOfItems++;
				}
			}
			return (countOfItems == StockpileZones.Count * 4);
		}
	}
}
