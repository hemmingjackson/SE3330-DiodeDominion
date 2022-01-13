using Microsoft.Xna.Framework.Input;

namespace Diode_Dominion.DiodeDominion.Keybinds
{
	/// <summary>
	/// Holds the static value of the shortcuts that the game is using.
	/// </summary>
	public class Shortcuts
	{
		/// <summary>
		/// Constructor is private as only static instances of the values in this class are useful.
		/// </summary>
		private Shortcuts() { }

		/// <summary>
		/// Key that will move the camera up in the game world
		/// </summary>
		public static Keys MoveCameraUp { get; set; } = Keys.Up;

		/// <summary>
		/// Key that will move the camera right in the game world
		/// </summary>
		public static Keys MoveCameraRight { get; set; } = Keys.Right;

		/// <summary>
		/// Key that will move the camera down in the game world
		/// </summary>
		public static Keys MoveCameraDown { get; set; } = Keys.Down;

		/// <summary>
		/// Key that will move the camera left in the game world
		/// </summary>
		public static Keys MoveCameraLeft { get; set; } = Keys.Left;

		/// <summary>
		/// Key that will zoom the camera in, increasing the game's scale
		/// </summary>
		public static Keys ZoomCameraIn { get; set; } = Keys.OemPlus;

		/// <summary>
		/// Key that will zoom the camera out, decreasing the game's scale
		/// </summary>
		public static Keys ZoomCameraOut { get; set; } = Keys.OemMinus;

		/// <summary>
		/// Key that can be used to pause or resume the game
		/// </summary>
		public static Keys PauseGame { get; set; } = Keys.P;
	}
}
