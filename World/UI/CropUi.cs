using Diode_Dominion.DiodeDominion.Entities;
using Diode_Dominion.DiodeDominion.Entities.Colonists;
using Diode_Dominion.DiodeDominion.Entities.StaticEntities;
using Diode_Dominion.DiodeDominion.Entities.StaticEntities.Crops;
using Diode_Dominion.DiodeDominion.Events;
using Diode_Dominion.DiodeDominion.Textures;
using Diode_Dominion.Engine.Containers;
using Diode_Dominion.Engine.Controls.Buttons;
using Diode_Dominion.Engine.Controls.EntityInfoComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Diode_Dominion.DiodeDominion.World.UI
{
	/// <summary>
	/// Class to display crop information
	/// </summary>
	class CropUi : EntityUi
	{
		#region Fields

		// Info about crops
		private InfoComponent cropInfo;
		private double oldGrowth;
		internal Crop CropInfo { get; set; }
		#endregion

		#region Constructors

		/// <summary>
		/// Create the UI with the proper static entity
		/// </summary>
		/// <param name="entities">Entities in the game world</param>
		/// <param name="crop"></param>
		internal CropUi(IEnumerable<Entity> entities, Crop crop)
		{
			Entity = crop;
			CropInfo = crop;

			// Find all the COLONISTS that can interact with the static entity
			Entities = entities.Where(entity => entity is Colonist && crop.CanInteract(entity)).ToList();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Handles opening UI
		/// </summary>
		public override void Open()
		{
			CreateContainer();

			// Add all of the UI components
			//CreateToolRequired();
			CreateNameInfo();
			CreateGrowthInfo();
			CreateEntityHarvestCropButton();

			// Update the size of the Container if anything is going out of bounds
			Resize();

			// Created last so it is in the correct location if the container is resized 
			CreateExitButton();

			UserInterfaceManager.Instance.AddUserInterface(this);
		}

		/// <summary>
		/// Updates UI
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
		{
			Container.Update(gameTime);

			if (((StaticEntity)Entity).OnGround)
			{
				Close();
			}
			if (Math.Abs(oldGrowth - Entity.Health) > 0)
			{
				cropInfo.DisplayText = "Growth: " + Math.Floor(Entity.Health) + "%";

				oldGrowth = Entity.Health;
			}
			if (IdleClick())
			{
				Close();
			}
		}

		/// <summary>
		/// Draws the UI
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			Container.Draw(gameTime, spriteBatch);
		}

		/// <summary>
		/// Closes UI
		/// </summary>
		public override void Close()
		{
			UserInterfaceManager.Instance.RemoveUserInterface(this);
		}

		#endregion

		#region Ui Components

		/// <summary>
		/// Create the container that holds the UI components
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
			Container = new EntityContainer(containerTexture, (int)Entity.EntitySprite.Origin.X + 50, (int)Entity.EntitySprite.Origin.Y);
		}

		/// <summary>
		/// Create the health info for the crop
		/// </summary>
		private void CreateGrowthInfo()
		{
			cropInfo = new InfoComponent("Growth: " + Entity.Health + "%", new Vector2(5, 30))
			{
				PenColor = Color.White
			};

			Container.AddComponent(cropInfo);
		}

		/// <summary>
		/// Create the name info for the crop
		/// </summary>
		public void CreateNameInfo()
		{
			cropInfo = new InfoComponent("Name: " + Entity.Name, new Vector2(5, 5))
			{
				PenColor = Color.White
			};

			Container.AddComponent(cropInfo);
		}

		/// <summary>
		/// Button for harvesting the crop
		/// </summary>
		private void CreateEntityHarvestCropButton()
		{
			InfoComponent harvestEntityInfo = new InfoComponent("Harvest Crop:", new Vector2(5, 60))
			{
				PenColor = Color.White
			};

			Container.AddComponent(harvestEntityInfo);

			int btnLocation = 85;

			// Check if there are any entities that can harvest the static entity

			if (!Entities.Any())
			{
				InfoComponent noEntityAvailable =
					new InfoComponent("    No Colonist with " + ((StaticEntity)Entity).ToolTypeRequired.ToString().ToLower(), new Vector2(ComponentEdgeFloat, btnLocation))
					{
						PenColor = Color.Maroon
					};

				Container.AddComponent(noEntityAvailable);
			}
			if(Entity.Health < 100)
			{
				InfoComponent notHarvestable =
					new InfoComponent("    Crop is not fully grown ", new Vector2(ComponentEdgeFloat, btnLocation))
					{
						PenColor = Color.Maroon
					};
				Container.AddComponent(notHarvestable);
			}
			else
			{
				// Create the buttons for each entity that can harvest it
				foreach (Entity harvestEntity in Entities)
				{
					Button button = new Button(Settings.Content.Load<Texture2D>(TextureLocalization.CommandButton),
						new Vector2(5, btnLocation), harvestEntity.Name)
					{
						PenColor = Color.White,
						HoverTextColor = Color.Red
					};

					btnLocation += 30;

					Container.AddComponent(button);

					// Check which button is clicked
					button.Click += (sender, args) =>
					{
						// Check for the entity that was clicked
						foreach (Entity entity in Entities)
						{
							// Check for the proper name
							if (((Button)sender).Text == entity.Name)
							{
								// Send off the event
								entity.BaseAi.HarvestStaticEntityEvent(new HarvestStaticEntityEvent(entity.Id, (StaticEntity)Entity));

								break;
							}
						}
					};
				}
			}
		}

		#endregion
	}
}
