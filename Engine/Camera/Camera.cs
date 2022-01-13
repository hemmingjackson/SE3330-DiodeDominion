using Diode_Dominion.DiodeDominion.Keybinds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Diode_Dominion.Engine.Camera
{
	/// <summary>
	/// This class will allow for panning around the game world to occur.
	/// This will allow 
	/// </summary>
	public class Camera
	{
		#region Fields

		private static Vector2 _location;

		/// <summary>
		/// The amount that the camera view will scale by on every update cycle when pressed.
		/// </summary>
		private const float ScaleFactor = 0.002f;

		/// <summary>
		/// The furthest in the zoom is allowed to go with the camera
		/// </summary>
		private const float MaxScale = 1f;

		/// <summary>
		/// The furthest out the camera is allowed to go
		/// </summary>
		private const float MinScale = 0.5f;

		/// <summary>
		/// Holds the camera bounds. This will be used
		/// to determine where the camera should move towards.
		/// </summary>
		private readonly CameraBound cameraBound;

		#endregion

		#region Properties

		/// <summary>
		/// Location of the camera. Usable outside of the camera so that
		/// other classes can know where the player is looking.
		/// </summary>
		public static Vector2 Location => _location;

		/// <summary>
		/// This is a value passed to the sprite batch on
		/// spriteBatch.Begin(transformMatrix: Transform)
		/// 
		/// This will allow for the camera to move with the view
		/// </summary>
		public Matrix Transform { get; private set; }

		/// <summary>
		/// X location of the camera
		/// </summary>
		public float X
		{
			get => cameraBound.X;
			set => cameraBound.X = value;
		}

		/// <summary>
		/// Y location of the camera
		/// </summary>
		public float Y
		{
			get => cameraBound.Y;
			set => cameraBound.Y = value;
		}

		/// <summary>
		/// Number of pixels that the camera pans when the camera is told
		/// to move. Defaults to what is set in the game settings.
		/// </summary>
		public int PanSpeed { get; set; }

		/// <summary>
		/// Scale of the game screen. A smaller number will zoom the screen out more.
		/// Defaults to what is set in the game settings.
		/// </summary>
		public float Scale { get; set; } = Settings.GameScale;

		#endregion

		#region Constructors

		/// <summary>
		/// Create a camera object that can be used to pan around the screen.
		/// </summary>
		public Camera()
		{
			cameraBound = new CameraBound();
			TransformView();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Cause the camera view to update. Does not occur automatically.
		/// </summary>
		public void TransformView()
		{
			Transform = Matrix.CreateTranslation(-X, -Y, 0) *
				Matrix.CreateTranslation(Settings.GameDimensions.X / 2, Settings.GameDimensions.Y / 2, 0) *
				Matrix.CreateScale(Scale);

			Settings.Transform = Transform;

			// Set where the camera is looking
			_location.X = X;
			_location.Y = Y;
		}

		/// <summary>
		/// Move the camera by (x, y) amount.
		/// This will add the value passed to the coordinates.
		/// Passing a negative value will result in it subtracting
		/// from the coordinates. 
		/// </summary>
		/// 
		/// <param name="deltaX">Change in X</param>
		/// <param name="deltaY">Change in Y</param>
		/// 
		/// <returns><see cref="Vector2"/> relating to the location of the camera</returns>
		public Vector2 MoveBounds(float deltaX, float deltaY)
		{
			// Do not allow the camera to go out of the map
			if (cameraBound.X < 0 && deltaX < 0)
			{
				deltaX = 0;
			}
			/*
			else if (cameraBound.X > Settings.GameDimensions.X && deltaX > 0)
			{
				deltaX = 0;
			}
			*/
			
			if (cameraBound.Y < 0 && deltaY < 0)
			{
				deltaY = 0;
			}
			/*
			else if (cameraBound.Y > Settings.GameDimensions.Y && deltaY > 0)
			{
				deltaY = 0;
			}
			*/

			return cameraBound.MoveLocation(deltaX, deltaY);
		}

		/// <summary>
		/// Sets the absolute x, y location of the camera .
		/// </summary>
		/// 
		/// <param name="x">X location of the camera</param>
		/// <param name="y">Y location of the camera</param>
		/// 
		/// <returns><see cref="Vector2"/> relating to the location of the camera</returns>
		public Vector2 SetBounds(float x, float y)
		{
			X = x;
			Y = y;
			
			return cameraBound.Location;
		}

		/// <summary>
		/// Allow the camera to update
		/// </summary>
		public void Update()
		{
			// Change the pan speed based on the scale factor of the game
			PanSpeed = (int)(Settings.CameraPanSpeed / Scale);
			Settings.GameScale = Scale;

			// Handle moving right or left
			if (Keyboard.GetState().IsKeyDown(Shortcuts.MoveCameraRight))
			{
				MoveBounds(PanSpeed, 0);
				TransformView();
			}
			else if (Keyboard.GetState().IsKeyDown(Shortcuts.MoveCameraLeft))
			{
				MoveBounds(-PanSpeed, 0);
				TransformView();
			}

			// Handle moving up or down
			// Values are inverted as y = 0 is the top of the screen
			if (Keyboard.GetState().IsKeyDown(Shortcuts.MoveCameraUp))
			{
				MoveBounds(0, -PanSpeed);
				TransformView();
			}
			else if (Keyboard.GetState().IsKeyDown(Shortcuts.MoveCameraDown))
			{
				MoveBounds(0, PanSpeed);
				TransformView();
			}

			// Handle zooming the camera in and out
			if (Keyboard.GetState().IsKeyDown(Shortcuts.ZoomCameraIn))
			{
				if (Scale < MaxScale)
				{
					Scale += ScaleFactor;
					TransformView();
				}
			}
			else if (Keyboard.GetState().IsKeyDown(Shortcuts.ZoomCameraOut))
			{
				if (Scale > MinScale)
				{
					Scale -= ScaleFactor;
					TransformView();
				}
			}

		}

		#endregion
	}
}
