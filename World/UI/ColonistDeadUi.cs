using Diode_Dominion.Engine.Containers;
using Diode_Dominion.Engine.Controls.EntityInfoComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Diode_Dominion.DiodeDominion.Entities;

namespace Diode_Dominion.DiodeDominion.World.UI
{
	internal class ColonistDeadUi : EntityUi
	{

		#region Fields

		/// <summary>
		/// Entity the UI is based around
		/// </summary>
		internal Entity Colonist { get; set; }

		private InfoComponent nameInfo;

		private ItemComponent ItemComponent { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Create the UI with the proper colonist
		/// </summary>
		internal ColonistDeadUi(Entity colonist)
		{
			Entity = colonist;
			Colonist = colonist;

		}

		#endregion

		#region Methods

		/// <summary>
		/// Handling opening the UI
		/// </summary>
		public override void Open()
		{
			CreateContainer();
			ItemComponent = new ItemComponent(new Vector2(25, 5));
			Container.AddComponent(ItemComponent);

			// Add all of the UI components
			CreateNameInfo();

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
			ItemComponent.Draw(gameTime, spriteBatch);

		}

		/// <summary>
		/// Handles Updating the UI
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
		{
			Container.Update(gameTime);
			ItemComponent.Update(gameTime);

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
				color[i] = Color.Blue;
			}

			// Create the container for the UI
			Texture2D containerTexture = new Texture2D(Settings.SpriteBatch.GraphicsDevice, Width, Height);
			containerTexture.SetData(color);
			Container = new EntityContainer(containerTexture, (int)Entity.EntitySprite.Origin.X + 50, (int)Entity.EntitySprite.Origin.Y);
		}

		/// <summary>
		/// Create the name info for the entity
		/// </summary>
		private void CreateNameInfo()
		{
			nameInfo = new InfoComponent("Fatal Error.", new Vector2(5, 75))
			{
				PenColor = Color.DarkRed
			};

			Container.AddComponent(nameInfo);
		}
		#endregion
	}
}