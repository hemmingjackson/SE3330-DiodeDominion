using Diode_Dominion.DiodeDominion.Textures;
using Diode_Dominion.Engine.MouseInformation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Diode_Dominion.Engine.Sprites;

namespace Diode_Dominion.DiodeDominion.World.Tiles
{
   public class MapTile
   {
	   #region Fields

      /// <summary>
      /// Holds the previous tile type if the type of tile were to change
      /// </summary>
	   private TileType previousTileType = TileType.NONE;

	   #endregion

		#region Properties

		/// <summary>
		/// Shows location of the map tile
		/// </summary>
		public Rectangle Location { get; set; }

      /// <summary>
      /// Shows if a entity can enter into the tile
      /// </summary>
      public bool CanEnter { get; private set; }

      /// <summary>
      /// Holds the texture of the file
      /// </summary>
      public OverlaySprite TileSprite { get; set; }

      /// <summary>
      /// Holds the enum of what type of tile the map tile is
      /// </summary>
      public TileType TypeOfTile { get; private set; }


		#endregion

		#region Constructors

		/// <summary>
		/// Basic constructor for a map tile
		/// </summary>
		/// <param name="tileSize"></param>
		/// <param name="typeOfTile"></param>
		/// <param name="xCord"></param>
		/// <param name="yCord"></param>
		public MapTile(int tileSize, TileType typeOfTile, int xCord, int yCord)
      {
         Location = new Rectangle(xCord, yCord, tileSize, tileSize);
         TypeOfTile = typeOfTile;
         DetermineCanEnter(typeOfTile);
      }
		#endregion

		#region Methods
		/// <summary>
		/// Determines if an entity can enter a tile.
		/// </summary>
		/// <param name="tile"></param>
		private void DetermineCanEnter(TileType tile)
      {
         if (tile == TileType.BUILDING || tile == TileType.STONE || tile == TileType.WATER)
         {
            CanEnter = false;
         }
         else
            CanEnter = true;
      }

      /// <summary>
      /// Changes the tile type to a new tile and resets the ability for the 
      /// entity to enter based on the tile type passed.
      /// </summary>
      /// <param name="newTileType"></param>
      public void ChangeTileType(TileType newTileType)
      {
         TypeOfTile = newTileType;
         DetermineCanEnter(newTileType);
      }

      /// <summary>
      /// Loads texture content into the map entity
      /// </summary>
      /// <param name="content">Content manager that can import textures</param>
      internal void LoadContent(ContentManager content)
      {
         // Call method to load the correct tile
	      Texture2D tileTexture = GetTileTexture(content);

	      // Create the overlay texture
         Texture2D overlayTexture = new Texture2D(Settings.SpriteBatch.GraphicsDevice, tileTexture.Width, tileTexture.Height);
         Color[] color = new Color[tileTexture.Width * tileTexture.Height];
         Color outlineColor = Color.Black;

         // Set the outline
         for (int i = 0; i < color.Length; i++)
         {
	         if (i % tileTexture.Width == 0) // Left side
	         {
               // Two pixels thick
		         color[i] = outlineColor;
		         color[i + 1] = outlineColor;
	         }
            else if ((i + 1) % tileTexture.Width == 0) // Right side
	         {
		         // Two pixels thick
		         color[i] = outlineColor;
		         color[i - 1] = outlineColor;
	         }
            else if (i < tileTexture.Width) // Top side
	         {
		         color[i] = outlineColor;
		         color[i * tileTexture.Width] = outlineColor;
	         }
            else if (i >= (tileTexture.Width - 1) * tileTexture.Height)
	         {
		         color[i] = outlineColor;
		        color[(tileTexture.Width - 1) * tileTexture.Height] = outlineColor;
	         }
         }
         overlayTexture.SetData(color);

         if (TypeOfTile != TileType.WATER)
            TileSprite = new OverlaySprite(new Vector2(Location.X, Location.Y), tileTexture, overlayTexture);
         else
         {
	         TileSprite = new OverlaySprite(new Vector2(Location.X, Location.Y)
		         , content.Load<Texture2D>("Tiles/Watertiles/Water1")
		         , content.Load<Texture2D>("Tiles/Watertiles/Water2")
		         , content.Load<Texture2D>("Tiles/Watertiles/Water3")
		         , content.Load<Texture2D>("Tiles/Watertiles/Water4")
		         , content.Load<Texture2D>("Tiles/Watertiles/Water5")
		         , content.Load<Texture2D>("Tiles/Watertiles/Water6")
		         , content.Load<Texture2D>("Tiles/Watertiles/Water7")
		         , content.Load<Texture2D>("Tiles/Watertiles/Water8")
		         , content.Load<Texture2D>("Tiles/Watertiles/Water9")
		         , content.Load<Texture2D>("Tiles/Watertiles/Water10")
		         , overlayTexture) {IsAnimated = true};
         }
      }

      /// <summary>
      /// Draws the sprite of the tile
      /// </summary>
      /// <param name="gameTime"></param>
      /// <param name="spriteBatch"></param>
      internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
      {
         // Load the new tile texture if it changes
	      if (previousTileType != TypeOfTile)
	      {
		      previousTileType = TypeOfTile;

            LoadContent(Settings.Content);
	      }

         TileSprite.Draw(gameTime, spriteBatch);
      }
      /// <summary>
      /// Update the overlay status of the tile. If the 
      /// </summary>
      /// <param name="gameTime"></param>
      public void Update(GameTime gameTime)
		{
         TileSprite.DisplayOverlay = MouseSelection.SelectedRegion.Intersects(TileSprite.Bounds);
         TileSprite.Update(gameTime);
      }

      /// <summary>
      /// Loads the proper tile texture for the tile
      /// </summary>
      /// <param name="content"></param>
      private Texture2D GetTileTexture(ContentManager content)
      {
	      Texture2D texture = null;

	      if (TypeOfTile == TileType.DIRT)
	      {
		      texture = content.Load<Texture2D>(TextureLocalization.TileDirt);
	      }
         else if (TypeOfTile == TileType.WATER)
	      {
		      texture = content.Load<Texture2D>(TextureLocalization.TileWater);
	      }
	      else if (TypeOfTile == TileType.STONE)
	      {
		      texture = content.Load<Texture2D>(TextureLocalization.TileStone);
	      }
         else if(TypeOfTile == TileType.FARMLAND)
			{
            texture = content.Load<Texture2D>(TextureLocalization.FarmTile);
			}

	      return texture;
      }
		#endregion
	}
}
