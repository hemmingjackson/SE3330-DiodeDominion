using System;
using Microsoft.Xna.Framework.Graphics;
using Diode_Dominion.Engine.Sprites;
using Diode_Dominion.Engine.Fonts;
using Microsoft.Xna.Framework;
using Diode_Dominion.Engine.Containers;

namespace Diode_Dominion.Engine.Controls.EntityInfoComponents
{
	public class InfoComponent : Component
	{
		public Color PenColor { get; set; } = Color.Black;

		/// <summary>
		/// Text that is displayed from the component
		/// </summary>
		public string DisplayText { get; set; }

		/// <summary>
		/// Width of the string
		/// </summary>
		public int TextWidth => (int)Font.DefaultFont.MeasureString(DisplayText).X;

		/// <summary>
		/// Height of the string
		/// </summary>
		public int TextHeight => (int)Font.DefaultFont.MeasureString(DisplayText).Y;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <param name="location"></param>
		public InfoComponent(string text, Vector2 location)
		{
			DisplayText = text;

			Sprite = new Sprite(location);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.DrawString(Font.DefaultFont, DisplayText, Sprite.Origin, PenColor);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime) { }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="container"></param>
		public override void SetContainer(Container container)
		{
			Container = container;
		}

		/// <summary>
		/// 
		/// </summary>
		public override void UnloadContent()
		{
		}
	}
}
