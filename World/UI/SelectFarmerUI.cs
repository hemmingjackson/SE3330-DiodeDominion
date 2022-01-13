using System;
using System.Collections.Generic;
using System.Linq;
using Diode_Dominion.DiodeDominion.Entities;
using Diode_Dominion.DiodeDominion.Entities.Colonists;
using Diode_Dominion.DiodeDominion.Entities.Items.Tools;
using Diode_Dominion.DiodeDominion.Events;
using Diode_Dominion.DiodeDominion.Textures;
using Diode_Dominion.DiodeDominion.World.Tiles;
using Diode_Dominion.DiodeDominion.World.UI.CommandPromptUI;
using Diode_Dominion.Engine.Camera;
using Diode_Dominion.Engine.Containers;
using Diode_Dominion.Engine.Controls.Buttons;
using Diode_Dominion.Engine.Controls.EntityInfoComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Diode_Dominion.DiodeDominion.World.UI
{
	/// <summary>
	/// UI for selecting a farmer
	/// </summary>
	internal class SelectFarmerUi : TaskUi
	{
		/// <summary>
		/// Location of container within UI
		/// </summary>
		private const int ContainerLocation = 100;
		/// <summary>
		/// X location of info component
		/// </summary>
		private const int InfoCompYLocation = 60;
		/// <summary>
		/// Y location of info component
		/// </summary>
		private const int InfoCompXLocation = 5;
		/// <summary>
		/// Relative X location of UI based on screen
		/// </summary>
		private const double XScreenLocation = 2.5;
		/// <summary>
		/// Relative Y location of UI based on screen
		/// </summary>
		private const double YScreenLocation = 7.25;
		/// <summary>
		/// Button spacing for colonist buttons
		/// </summary>
		private const int ButtonSpacing = 25;
		/// <summary>
		/// Map tiles that will be tilled/farmed
		/// </summary>
		private readonly IEnumerable<MapTile> mapTiles;
		/// <summary>
		/// Reference to the game world
		/// </summary>
		private readonly GameWorld gameWorld;
		/// <summary>
		/// Constructor for the UI
		/// </summary>
		/// <param name="gameWorld"></param>
		/// <param name="entities"></param>
		/// <param name="tiles"></param>
		internal SelectFarmerUi(GameWorld gameWorld, IEnumerable<Entity> entities, IEnumerable<MapTile> tiles)
		{
			//Find any entities that have a hoe
			Entities = entities.Where(entity => entity is Colonist);
			Entities = Entities.ToList();
			Entities = Entities.Where(entity => entity.Holdables().Exists(item => (item.IsTool && ((Tool)item).ToolType == ToolType.HOE)));
			mapTiles = tiles;
			this.gameWorld = gameWorld;
		}
		/// <summary>
		/// Handles closing the UI
		/// </summary>
		public override void Close()
		{
			UserInterfaceManager.Instance.RemoveUserInterface(this);
		}
		/// <summary>
		/// Draws the UI on the screen
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>

		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			Container.Draw(gameTime, spriteBatch);
		}
		/// <summary>
		/// Handles opening the UI
		/// </summary>

		public override void Open()
		{
			CreateContainer();
			// Add all of the UI components
			CreatePossibleFarmers();

			// Update the size of the Container if anything is going out of bounds
			Resize();

			// Created last so it is in the correct location if the container is resized 
			CreateExitButton();

			UserInterfaceManager.Instance.AddUserInterface(this);
		}
		/// <summary>
		/// Handles creating a list of farmers to choose from
		/// </summary>

		private void CreatePossibleFarmers()
		{
			InfoComponent harvestEntityInfo = new InfoComponent("Select Farmer:", new Vector2(InfoCompXLocation, InfoCompYLocation))
			{
				PenColor = Color.White
			};

			Container.AddComponent(harvestEntityInfo);

			int btnLocation = 85;

			// Check if there are any entities that can harvest the static entity
			if (!Entities.Any())
			{
				InfoComponent noEntityAvailable =
					new InfoComponent("No Colonist with a hoe", new Vector2(ComponentEdgeFloat, btnLocation))
					{
						PenColor = Color.Maroon
					};

				Container.AddComponent(noEntityAvailable);
			}
			else
			{
				// Create the buttons for each entity that can harvest it
				foreach (Entity harvestEntity in Entities)
				{
					Button button = new Button(Settings.Content.Load<Texture2D>(TextureLocalization.CommandButton),
						new Vector2(10, btnLocation), harvestEntity.Name)
					{
						PenColor = Color.White,
						HoverTextColor = Color.Red
					};

					btnLocation += ButtonSpacing;
					button.Click += Create_Farmland_Click;
					Container.AddComponent(button);
				}
			}
		}
		/// <summary>
		/// Handles when a entity is chosen to tile the ground
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Create_Farmland_Click(object sender, EventArgs e)
		{
			foreach (Entity entity in Entities)
			{
				// Check for the proper name
				if (((Button)sender).Text == entity.Name)
				{
					foreach(MapTile tile in mapTiles)
					{
						if(tile.TypeOfTile == TileType.DIRT)
							entity.BaseAi.Update(new TileTillEvent(tile, entity));
						// Calls plant crop event if selected tiles are farmland
						if (tile.TypeOfTile == TileType.FARMLAND)
						{
							entity.BaseAi.Update(new PlantCropEvent(gameWorld, tile, entity));
						}
					}	
					break;
				}
			}
			Close();
		}
		/// <summary>
		/// Updates the UI if the user clicks off the UI or the camera moves
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
		{
			Container.Sprite.Origin = 
				new Vector2((int)(Camera.Location.X - Settings.GameDimensions.X / XScreenLocation), (int)(Camera.Location.Y - Settings.GameDimensions.Y / YScreenLocation));
			Container.Update(gameTime);
			if (IdleClick())
			{
				Close();
			}
		}
		/// <summary>
		/// Creates container to hold all information/buttons
		/// </summary>
		private void CreateContainer()
		{
			// Set the texture for the Container
			Color[] color = new Color[Width * Height];
			for (int i = 0; i < color.Length; i++)
			{
				color[i] = Color.Black;
			}

			// Create the container for the UI
			Texture2D containerTexture = new Texture2D(Settings.SpriteBatch.GraphicsDevice, Width, Height);
			containerTexture.SetData(color);
			Container = new EntityContainer(containerTexture, ContainerLocation, ContainerLocation);
		}
	}
}
