using Microsoft.Xna.Framework;

namespace Diode_Dominion.Engine.Camera
{
	/// <summary>
	/// This is the class that the camera will use to follow a box around the screen.
	/// This object will move and thus the camera will move with it.
	/// </summary>
	public class CameraBound
	{
		#region Fields

		/// <summary>
		/// Location that will hold the camera in place
		/// </summary>
		private Vector2 location;

		#endregion

		#region Properties

		/// <summary>
		/// Location that are used to determine the location that the camera should be pointing at.
		/// </summary>
		public Vector2 Location
		{
			get => location;
			private set => location = value;
		}

		/// <summary>
		/// X location of the location on the screen
		/// </summary>
		public float X
		{
			get => location.X;
			set => location.X = value;
		}

		/// <summary>
		/// Y location of the location on the screen.
		/// </summary>
		public float Y
		{
			get => location.Y;
			set => location.Y = value;
		}

		#endregion

		/// <summary>
		/// A camera location object that starts the camera at the center of the screen.
		/// </summary>
		public CameraBound()
		{
			// Default the coordinates of the camera location to the center of the screen
			// The size of the rectangle is arbitrary and does not affect the game in any
			// meaningful way. 
			Location = new Vector2(
				Settings.GameDimensions.X / 2,
				Settings.GameDimensions.Y / 2);
		}

		/// <summary>
		/// Move the camera location by (x, y) amount.
		/// This will add the value passed to the coordinates.
		/// Passing a negative value will result in it subtracting
		/// from the coordinates. 
		/// </summary>
		/// 
		/// <param name="deltaX">Change in X</param>
		/// <param name="deltaY">Change in Y</param>
		/// 
		/// <returns><see cref="Vector2"/> relating to the location of the camera</returns>
		public Vector2 MoveLocation(float deltaX, float deltaY)
		{
			X += deltaX;
			Y += deltaY;

			return Location;
		}

		/// <summary>
		/// Sets the absolute x, y location of the camera location.
		/// </summary>
		/// 
		/// <param name="x">X location of the camera location</param>
		/// <param name="y">Y location of the camera location</param>
		/// 
		/// <returns><see cref="Vector2"/> relating to the location of the camera</returns>
		public Vector2 SetLocation(int x, int y)
		{
			X = x;
			Y = y;

			return location;
		}
	}
}
