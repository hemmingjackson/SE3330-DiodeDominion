using Diode_Dominion.DiodeDominion.Entities;
using Diode_Dominion.DiodeDominion.Entities.Animals;
using Diode_Dominion.DiodeDominion.Textures;
using Diode_Dominion.Engine.Containers;
using Diode_Dominion.Engine.Controls.Buttons;
using Diode_Dominion.Engine.Controls.EntityInfoComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Diode_Dominion.DiodeDominion.World.UI.CommandPromptUI;

namespace Diode_Dominion.DiodeDominion.World.UI
{
	/// <summary>
	/// Class to display animal information
	/// </summary>
	class AnimalInfoUi : EntityUi
	{
		#region Fields

		/// <summary>
		/// Information about the animal
		/// </summary>
		private InfoComponent healthInfo;
		private InfoComponent nameInfo;
		private InfoComponent killedInfo;
		private double oldHealth;
		private double oldMaxHealth;
		private Vector2 entityOldLoc;

		#endregion

		#region Constructors

		/// <summary>
		/// Create the UI with the proper animal
		/// </summary>
		/// <param name="entities"></param>
		/// <param name="animal"></param>
		internal AnimalInfoUi(IEnumerable<Entity> entities, Animal animal)
		{
			Entity = animal;
			Entities = entities;
		}

		#endregion

		#region Methods
		/// <summary>
		/// Opens the UI and adds all components
		/// </summary>
		public override void Open()
		{
			CreateContainer();

			// Add all of the UI components
			CreateHealthInfo();
			CreateNameInfo();
			if (Entity.Health == 0)
			{
				CreateKilledInfo();
			}
			else
			{
				OpenCommandPromptButton();
			}

			// Update the size of the Container if anything is going out of bounds
			Resize();

			// Created last so it is in the correct location if the container is resized 
			CreateExitButton();

			UserInterfaceManager.Instance.AddUserInterface(this);
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
		/// Updates the UI
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
		{
			if (Entity.EntitySprite.Origin != entityOldLoc)
			{
				Container.Sprite.Origin = new Vector2(Entity.EntitySprite.Origin.X + 50,
					 Entity.EntitySprite.Origin.Y);
				entityOldLoc = Entity.EntitySprite.Origin;
			}

			Container.Update(gameTime);
			
			// Only update when there is a change in the health values as string concatenation is inefficient 
			if (Math.Abs(oldHealth - Entity.Health) > 0 || Math.Abs(oldMaxHealth - Entity.MaxHealth) > 0)
			{
				healthInfo.DisplayText = "Health: " + Math.Floor(Entity.Health) + "/" + Entity.MaxHealth;

				oldHealth = Entity.Health;
				oldMaxHealth = Entity.MaxHealth;
			}
			if (IdleClick())
			{
				Close();
			}
		}

		/// <summary>
		/// Removes the UI
		/// </summary>
		public override void Close()
		{
			UserInterfaceManager.Instance.RemoveUserInterface(this);
		}

		#endregion

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
		/// Create the name info for the animal
		/// </summary>
		private void CreateNameInfo()
		{
			nameInfo = new InfoComponent("Name: " + Entity.Name, new Vector2(5, 5))
			{
				PenColor = Color.White
			};

			Container.AddComponent(nameInfo);
		}

		/// <summary>
		/// Create the health info for the animal
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
		/// Create dead info for dead animal
		/// </summary>
		private void CreateKilledInfo()
        {
			killedInfo = new InfoComponent("Killed", new Vector2(5, 55))
			{
				PenColor = Color.Red
			};

			Container.AddComponent(killedInfo);
		}

		/// <summary>
		/// Opens CommandPrompt to select colonist to perform action
		/// </summary>
		private void OpenCommandPromptButton()
		{
			int btnLocation = 60;

			Button button = new Button(Settings.Content.Load<Texture2D>(TextureLocalization.CommandButton),
						new Vector2(0, btnLocation), "Attack")
			{
				PenColor = Color.Red,
				HoverTextColor = Color.DarkRed
			};

			Container.AddComponent(button);

			// Check which button is clicked
			button.Click += Button_Click;
		}

		/// <summary>
		/// Creates new command prompt UI based on the animal selected
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Button_Click(object sender, EventArgs e)
		{
			// Closes Info UI
			Close();
			if (Entity is Animal animal)
			{
				new CommandPromptDialogBox(Entities.ToList(), animal).Open();
			}
		}
		#endregion
	}
}
