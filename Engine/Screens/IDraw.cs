using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Diode_Dominion.Engine.Screens
{
	/// <summary>
	/// Used by classes that wish to draw <see cref="Texture2D"/>
	/// to the screen.
	/// </summary>
	public interface IDraw
	{
		/// <summary>
		/// Everything that is going to be drawn should call this method
		/// </summary>
		/// 
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		void Draw(GameTime gameTime, SpriteBatch spriteBatch);
	}
}
