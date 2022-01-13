using System;
using System.Collections.Generic;
using System.Linq;
using Diode_Dominion.DiodeDominion.Entities;
using Diode_Dominion.DiodeDominion.Entities.StaticEntities.Ores;

using Diode_Dominion.DiodeDominion.World.Tiles;
using Microsoft.Xna.Framework;

namespace Diode_Dominion.DiodeDominion.World.Generators
{
	/// <summary>
	/// Generates all of the ores for the game world.
	/// Starts by taking all of the <see cref="MapTile"/> that
	/// make up the world and adding ores to different locations
	/// where dirt exists.
	/// </summary>
	public class OreGenerator : IGenerate
	{
		#region Fields

		/// <summary>
		/// Map tiles that make up the world
		/// </summary>
		private readonly List<MapTile> mapTiles;

		/// <summary>
		/// Random object that determines whether ores will be placed
		/// </summary>
		private readonly Random random;

		#endregion

		#region Constructors

		/// <summary>
		/// Create a generator object to populate the world with ores
		/// </summary>
		/// 
		/// <param name="mapTiles">Map tiles that make up the world</param>
		/// <param name="random"></param>
		public OreGenerator(MapTile[,] mapTiles, Random random)
		{
			List<MapTile> tiles = new List<MapTile>();
			this.random = random;

			for (int i = 1; i < mapTiles.GetLength(0); i++)
			{
				for (int j = 1; j < mapTiles.GetLength(1); j++)
				{
					if (mapTiles[i, j].TypeOfTile == TileType.DIRT)
					{
						tiles.Add(mapTiles[i, j]);
					}
				}
			}

			this.mapTiles = tiles;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Generate the <see cref="Ore"/> for the game world
		/// </summary>
		/// 
		/// <returns>Enumerable of the ores for the game world</returns>
		public IEnumerable<Entity> Generate()
		{
			List<Ore> ores = new List<Ore>();

			// Iterate over every map tile
			foreach (MapTile tile in mapTiles.Where(tile => tile.TypeOfTile == TileType.DIRT))
			{
				ores.AddRange(GenerateOreSubset(tile.TileSprite.Origin));
			}

			return ores;
		}

		/// <summary>
		/// Generate ores near the map tile
		/// </summary>
		/// 
		/// <param name="mapTileLocation">Location of the map tile</param>
		/// 
		/// <returns>ores generated around the tile</returns>
		private IEnumerable<Ore> GenerateOreSubset(Vector2 mapTileLocation)
		{
			List<Ore> ores = new List<Ore>();

			// Make a random number of ores on a dirt tile
			for (int i = 0; i < random.Next(-50, 3); i++)
			{
				// Create a new ore
				Ore ore = new Ore
				{
					EntitySprite =
					{
						Origin = mapTileLocation
					}
				};

				// Randomly change the ores location
				float updateX = mapTileLocation.X + random.Next(0, 100) - ore.EntitySprite.Width;
				float updateY = mapTileLocation.Y + random.Next(0, 100) - ore.EntitySprite.Height;

				ore.EntitySprite.Origin = new Vector2(updateX, updateY);
				ore.Health += random.Next(-10, 500);
				ore.MaxHealth = ore.Health;

				ores.Add(ore);
			}

			return ores;
		}

		#endregion
	}
}