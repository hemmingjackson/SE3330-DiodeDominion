using System;
using System.Threading.Tasks;
using Diode_Dominion.DiodeDominion.World.Tiles;

namespace Diode_Dominion.DiodeDominion.World.Generators
{
	/// <summary>
	/// This class is responsible for generating all tiles in the world
	/// Currently it generates mountains and water pools
	/// </summary>
	public class DefaultWorldGenerator : IWorldGenerator
	{

		#region Fields
		private const int TileOffset = 1;
		/// <summary>
		/// Direction used for tiles placed in the west direction
		/// </summary>
		private const int West = 4;
		/// <summary>
		/// Direction used for tiles placed in the south direction
		/// </summary>
		private const int South = 3;
		/// <summary>
		/// Direction used for tiles placed in the east direction
		/// </summary>
		private const int East = 2;
		/// <summary>
		/// Direction used for tiles placed in the north direction
		/// </summary>
		private const int North = 1;
		/// <summary>
		/// Cap of values for the rand function 
		/// </summary>
		private const int RandCap = 5;

		/// <summary>
		/// Size of tile
		/// </summary>
		private const int TileSize = 100; 
		/// <summary>
		/// Length of a map tile
		/// </summary>
		private const int TileLength = 100;
		/// <summary>
		/// Height of map tile
		/// </summary>
		private const int TileHeight = 100;
		/// <summary>
		/// Holds the map tiles that are being generated
		/// </summary>
		private readonly MapTile[,] mapTiles;
		/// <summary>
		/// Rand variable that generates "random" numbers
		/// </summary>
		private readonly Random rand;
		#endregion
		/// <summary>
		/// Default constructor that sets seed
		/// </summary>
		/// <param name="worldHeight">Tile height of the world</param>
		/// <param name="worldWidth">Tile width of the world</param>
		/// <param name="seed">seed that will be used for the random number generator</param>
		public DefaultWorldGenerator(int worldHeight, int worldWidth, int seed)
		{
			rand = new Random(seed);
			mapTiles = new MapTile[worldHeight, worldWidth];
		}
		/// <summary>
		///  Creates and returns an 2d array of the world that has mountains and pools generated.
		/// </summary>
		/// <returns>A list of the map tiles</returns>
		public MapTile[,] GenerateWorld()
		{	
			Parallel.For(0, mapTiles.GetLength(0), i =>
			{
				for (int j = 0; j < mapTiles.GetLength(1); j++)
				{
					mapTiles[i, j] = new MapTile(TileSize, TileType.DIRT, j * TileLength, i * TileHeight);
				}
			});
			PlaceMountainTiles();
			PlaceWaterTiles();
			return mapTiles; 
		}
		/// <summary>
		/// Picks a random location that is currently dirt to generate a water tile.
		/// Water tiles will then generate randomly from each cardinal direction
		/// </summary>
		private void PlaceWaterTiles()
		{
			for (int i = 0; i < rand.Next(5, mapTiles.GetLength(1)); i++)
			{
				bool locationPicked = false;
				while (!locationPicked)
				{ 
					int startingYCord = rand.Next(RandCap, mapTiles.GetLength(0) - 1);
					int startingXCord = rand.Next(RandCap, mapTiles.GetLength(1) - 1);
					if(mapTiles[startingYCord,startingXCord].TypeOfTile == TileType.DIRT)
					{
						mapTiles[startingYCord, startingXCord].ChangeTileType(TileType.WATER);
						//mapTiles[startingYCord, startingXCord].LoadContent(Settings.Content);

						PlaceTile(startingYCord, startingXCord - 1, mapTiles.GetLength(0), TileType.WATER);
						PlaceTile(startingYCord + 1, startingXCord, mapTiles.GetLength(0), TileType.WATER);
						PlaceTile(startingYCord, startingXCord + 1, mapTiles.GetLength(0), TileType.WATER);
						PlaceTile(startingYCord - 1, startingXCord, mapTiles.GetLength(0), TileType.WATER);
						locationPicked = true;
					}
				}
			}
		}
		/// <summary>
		/// Picks a random location that is currently dirt to generate a mountain tile.
		/// Water tiles will then generate randomly from each cardinal direction
		/// </summary>
		private void PlaceMountainTiles()	
		{
			for(int i = 0; i < 3; i++)
			{
				int startingYCord = rand.Next(RandCap, mapTiles.GetLength(0) - TileOffset);
				int startingXCord = rand.Next(RandCap, mapTiles.GetLength(1) - TileOffset);

				mapTiles[startingYCord, startingXCord].ChangeTileType(TileType.STONE);
				//mapTiles[startingYCord, startingXCord].LoadContent(Settings.Content);

				PlaceTile(startingYCord, startingXCord - TileOffset, mapTiles.GetLength(0), TileType.STONE);
				PlaceTile(startingYCord + TileOffset, startingXCord, mapTiles.GetLength(0), TileType.STONE);
				PlaceTile(startingYCord, startingXCord + TileOffset, mapTiles.GetLength(0), TileType.STONE);
				PlaceTile(startingYCord - TileOffset, startingXCord, mapTiles.GetLength(0), TileType.STONE);
			}
		}
		/// <summary>
		/// Places a tile and then looks for the next location to place a tile adjacent to it.
		/// Tiles left decreases by a random amount each time a tile is place. This will
		/// continue to place tiles until tiles left is less than or equal to zero
		/// </summary>
		/// <param name="yCord">y coordinate of tile</param>
		/// <param name="xCord">x coordinate of the tile</param>
		/// <param name="placementPoints">tiles left to place</param>
		/// <param name="type"></param>
		private void PlaceTile(int yCord, int xCord, int placementPoints, TileType type)
		{
			mapTiles[yCord, xCord].ChangeTileType(type);
				//mapTiles[yCord, xCord].LoadContent(Settings.Content);
		

			if (placementPoints >= 0 && yCord >= 0 && xCord >= 0 )
			{
				bool tilePicked = false; 
				while(!tilePicked && placementPoints > 0)
				{
					//North is 1, West is 4
					int direction = rand.Next(1, RandCap);
					if(direction == North && yCord - TileOffset >= 0 && mapTiles[yCord - TileOffset, xCord].TypeOfTile == TileType.DIRT)
					{
						PlaceTile(yCord - TileOffset, xCord, placementPoints - rand.Next(TileOffset, RandCap), type);	
						tilePicked = true;
					}
					else if (direction == East && xCord + TileOffset < mapTiles.GetLength(1) && mapTiles[yCord, xCord + TileOffset].TypeOfTile == TileType.DIRT)
					{
						PlaceTile(yCord, xCord + TileOffset, placementPoints - rand.Next(TileOffset, RandCap), type);	
						tilePicked = true;
					}	
					else if (direction == South && yCord + TileOffset  < mapTiles.GetLength(0) && mapTiles[yCord + TileOffset, xCord].TypeOfTile == TileType.DIRT)
					{
						PlaceTile(yCord + TileOffset, xCord, placementPoints - rand.Next(TileOffset, RandCap), type);
						tilePicked = true;
					}
					else if(direction == West && xCord - TileOffset >= 0 && mapTiles[yCord, xCord - TileOffset].TypeOfTile == TileType.DIRT)
					{
						PlaceTile(yCord, xCord - TileOffset, placementPoints - rand.Next(TileOffset, RandCap), type);
						tilePicked = true;
					}
					placementPoints--;
				}
			}
		}
	}
}
