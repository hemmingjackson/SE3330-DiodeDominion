using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Diode_Dominion.DiodeDominion.Entities.Colonists;
using Diode_Dominion.DiodeDominion.Entities.Items;
using Diode_Dominion.DiodeDominion.Entities.StaticEntities.Crops;
using Diode_Dominion.DiodeDominion.Entities.StaticEntities.Stockpiles;
using Diode_Dominion.DiodeDominion.Textures;
using Diode_Dominion.DiodeDominion.World.Tiles;
using Diode_Dominion.Engine.Containers;
using Diode_Dominion.Engine.Controls.Buttons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Diode_Dominion.DiodeDominion.World.UI
{
	/// <summary>
	/// Creates the interaction menu that holds all the options
	/// to interact and modify tiles
	/// </summary>
	internal class TileInteractions
	{
		#region Fields
		private readonly GameWorld gameWorld;
		/// <summary>
		/// Width of the menu
		/// </summary>
		private const int Width = 125;
		/// <summary>
		/// Holds the container
		/// </summary>
		private readonly Container menu;

		/// <summary>
		/// Color that the menu will be when it is loaded
		/// </summary>
		private readonly Color containerColor = Color.LightBlue;

		#endregion
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="gameWorld"></param>
		public TileInteractions(GameWorld gameWorld)
		{
			this.gameWorld = gameWorld;
			int height = (int)Settings.GameDimensions.Y;

			// Create the colors for the container texture
			Color[] colors = new Color[Width * height];
			for (int i = 0; i < colors.Length; i++)
			{
				colors[i] = containerColor;
			}

			// Set the color data to the texture
			Texture2D containerTexture = new Texture2D(Settings.SpriteBatch.GraphicsDevice, Width, height);
			containerTexture.SetData(colors);

			menu = new Container(containerTexture, 0, 0);
		}

		#region Methods

		/// <summary>
		/// Load the content for the UI
		/// </summary>
		/// 
		/// <param name="content"></param>
		public void LoadContent(ContentManager content)
		{
			// Create the buttons
			Button tileEditorButton = new Button(content.Load<Texture2D>(TextureLocalization.TileEditorButton), 
				new Vector2(0, 0),
				"");
			Button createStockpileButton = new Button(content.Load<Texture2D>(TextureLocalization.CreateStockpileButton),
				new Vector2(),
				"");
			Button createFarmlandButton = new Button(content.Load<Texture2D>(TextureLocalization.CreateFarmlandButton),
			new Vector2(),
			"");
			Button createColonistButton = new Button(content.Load<Texture2D>(TextureLocalization.CreateColonistButton),
				new Vector2(),
				"");
			Button plantCropButton = new Button(content.Load<Texture2D>(TextureLocalization.PlantCropButton),
				new Vector2(),
				"");
			Button deleteStockpileButton = new Button(content.Load<Texture2D>(TextureLocalization.DeleteStockpileButton),
				new Vector2(),
				"");

			// Make certain the buttons do not transform with the view
			tileEditorButton.Sprite.ShouldTransform = false;
			createStockpileButton.Sprite.ShouldTransform = false;
			createFarmlandButton.Sprite.ShouldTransform = false;
			createColonistButton.Sprite.ShouldTransform = false;
			plantCropButton.Sprite.ShouldTransform = false;
			deleteStockpileButton.Sprite.ShouldTransform = false;

			// Add on-click event to the buttons
			tileEditorButton.Click += TileEditorButtonOnClick;
			createStockpileButton.Click += CreateStockpileButtonOnClick;
			createFarmlandButton.Click += CreateFarmlandButtonOnClick;
			createColonistButton.Click += CreateColonistButtonOnClick;
			plantCropButton.Click += PlantCropButtonOnClick;
			deleteStockpileButton.Click += DeleteStockpileButtonOnClick;

			// Add buttons to the container
			menu.AddComponent(tileEditorButton);
			menu.AddComponent(createStockpileButton);
			menu.AddComponent(deleteStockpileButton);
			menu.AddComponent(createFarmlandButton);
			menu.AddComponent(plantCropButton);
			menu.AddComponent(createColonistButton);
			


			// Do not transform the bounds of the components
			Parallel.For(0, menu.ComponentManager.Count, i => menu.ComponentManager[i].Sprite.ShouldTransform = false);
		}

		private void DeleteStockpileButtonOnClick(object sender, EventArgs e)
		{
			int index = 0;
			while (index < gameWorld.Stockpiles.Count)
			{
				if (gameWorld.Stockpiles[index].IsTouchingStockpile(gameWorld.SelectedTiles))
				{
					IEnumerable<Item> itemsToSpawn = gameWorld.Stockpiles[index].GetItems();
					gameWorld.Stockpiles.RemoveAt(index);
					foreach(Item item in itemsToSpawn)
					{
						gameWorld.Entities.Add(item);
					}	
				}
				else
					index++;
			}
		}

		/// <summary>
		/// Handles what should occur when the create plant crops button is clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PlantCropButtonOnClick(object sender, EventArgs e)
		{
			IUserInterface userInterface = new SelectFarmerUi(gameWorld, gameWorld.Entities, gameWorld.SelectedTiles);
			userInterface.Open();
		}

		public void GrowCorn()
		{
			int index = 0;
			while (index < gameWorld.Stockpiles.Count)
			{
				if (gameWorld.Stockpiles[index].IsTouchingStockpile(gameWorld.SelectedTiles))
					gameWorld.Stockpiles.RemoveAt(index);
				else
					index++;
			}
		}

		public void GrowStrawberry()
		{
			foreach (Crop crop in from tile in 
				gameWorld.SelectedTiles where tile.TypeOfTile == TileType.FARMLAND 
				select new Crop(CropType.STRAWBERRY)
			{
				EntitySprite = {Origin = new Vector2(tile.Location.X, tile.Location.Y)}
			})
			{
				gameWorld.Entities.Add(crop);
			}
		}

		/// <summary>
		/// Handles what should occur when the create farmland button is clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CreateFarmlandButtonOnClick(object sender, EventArgs e)
		{
			IUserInterface userInterface = new SelectFarmerUi(gameWorld, gameWorld.Entities, gameWorld.SelectedTiles);
			userInterface.Open();
		}
		/// <summary>
		/// Handles what should occur when the create colonist button is clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CreateColonistButtonOnClick(object sender, EventArgs e)
		{
			Colonist colonist = gameWorld.ColonistGen.GenerateColonist();
			colonist.LoadContent(Settings.Content);
			colonist.BaseAi.SetMapTiles(gameWorld.TileMap.Tiles);
			gameWorld.Entities.Add(colonist);
		}

		/// <summary>
		/// Handle what should occur when the create stockpile button is clicked
		/// </summary>
		/// 
		/// <param name="sender">Object sent along with the button click</param>
		/// <param name="e"></param>
		private void CreateStockpileButtonOnClick(object sender, EventArgs e)
		{
			//Check to see if any tiles are selected
			if (gameWorld.SelectedTiles.Count > 0)
			{
				bool isTouching = false;
				int stockpileIndex = gameWorld.Stockpiles.Count;
				//Iterate through list, stop if any stockpiles are touching any tiles in the selected tiles
				for(int i = 0; i < gameWorld.Stockpiles.Count && !isTouching;  i++)
				{
					if (gameWorld.Stockpiles[i].IsTouchingStockpile(gameWorld.SelectedTiles))
					{
						isTouching = true;
						stockpileIndex = i;
					}		
				}
				if (!isTouching)
					gameWorld.Stockpiles.Add(new ConglomerateStockpile(gameWorld.SelectedTiles));
				else
					gameWorld.Stockpiles[stockpileIndex].AddTiles(gameWorld.SelectedTiles);
			}
		}

		/// <summary>
		/// Handle what should occur when the tile editor button is clicked
		/// </summary>
		/// 
		/// <param name="sender">Object sent along with the button click</param>
		/// <param name="e"></param>
		private void TileEditorButtonOnClick(object sender, EventArgs e)
		{
			//throw new NotImplementedException();
		}

		/// <summary>
		/// Draw all of the components
		/// </summary>
		/// 
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			menu.Draw(gameTime, spriteBatch, Color.White * Settings.MenuTransparency);
		}

		/// <summary>
		/// Update all of the components
		/// </summary>
		/// 
		/// <param name="gameTime"></param>
		public void Update(GameTime gameTime)
		{
			menu.Update(gameTime);
		}

		#endregion
	}
}
