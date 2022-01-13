using Diode_Dominion.DiodeDominion.Keybinds;
using Microsoft.Xna.Framework.Input;

namespace Diode_Dominion.DiodeDominion.World.Misc
{
	/// <summary>
	/// Handles the logic on whether the game should pause or resume
	/// </summary>
	internal class PauseGame
	{
		/// <summary>
		/// Previous key that was pressed
		/// </summary>
		private Keys previousKey;

		public void Update()
		{
			// Check if the pause key is pressed
			if (Keyboard.GetState().IsKeyDown(Shortcuts.PauseGame))
			{
				// Make certain the key is not double pressed
				if (previousKey != Shortcuts.PauseGame)
				{
					previousKey = Shortcuts.PauseGame;

					// Pause/resume the game
					Settings.IsGameActive = !Settings.IsGameActive;
				}
			}
			else
			{
				// Reset the previous key
				previousKey = Keys.None;
			}
		}
	}
}
