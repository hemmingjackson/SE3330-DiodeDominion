using System;
using System.Collections.Generic;
using System.Linq;
using Diode_Dominion.DiodeDominion.Entities;
using Diode_Dominion.DiodeDominion.Entities.Colonists;
using Diode_Dominion.DiodeDominion.Entities.StaticEntities;
using Diode_Dominion.DiodeDominion.Events;
using Diode_Dominion.DiodeDominion.Textures;
using Diode_Dominion.Engine.Containers;
using Diode_Dominion.Engine.Controls.Buttons;
using Diode_Dominion.Engine.Controls.EntityInfoComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Diode_Dominion.DiodeDominion.World.UI
{
	/// <summary>
	/// Handles creating a UI for a static entity when the static entity is clicked
	/// </summary>
	internal class StaticEntityUi : EntityUi
	{
		#region Fields

		/// <summary>
		/// Information about the health of the static entity
		/// </summary>
		private InfoComponent healthInfo;
		private double oldHealth;
		private double oldMaxHealth;
		private readonly StaticEntity currentStaticEntity;
		private InfoComponent nameInfo;

		#endregion

		#region Constructors

		/// <summary>
		/// Create the UI with the proper static entity
		/// </summary>
		/// <param name="entities">Entities in the game world</param>
		/// <param name="staticEntity">Static entity associated with the UI</param>
		internal StaticEntityUi(IEnumerable<Entity> entities, StaticEntity staticEntity)
		{
			Entity = staticEntity;
			currentStaticEntity = staticEntity;

			// Find all the COLONISTS that can interact with the static entity
			Entities = entities.Where(entity => entity is Colonist && staticEntity.CanInteract(entity)).ToList();
		}
		
		#endregion
		
		#region Methods
		
		/// <summary>
		/// Handle opening the user interface
		/// </summary>
		public override void Open()
		{
			CreateContainer();

			// Add all of the UI components
			if (currentStaticEntity.IsHarvestable == false)
			{
				CreateNameInfo();
				CreateHealthInfo();
			}
			else
			{
				CreateToolRequired();
				CreateHealthInfo();
				CreateEntityHarvestButtons();
			}

			// Update the size of the Container if anything is going out of bounds
			Resize();

			// Created last so it is in the correct location if the container is resized 
			CreateExitButton();

			UserInterfaceManager.Instance.AddUserInterface(this);
		}

		/// <summary>
		/// Removes the user interface
		/// </summary>
		public override void Close()
		{
			UserInterfaceManager.Instance.RemoveUserInterface(this);
		}

		/// <summary>
		/// Handle updating the UI
		/// </summary>
		/// 
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
		{
			Container.Update(gameTime);

			// Remove the UI when the static entity is finished being harvested
			if (((StaticEntity)Entity).OnGround)
			{
				Close();
			}

			if (IdleClick())
			{
				Close();
			}

			// Only update when there is a change in the health values as string concatenation is inefficient 
			if (Math.Abs(oldHealth - Entity.Health) > 0 || Math.Abs(oldMaxHealth - Entity.MaxHealth) > 0)
			{
				healthInfo.DisplayText = "Health: " + Math.Floor(Entity.Health) + "/" + Entity.MaxHealth;

				oldHealth = Entity.Health;
				oldMaxHealth = Entity.MaxHealth;
			}
		}

		/// <summary>
		/// Handle drawing the UI
		/// </summary>
		/// 
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			Container.Draw(gameTime, spriteBatch);
		}

		#region UI Components

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
		/// Create the health info for the entity
		/// </summary>
		private void CreateHealthInfo()
		{
				healthInfo = new InfoComponent("Health: " + Entity.Health + "/" + Entity.MaxHealth, new Vector2(5, 30))
				{
					PenColor = Color.White
				};

			Container.AddComponent(healthInfo);
		}

		/// <summary>
		/// Create the info for the tool required
		/// </summary>
		private void CreateToolRequired()
		{
			InfoComponent toolRequired = new InfoComponent("Tool: " + ((StaticEntity) Entity).ToolTypeRequired,
				new Vector2(5, 5))
			{
				PenColor = Color.White
			};

			Container.AddComponent(toolRequired);
		}

		private void CreateEntityHarvestButtons()
		{
			InfoComponent harvestEntityInfo = new InfoComponent("Send to Harvest:", new Vector2(5, 60))
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
			else
			{
				// Create the buttons for each entity that can harvest it
				foreach (Entity harvestEntity in Entities)
				{
					Button button = new Button(Settings.Content.Load<Texture2D>(TextureLocalization.LongCommandButton),
						new Vector2(5, btnLocation), harvestEntity.Name)
					{
						PenColor = Color.White,
						HoverTextColor = Color.Red
					};

					btnLocation += 25;

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

		/// <summary>
		/// Create the name info for the entity
		/// </summary>
		private void CreateNameInfo()
		{
			nameInfo = new InfoComponent("Name: " + Entity.Name, new Vector2(5, 5))
			{
				PenColor = Color.White
			};

			Container.AddComponent(nameInfo);
		}

		#endregion

		#endregion
	}
}