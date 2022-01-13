using System;
using System.Collections.Generic;
using System.Linq;
using Diode_Dominion.DiodeDominion.Entities;
using Diode_Dominion.DiodeDominion.Entities.StaticEntities.Trees;
using Diode_Dominion.DiodeDominion.World.Tiles;
using Microsoft.Xna.Framework;

namespace Diode_Dominion.DiodeDominion.World.Generators
{
	/// <summary>
	/// Generates all of the trees for the game world.
	/// Starts by taking all of the <see cref="MapTile"/> that
	/// make up the world and adding trees to different locations
	/// where dirt exists.
	/// </summary>
	public class TreeGenerator : IGenerate
	{
		#region Fields
		
		private const int TreeHeight = 100;
		private const int LowerBoundTreeGenChance = -50;
		private const int UpperBoundTreeGenChance = 3;
		private const int TileSize = 100;
		private const int MinimumHealthVariance = -10;
		private const int MaximumHealthVariance = 500;

		/// <summary>
		/// Map tiles that make up the world
		/// </summary>
		private readonly List<MapTile> mapTiles;

		/// <summary>
		/// Random object that determines whether trees will be placed
		/// </summary>
		private readonly Random random;

		#endregion

		#region Constructors

		/// <summary>
		/// Create a generator object to populate the world with trees
		/// </summary>
		/// 
		/// <param name="mapTiles">Map tiles that make up the world</param>
		/// <param name="random"></param>
		public TreeGenerator(MapTile[,] mapTiles, Random random)
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
		/// Generate the <see cref="Tree"/> for the game world
		/// </summary>
		/// 
		/// <returns>Enumerable of the trees for the game world</returns>
		public IEnumerable<Entity> Generate()
		{
			List<Tree> trees = new List<Tree>();

			// Iterate over every map tile
			foreach (MapTile tile in mapTiles.Where(tile => tile.TypeOfTile == TileType.DIRT))
			{
				trees.AddRange(GenerateTreeSubset(tile.TileSprite.Origin));
			}

			return trees;
		}

		/// <summary>
		/// Generate trees near the map tile
		/// </summary>
		/// 
		/// <param name="mapTileLocation">Location of the map tile</param>
		/// 
		/// <returns>Trees generated around the tile</returns>
		private IEnumerable<Tree> GenerateTreeSubset(Vector2 mapTileLocation)
		{
			List<Tree> trees = new List<Tree>();

			// Make a random number of trees on a dirt tile
			for (int i = 0; i < random.Next(LowerBoundTreeGenChance, UpperBoundTreeGenChance); i++)
			{
				// Create a new tree
				Tree tree = new Tree();

				// Randomly change the tree's location
				float updateX = mapTileLocation.X + random.Next(0, TileSize);
				float updateY = mapTileLocation.Y + random.Next(0, TileSize) - TreeHeight;

				tree.EntitySprite.Origin = new Vector2(updateX, updateY);
				tree.Health += random.Next(MinimumHealthVariance, MaximumHealthVariance);
				tree.MaxHealth = tree.Health;

				trees.Add(tree);
			}

			return trees;
		}
		
		#endregion
	}
}