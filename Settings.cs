using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Diode_Dominion
{
	public static class Settings
	{
		/// <summary>
		/// FPS that the game will attempt to run at.
		/// </summary>
		public static int Fps => 120;

		/// <summary>
		/// The default resolution of the game
		/// </summary>
		public static Vector2 GameDimensions { get; set; } = new Vector2(1280f, 720f);

		/// <summary>
		/// Information about the graphics that can be used outside of draw
		/// for object creation purposes.
		/// </summary>
		public static SpriteBatch SpriteBatch { get; set; }

		/// <summary>
		/// Used to load textures into the game.
		/// </summary>
		public static ContentManager Content { get; set; }

		/// <summary>
		/// How far from the origin the game has translated.
		/// At the beginning of the game it will be (0, 0) and
		/// as the player moves the camera this value will change.
		/// </summary>
		public static Rectangle GameTransform { get; set; }

		/// <summary>
		/// How much the games scale has changed has changed since the start of the game.
		/// The larger the scale value, the more zoomed in and vice versa.
		/// </summary>
		public static float GameScale { get; set; } = 1f;

		/// <summary>
		/// The rotation of the game. As the value changes from 0 to 360, the image will rotate.
		/// </summary>
		public static float GameRotation { get; set; } = 0.0f;

		/// <summary>
		/// The number of pixels that the camera pans by each update.
		/// </summary>
		public static int CameraPanSpeed { get; set; } = 5;

		/// <summary>
		/// This value will contain whether the game is currently active or if it is
		/// paused. Defaults to true on startup.
		/// </summary>
		public static bool IsGameActive { get; set; } = true;

		/// <summary>
		/// Whether the game has focus on the desktop
		/// </summary>
		public static bool DoesGameHaveFocus { get; set; } = true;

		/// <summary>
		/// Transformation caused by the camera movements
		/// </summary>
		public static Matrix Transform { get; set; }

		/// <summary>
		/// Create the mouse transform so that it grabs the correct location on the screen
		/// </summary>
		public static Vector2 MouseTransformLocation => Vector2.Transform(new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y), Matrix.Invert(Transform));

		/// <summary>
		/// Transparency of the in world game menus
		/// </summary>
		public static float MenuTransparency { get; set; } = 0.75f;

		/// <summary>
		/// The volume of the in-game music and sound effects.
		/// Can range from 0 (muted) to 1 (full volume)
		/// </summary>
		public static float Volume { get; set; } = 1f;

		/// <summary>
		/// Whether debug mode is active or not
		/// </summary>
		public static bool ActiveDebug { get; internal set; } = false;
	}
}
