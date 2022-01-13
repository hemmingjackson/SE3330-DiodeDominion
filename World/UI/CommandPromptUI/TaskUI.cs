using System.Collections.Generic;
using Diode_Dominion.DiodeDominion.Entities;
using Diode_Dominion.DiodeDominion.Textures;
using Diode_Dominion.Engine.Containers;
using Diode_Dominion.Engine.Controls;
using Diode_Dominion.Engine.Controls.Buttons;
using Diode_Dominion.Engine.Controls.EntityInfoComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Diode_Dominion.DiodeDominion.World.UI.CommandPromptUI
{
	internal abstract class TaskUi: IUserInterface
	{
		/// <summary>
		/// How far from the edge and each component the container will attempt to
		/// be for each component
		/// </summary>
		internal int ComponentEdgeFloat { get; set; } = 5;

		internal IEnumerable<Entity> Entities { get; set; }

		/// <summary>
		/// The width of the container for the UI
		/// </summary>
		protected int Width { get; set; } = 1;

		/// <summary>
		/// The height of the container for the UI
		/// </summary>
		protected int Height { get; set; } = 1;

		/// <summary>
		/// Entity the UI is based around
		/// </summary>
		internal Entity Entity { get; set; }

		/// <summary>
		/// The container that makes up everything within the UI
		/// </summary>
		internal EntityContainer Container { get; set; }

		private static MouseState _currentMouse;

		public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

		public abstract void Update(GameTime gameTime);

		public abstract void Open();

		public abstract void Close();

		/// <summary>
		/// Resize the container that the UIs use to draw all of the components
		/// </summary>
		internal void Resize()
		{
			// Holds largest x/y position of the components
			float componentLocationY = CalculateHighestComponentY();
			float componentLocationX = CalculateHighestComponentX() + 75;

			// Check if any of the components are going off screen
			if (Container.Sprite.Origin.Y + Container.Sprite.Height < componentLocationY ||
				 Container.Sprite.Origin.X + Container.Sprite.Width < componentLocationX)
			{
				// Calculate the new width/height of the container
				int newHeight = Height = (int)System.Math.Ceiling(componentLocationY - Container.Sprite.Origin.Y) + ComponentEdgeFloat;
				int newWidth = Width = (int)System.Math.Ceiling(componentLocationX - Container.Sprite.Origin.X) + ComponentEdgeFloat;

				// Set the texture for the container
				Color[] color = new Color[newWidth * newHeight];
				for (int i = 0; i < color.Length; i++)
				{
					color[i] = Color.Black;
				}

				// Apply the new texture size
				Texture2D containerTexture = new Texture2D(Settings.SpriteBatch.GraphicsDevice, newWidth, newHeight);
				containerTexture.SetData(color);

				// Set the new container texture
				Container.Sprite.Texture = containerTexture;
			}
		}

		/// <summary>
		/// Allows for the creation of the exit button for the UI
		/// </summary>
		internal void CreateExitButton()
		{
			// Button Textures
			Texture2D exitButtonTexture = Settings.Content.Load<Texture2D>(TextureLocalization.ExitButton);

			Button btnExit = new Button(exitButtonTexture,
				new Vector2(Container.Sprite.Width - exitButtonTexture.Width - ComponentEdgeFloat, ComponentEdgeFloat), "X");

			// Close the UI on click
			btnExit.Click += (sender, args) => Close();

			Container.AddComponent(btnExit);
		}

		/// <summary>
		/// Calculate the highest X coordinate from the components within the container
		/// </summary>
		/// 
		/// <returns>Highest X coordinate</returns>
		private float CalculateHighestComponentX()
		{
			float componentLocationX = 0;

			// Find the component that is furthest down/right of the screen
			foreach (Component component in Container.ComponentManager)
			{
				// Info component does not have a good texture to grab the size from so checking another way
				if (component is InfoComponent infoComponent)
				{
					// Furthest X position
					if (infoComponent.Sprite.Origin.X + infoComponent.TextWidth > componentLocationX)
					{
						componentLocationX = infoComponent.Sprite.Origin.X + infoComponent.TextWidth;
					}
				}
				else
				{
					// Furthest X position
					if (component.Sprite.Origin.X + component.Sprite.Width > componentLocationX)
					{
						componentLocationX = component.Sprite.Origin.X + component.Sprite.Width;
					}
				}
			}

			return componentLocationX;
		}

		/// <summary>
		/// Calculate the highest Y coordinate from the components within the container
		/// </summary>
		/// 
		/// <returns>Highest X coordinate</returns>
		private float CalculateHighestComponentY()
		{
			float componentLocationY = 0;

			// Find the component that is furthest down/right of the screen
			foreach (Component component in Container.ComponentManager)
			{
				// Info component does not have a good texture to grab the size from so checking another way
				if (component is InfoComponent infoComponent)
				{
					// Furthest Y position
					if (infoComponent.Sprite.Origin.Y + infoComponent.TextHeight > componentLocationY)
					{
						componentLocationY = infoComponent.Sprite.Origin.Y + infoComponent.TextHeight;
					}
				}
				else
				{
					// Furthest Y position
					if (component.Sprite.Origin.Y + component.Sprite.Height > componentLocationY)
					{
						componentLocationY = component.Sprite.Origin.Y + component.Sprite.Height;
					}
				}
			}

			return componentLocationY;
		}

		/// <summary>
		/// Handle checking whether the mouse is intersecting the UI
		/// </summary>
		/// <returns></returns>
		public bool Intersects()
		{
			return Container.Sprite.Bounds.Contains(Settings.MouseTransformLocation);
		}

		/// <summary>
		/// Handle checking if the mouse is clicked outside of the UI window
		/// </summary>
		/// <returns></returns>
		public bool IdleClick()
		{
			_currentMouse = Mouse.GetState();
			return _currentMouse.LeftButton == ButtonState.Pressed && !Intersects();
		}
	}
}
