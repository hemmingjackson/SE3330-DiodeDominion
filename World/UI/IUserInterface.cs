using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Diode_Dominion.DiodeDominion.World.UI
{
	/// <summary>
	/// Describes the common behavior that must exist among all user interfaces
	/// </summary>
	public interface IUserInterface
	{
		/// <summary>
		/// Use this to draw the user interface and the components to the
		/// screen
		/// </summary>
		/// 
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		void Draw(GameTime gameTime, SpriteBatch spriteBatch);

		/// <summary>
		/// Use this to update the user interface and the components
		/// that are within the user interface
		/// </summary>
		/// 
		/// <param name="gameTime"></param>
		void Update(GameTime gameTime);

		/// <summary>
		/// Use this to open the user interface
		/// </summary>
		void Open();

		/// <summary>
		/// Use this to close the user interface
		/// </summary>
		void Close();

		/// <summary>
		/// Whether the mouse is intersecting the user interface
		/// </summary>
		/// <returns></returns>
		bool Intersects();

		/// <summary>
		/// Whether the mouse is clicked outside the user interface
		/// </summary>
		/// <returns></returns>
		bool IdleClick();
	}
}