using Diode_Dominion.DiodeDominion.Entities.Items;
using Diode_Dominion.Engine.Containers;
using Diode_Dominion.Engine.Controls.EntityInfoComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Diode_Dominion.DiodeDominion.World.UI
{
	/// <summary>
	/// Class to display meat information
	/// </summary>
	internal class MeatUi : EntityUi
	{
		#region Fields

		/// <summary>
		/// Information about the health, inventory, and mood of the colonist
		/// </summary>
		private InfoComponent meatType;
		private InfoComponent healthValue;
		public Meat Meat;
		private Vector2 entityOldLoc;
		#endregion

		#region Constructors

		/// <summary>
		/// Create the UI with the proper colonist
		/// </summary>
		internal MeatUi(Meat meat)
		{
			Entity = meat;
			Meat = meat;
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
			CreateMeatType();
			CreateHealthValue();

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
		private void CreateMeatType()
		{
			meatType = new InfoComponent("Raw Meat: " + Meat.FoodType, new Vector2(5, 5))
			{
				PenColor = Color.White
			};

			Container.AddComponent(meatType);
		}

		/// <summary>
		/// Displays the health info for the colonist
		/// </summary>
		private void CreateHealthValue()
		{

			healthValue = new InfoComponent("Health Value: " + Meat.HealthValue, new Vector2(5, 30))
			{
				PenColor = Color.White
			};

			Container.AddComponent(healthValue);

		}
		#endregion
	}
}