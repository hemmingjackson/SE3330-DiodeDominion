using Microsoft.Xna.Framework;

namespace Diode_Dominion.Engine.Screens
{
	/// <summary>
	/// This interface is used to update game classes on
	/// every update tick
	/// </summary>
	public interface IUpdate
	{
		/// <summary>
		/// Update the components of the game
		/// </summary>
		/// <param name="gameTime"></param>
		void Update(GameTime gameTime);
	}
}
