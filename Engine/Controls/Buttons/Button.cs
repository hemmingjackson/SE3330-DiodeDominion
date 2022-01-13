using Diode_Dominion.Engine.Containers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Diode_Dominion.Engine.Sprites;

namespace Diode_Dominion.Engine.Controls.Buttons
{
	/// <summary>
	/// Handles the logic that goes into clicking a button
	/// </summary>
	internal class Button : Component
	{
		#region Fields

		/// <summary>
		/// Current state of the mouse
		/// </summary>
		private MouseState currentMouse;

		/// <summary>
		/// Previous state of the mouse
		/// </summary>
		private MouseState previousMouse;

		/// <summary>
		/// Whether the mouse cursor is over the button
		/// </summary>
		private bool isHovering;

		#endregion

		#region Properties

		/// <summary>
		/// Handle when the mouse is clicked
		/// </summary>
		public event EventHandler Click;

		/// <summary>
		/// Text color of the font
		/// </summary>
		public Color PenColor { get; set; }

		/// <summary>
		/// Text that is written on the button
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Font of the text on the button
		/// </summary>
		public SpriteFont Font { get; set; } = Fonts.Font.DefaultFont;

		/// <summary>
		/// Color of the button when being hovered over
		/// </summary>
		public Color HoverColor { get; set; } = Color.Gray;

		/// <summary>
		/// Color of the text when being hovered over
		/// </summary>
		public Color HoverTextColor { get; set; }

		#endregion

		#region Contructors

		/// <summary>
		/// Create a button with the supplied texture, position, and text
		/// </summary>
		/// 
		/// <param name="texture">Texture to apply to the button</param>
		/// <param name="position">Location of the button</param>
		/// <param name="text">Text to display on the button</param>
		public Button(Texture2D texture, Vector2 position, string text)
		{
			Sprite = new Sprite(position, texture);

			Text = text;
			PenColor = HoverTextColor = Color.Black;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Used to draw the texture of the button
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			// Only draw the button if it is visible
			if (Sprite.IsOnScreen && IsVisible)
			{
				Color color = Color.DarkGray;
				Color penColor = PenColor;

				if (isHovering)
				{
					color = HoverColor;
					penColor = HoverTextColor;
				}
				//spriteBatch.Draw(Sprite.Texture, Sprite.Origin, color);
				Sprite.Draw(gameTime, spriteBatch, color);


				// Add the string to the center point of the button
				if (!string.IsNullOrEmpty(Text) && Font != null)
				{
					float xCord = (Sprite.Origin.X + (float)Sprite.Width / 2) - (Font.MeasureString(Text).X / 2);
					float yCord = (Sprite.Origin.Y + (float)Sprite.Height / 2) - (Font.MeasureString(Text).Y / 2);

					spriteBatch.DrawString(Font, Text, new Vector2(xCord, yCord), penColor);
				}
			}
		}

		/// <summary>
		/// Update the button
		/// </summary>
		/// 
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
		{
			// Only take actions of the button is enabled
			if (IsEnabled)
			{
				previousMouse = currentMouse;
				currentMouse = Mouse.GetState();
				Rectangle mouseRect = Sprite.ShouldTransform ? 
					new Rectangle((int)Settings.MouseTransformLocation.X, (int)Settings.MouseTransformLocation.Y, 1, 1) : 
					new Rectangle(currentMouse.X, currentMouse.Y, 1, 1);
				Sprite.Update(gameTime);

				isHovering = false;

				// Check if mouse is intersecting the button
				if (mouseRect.Intersects(Sprite.Bounds))
				{
					isHovering = true;

					// Check that the mouse state is now not pressed, but once was pressed 
					if (currentMouse.LeftButton == ButtonState.Released && previousMouse.LeftButton == ButtonState.Pressed)
					{
						// Use click event handler is not null
						Click?.Invoke(this, new EventArgs());
					}
				}
			}
		}

		/// <summary>
		/// Unload all of the fields of the button
		/// </summary>
		public override void UnloadContent()
		{
			Font = null;
			Sprite = null;
			Text = null;
		}

		/// <summary>
		/// Sets the Container that the Button is located within
		/// </summary>
		/// 
		/// <param name="container">Object reference to the Container</param>
		public override void SetContainer(Container container)
		{
			Container = container;
		}

		#endregion
	}

	#region Button Builder

	/// <summary>
	/// Allows for the easy building of buttons
	/// </summary>
	internal class ButtonBuilder
	{
		private Texture2D texture;
		private SpriteFont font;
		private Vector2 position;
		private string displayText;

		public ButtonBuilder WithTexture(Texture2D btnTexture)
		{
			texture = btnTexture;
			return this;
		}

		public ButtonBuilder WithFont(SpriteFont btnFont)
		{
			font = btnFont;
			return this;
		}

		public ButtonBuilder WithPosition(Vector2 btnPosition)
		{
			position = btnPosition;
			return this;
		}

		public ButtonBuilder WithText(string btnText)
		{
			displayText = btnText;
			return this;
		}

		public Button Build()
		{
			Button button = new Button(texture, position, displayText);

			if (font != null)
			{
				button.Font = font;
			}

			return button;
		}
	}

	#endregion
}
