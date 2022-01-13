using Diode_Dominion.DiodeDominion.Entities.Colonists;
using Diode_Dominion.Engine.Containers;
using Diode_Dominion.Engine.Controls.EntityInfoComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Diode_Dominion.DiodeDominion.World.UI
{
	/// <summary>
	/// Class to display colonist information
	/// </summary>
	internal class ColonistInfoUi : EntityUi
	{
		#region Fields

		/// <summary>
		/// Information about the health, inventory, and mood of the colonist
		/// </summary>
		private InfoComponent healthInfo;
		private InfoComponent nameInfo;
		private InfoComponent inventory;
		private InfoComponent hungerInfo;
		private InfoComponent moodInfo;
		private double oldHealth;
		private double oldMaxHealth;
		private double oldHunger;
		private double oldMaxHunger;
		private Vector2 entityOldLoc;

		/// <summary>
		/// Entity the UI is based around
		/// </summary>
		internal Colonist Colonist { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Create the UI with the proper colonist
		/// </summary>
		internal ColonistInfoUi(Colonist colonist)
		{
			Entity = colonist;
			Colonist = colonist;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Handling opening the UI and adding of UI components
		/// </summary>
		public override void Open()
		{
			CreateContainer();

			// Add all of the UI components
			CreateHealthInfo();
			CreateNameInfo();
			//CreateMoodInfo();
			ShowInventory();

			// Update the size of the Container if anything is going out of bounds
			Resize();

			// Created last so it is in the correct location if the container is resized 
			CreateExitButton();

			UserInterfaceManager.Instance.AddUserInterface(this);
		}

		/// <summary>
		/// Handles Drawing the UI
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			Container.Draw(gameTime, spriteBatch);
		}

		/// <summary>
		/// Handles Updating the UI
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
		{
			Container.Update(gameTime);

			if(Colonist.Alive == false)
            {
				Close();
				new ColonistDeadUi(Colonist).Open();
			}

			// Checks if colonist moves, follows Ui
			if (Entity.EntitySprite.Origin != entityOldLoc)
			{
				Container.Sprite.Origin = new Vector2(Entity.EntitySprite.Origin.X + 50,
					 Entity.EntitySprite.Origin.Y);
				entityOldLoc = Entity.EntitySprite.Origin;
			}

			// Checks if clicked outside Ui
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

			if (Math.Abs(oldHunger - Colonist.TimeTillRecharge) > 0 || Math.Abs(oldMaxHunger - Colonist.MaxBatteryLife) > 0)
			{
				hungerInfo.DisplayText = "Hunger: " + Math.Floor(Colonist.TimeTillRecharge) + "/" + Colonist.MaxBatteryLife;

				oldHunger = Colonist.TimeTillRecharge;
				oldMaxHunger = Colonist.MaxBatteryLife;
			}
			moodInfo.DisplayText = "Mood: " + Colonist.Mood;
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
		/// Displays the name info for the colonist
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
		/// Displays the health info for the colonist
		/// </summary>
		private void CreateHealthInfo()
		{
			healthInfo = new InfoComponent("Health: " + Entity.Health + "/" + Entity.MaxHealth, new Vector2(5, 30))
			{
				PenColor = Color.White
			};

			Container.AddComponent(healthInfo);

			hungerInfo = new InfoComponent("Hunger: " + Colonist.TimeTillRecharge + "/" + Colonist.MaxBatteryLife, new Vector2(5, 55))
			{
				PenColor = Color.White
			};

			Container.AddComponent(hungerInfo);

			moodInfo = new InfoComponent("Mood: " + Colonist.Mood, new Vector2(5, 80))
			{
				PenColor = Color.White
			};

			Container.AddComponent(moodInfo);

		}

		/// <summary>
		/// Lists items currently in the colonist's inventory
		/// </summary>
		private void ShowInventory()
		{
			List<Entities.Items.Item> inventoryList = Entity.Holdables();

			inventory = new InfoComponent("Inventory: " + String.Join(",",inventoryList.Select(r => r.Name)), new Vector2(5, 105))
			{
				PenColor = Color.White
			};

			Container.AddComponent(inventory);
		}

		#endregion
	}
}