using Diode_Dominion.Engine.Camera;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Diode_Dominion.DiodeDominion.World.Generators;
using Diode_Dominion.Engine.MouseInformation;

	namespace Diode_Dominion.DiodeDominion.World.Tiles
{
	/// <summary>
	/// Used to represent the grid of tiles that the world is created from
	/// </summary>
	public class TileMapping
	{
		#region Fields

		/// <summary>
		/// How many tiles from the top and bottom of the camera are to be
		/// updated and drawn
		/// </summary>
		private const int UpdateHeight = 20;

		/// <summary>
		/// How many tiles from the left and right of the camera are to be
		/// updated and drawn
		/// </summary>
		private const int UpdateWidth = UpdateHeight * 2;

		/// <summary>
		/// Defines the height of the entire world
		/// </summary>
		private int WorldTileHeight { get; } = 50;

		/// <summary>
		/// Defines the length of the entire world
		/// </summary>
		private int WorldTileLength { get; } = 50;

		/// <summary>
		/// Defines the tile width and height for all tiles
		/// </summary>
		private const int WorldTileSize = 100;

		#endregion

		#region Properties

		// Holds all of the games tiles
		public MapTile[,] Tiles { get; }

		#endregion

		/// <summary>
		/// Default world generation. Tiles will default to
		/// passed in type type
		/// </summary>
		public TileMapping(int sizeX, int sizeY, TileType tileType)
		{
			WorldTileLength = sizeX;
			WorldTileHeight = sizeY;

			Tiles = new MapTile[sizeY, sizeX];

			// Make rows
			for (int i = 0; i < sizeY; i++)
			{
				// Make columns
				for (int j = 0; j < sizeX; j++)
				{
					Tiles[i, j] = new MapTile(WorldTileSize, tileType, j * WorldTileSize, i * WorldTileSize);
				}
			}
		}
		/// <summary>
		/// Constructor for creating the tiles in a world. 
		/// </summary>
		/// <param name="seed">Seed used for world gen</param>
		public TileMapping(int seed)
		{
			IWorldGenerator worldGenerator = new DefaultWorldGenerator(WorldTileHeight, WorldTileLength, seed);	
			Tiles = worldGenerator.GenerateWorld();

		}
		/// <summary>
		/// This method contains a loop that calls the loadContent function for all 
		/// map tiles
		/// </summary>
		/// 
		/// <param name="content">Content manager that allows for easy 
		/// import of textures</param>
		public void LoadContent(ContentManager content)
		{
			// Load all of the sprites for the tiles
			for (int i = 0; i < Tiles.GetLength(0); i++)
			{
				/*
				int i1 = i;
				Parallel.For(0, Tiles.GetLength(1), j =>
				{
					Tiles[i1, j].LoadContent(content);
				});
				*/

				for (int j = 0; j < Tiles.GetLength(1); j++)
				{
					Tiles[i, j].LoadContent(content);
				}
			}
		}

		/// <summary>
		/// This method contains a loop that calls the draw function 
		/// of all map tiles
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch">Sprite Batch object. Used to 
		/// draw onto the screen</param>
		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			DetermineTileBounds(out int minXUpdate, out int updateToX, out int minYUpdate, out int updateToY);

			// Update the relevant tiles
			// Rows
			for (int i = minYUpdate; i < updateToY; i++)
			{
				// Columns
				for (int j = minXUpdate; j < updateToX; j++)
				{
					Tiles[i, j].Draw(gameTime, spriteBatch);
				}
			}

		}
		/// <summary>
		/// Updates tiles in the tile mapping. Determines which tiles are within bounds
		/// and update those tiles.
		/// </summary>
		/// <param name="gameTime"></param>
		public void Update(GameTime gameTime)
		{
			DetermineTileBounds(out int minXUpdate, out int updateToX, out int minYUpdate, out int updateToY);

			// Update the relevant tiles
			// Rows
			for (int i = minYUpdate; i < updateToY; i++)
			{
				// Columns
				for (int j = minXUpdate; j < updateToX; j++)
				{
					Tiles[i, j].Update(gameTime);
				}
			}
		}

		/// <summary>
		/// Used to determine which tiles should be updated and drawn to the screen.
		/// </summary>
		/// 
		/// <param name="minX">Minimum X tile to draw/update</param>
		/// <param name="maxX">Maximum X tile to draw/update</param>
		/// <param name="minY">Minimum Y tile to draw/update</param>
		/// <param name="maxY">Maximum Y tile to draw/update</param>
		private void DetermineTileBounds(out int minX, out int maxX, out int minY, out int maxY)
		{
			(float x, float y) = Camera.Location;

			// Grab the nearest tile to the camera's location
			int tileXNumber = (int) x / WorldTileSize;
			int tileYNumber = (int) y / WorldTileSize;

			// Set the minimum tiles that will be updated
			minX = tileXNumber - UpdateWidth;
			minY = tileYNumber - UpdateHeight;

			// Set how far down the 2D array they will be updated to
			maxX = tileXNumber + UpdateWidth;
			maxY = tileYNumber + UpdateHeight;

			// Stop from going out of bounds
			if (maxX > WorldTileLength)
			{
				maxX = WorldTileLength;
			}

			if (maxY > WorldTileHeight)
			{
				maxY = WorldTileHeight;
			}

			if (minX < 0)
			{
				minX = 0;
			}

			if (minY < 0)
			{
				minY = 0;
			}
		}
		/// <summary>
		/// Returns a list of tiles that are selected
		/// </summary>
		/// <returns></returns>
		public List<MapTile> ObtainSelectedTiles()
		{
			List<MapTile> selectedTiles = new List<MapTile>();
			//Needs slight optimization so we count only the tiles in the needed area
			if (MouseSelection.SelectedRegion.Width > 0 && MouseSelection.SelectedRegion.Height > 0)
			{
				for (int i = 0; i < Tiles.GetLength(0); i++)
				{
					for (int j = 0; j < Tiles.GetLength(1); j++)
					{
						if (MouseSelection.SelectedRegion.Intersects((Tiles[i, j].TileSprite.Bounds)))
							selectedTiles.Add(Tiles[i, j]);
					}
				}
			}
			return selectedTiles;
		}
	}
}
