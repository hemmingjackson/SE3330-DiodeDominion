using Diode_Dominion.Engine.Containers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Diode_Dominion.Engine.Controls.EntityInfoComponents
{
	internal class DisplayItemInfo : Component
	{
		#region Properties

		/// <summary>
		/// Text color of the font
		/// </summary>
		public Color PenColor { get; set; }

		/// <summary>
		/// Text that is written on the text box
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Font of the text on the text box
		/// </summary>
		public SpriteFont Font { get; set; } = Fonts.Font.DefaultFont;

		#endregion

		#region Methods

		/// <summary>
		/// Used to draw the texture of the text boxes
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Begin(); 
		
		}
		/// <summary>
		/// Update the text box
		/// </summary>
		/// 
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime) { }

		/// <summary>
		/// Sets the Container that the text box is located within
		/// </summary>
		/// <param name="container"></param>
		public override void SetContainer(Container container)
		{
			Container = container;
		}

		/// <summary>
		/// Unload all of the fields of the text box
		/// </summary>
		public override void UnloadContent()
		{
			Font = null;
			Sprite = null;
			Text = null;
		}

		#endregion
	}
}
