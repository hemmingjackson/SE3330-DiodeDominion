using Diode_Dominion.Engine.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Diode_Dominion.Engine.Sprites
{
	/// <summary>
	/// This class will hold textures, bounds, and methods
	/// that can be used to modify sprites. Methods to update
	/// the location of sprites will be used to apply view transformations
	/// on sprites so that the sprite is able to change with the camera view.
	/// </summary>
	public class Sprite : IUpdate, IDraw
	{
		#region Fields

		/// <summary>
		/// Current position of the sprite
		/// </summary>
		private Vector2 position;

		/// <summary>
		/// The pixel distance off-screen that sprite textures
		/// will continue to be rendered at.
		/// </summary>
		private const int ScreenOffset = 300;

		/// <summary>
		/// Holds the last transform matrix to see if there was an update.
		/// This should reduce the amount of mathematical operations drastically.
		/// </summary>
		private Matrix currentTransform;
		/// <summary>
		/// Holds the time when the texture was last updated
		/// </summary>
		private DateTime lastUpdated;
		/// <summary>
		/// Holds the current frame of an animation
		/// </summary>
		private int currentFrame;
		public bool IsAnimated { get; set; }
		#endregion

		#region Properties

		public bool ShouldTransform { get; set; } = true;
		/// <summary>
		/// Holds a reference to the current texture that is applied to the 
		/// sprite.
		/// </summary>
		public Texture2D Texture { get; set; }

		/// <summary>
		/// Holds the individual animations that exist for a sprite. When 
		/// changing the animation frame, set the Texture equal the specific
		/// index in the array.
		/// </summary>
		public Texture2D[] AnimationFrames { get; set; }

		/// <summary>
		/// Holds a rectangle that encompasses the sprite. This is useful
		/// when attempting to locate if the sprite is intersecting other
		/// sprites/rectangles. The bounds are very slightly smaller than the
		/// texture. This is to make it easier to see when debugging and there
		/// are many textures near each other.
		/// </summary>
		public Rectangle Bounds =>
			new Rectangle((int)(Origin.X * 1.001f),
				(int)(Origin.Y * 1.001f),
				(int)(Width * 0.99f),
				(int)(Height * 0.99f));

		/// <summary>
		/// Starting position of the sprite.
		/// Can be changed if the sprite moves
		/// </summary>
		public Vector2 Origin { get; set; }

		/// <summary>
		/// Current position of the sprite
		/// </summary>
		public Vector2 Position
		{
			get => position;
			set => position = value;
		}

		/// <summary>
		/// X location of the sprite within the game world
		/// </summary>
		public float X
		{
			get => position.X;
			set => position.X = value;
		}

		/// <summary>
		/// Y location of the sprite within the game world
		/// </summary>
		public float Y
		{
			get => position.Y;
			set => position.Y = value;
		}

		/// <summary>
		/// The width of the texture for the sprite.
		/// Changing this value will affect collision.
		/// </summary>
		public int Width => Texture?.Width ?? 0;

		/// <summary>
		/// The height of the texture for the sprite.
		/// Changing this value will affect collision.
		/// </summary>
		public int Height => Texture?.Height ?? 0;

		/// <summary>
		/// Whether the sprite is viewable on the game screen.
		/// </summary>
		public bool IsOnScreen { get; set; } = true;

		/// <summary>
		/// Used to determine whether the classes that have a sprite should update based on whether
		/// the game is paused or not.
		/// </summary>
		public bool ShouldUpdate => Settings.IsGameActive;

		/// <summary>
		/// Bounds for the texture to debug the location of the sprites bounds
		/// </summary>
		private Texture2D BoundsTexture { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Add a single sprite with default coordinates passed in but no texture.
		/// </summary>
		/// 
		/// <param name="coordinates">Coordinates of the rectangle</param>
		public Sprite(Vector2 coordinates)
		{
			// Set the coordinates of the sprite using the passed values to avoid potential invalid external changes
			(float x, float y) = coordinates;
			Origin = new Vector2(x, y);
			position = new Vector2(x, y);
		}

		/// <summary>
		/// Add a single sprite texture along with the corresponding coordinates of the <see cref="Texture2D"/>
		/// passed in through the <see cref="Rectangle"/> object.
		/// </summary>
		/// 
		/// <param name="spriteTexture">Texture of the sprite</param>
		/// <param name="coordinates">Coordinates of the rectangle</param>
		public Sprite(Vector2 coordinates, Texture2D spriteTexture)
		{
			// Set the coordinates of the sprite using the passed values to avoid potential invalid external changes
			(float x, float y) = coordinates;
			Origin = new Vector2(x, y);
			position = new Vector2(x, y);

			// Set the texture for the sprite
			Texture = spriteTexture;

			// Create the colors for the bounds texture
			Color[] color = new Color[spriteTexture.Height * spriteTexture.Width];
			for (int i = 0; i < color.Length; i++)
			{
				color[i] = Color.Transparent;
			}
			BoundsTexture = new Texture2D(Settings.SpriteBatch.GraphicsDevice, spriteTexture.Width, spriteTexture.Height);
			BoundsTexture.SetData(color);
		}

		/// <summary>
		/// Creates a sprite object from the coordinates of the passed in <see cref="Rectangle"/> and the
		/// <see cref="Texture2D"/>.
		/// </summary>
		/// 
		/// <param name="coordinates"></param>
		/// <param name="spriteTexture"></param>
		public Sprite(Rectangle coordinates, Texture2D spriteTexture) : this(new Vector2(coordinates.X, coordinates.Y), spriteTexture)
		{ }

		/// <summary>
		/// Handle the creation of a sprite that can take many textures at a time.
		/// The first <see cref="Texture2D"/> that is passed in will be the one that is shown first.
		/// </summary>
		/// 
		/// <param name="coordinates">Location of the sprite</param>
		/// <param name="animationTextures">Animation frames for the texture</param>
		public Sprite(Vector2 coordinates, params Texture2D[] animationTextures) : this(coordinates, animationTextures[0])
		{
			(float x, float y) = coordinates;
			Origin = new Vector2(x, y);
			position = new Vector2(x, y);
			// Set the animation frames
			AnimationFrames = animationTextures;
			
		}

		#endregion

		#region Methods

		/// <summary>
		/// Determine whether this <see cref="Sprite"/>s is colliding
		/// with the passed in <see cref="Sprite"/>.
		/// </summary>
		/// 
		/// <param name="sprite">Sprite to check for collision</param>
		/// 
		/// <returns>Whether a collision is occurring</returns>
		public virtual bool IsColliding(Sprite sprite)
		{
			return Bounds.Intersects(sprite.Bounds);
		}

		/// <summary>
		/// Handle the transformation of the bounds of the texture based on the movement of
		/// camera. This is done using <see cref="Matrix"/> translations.
		/// </summary>
		private void TransformBounds()
		{
			// Found @
			// https://gamedev.stackexchange.com/questions/57622/matrix-transforms-in-xna

			// Create the matrix for the transform
			Matrix matrix =
				Matrix.CreateTranslation(Origin.X, Origin.Y, 0.0f) *
				Settings.Transform;

			// Update the x, y coordinates of the sprite
			X = matrix.Translation.X;
			Y = matrix.Translation.Y;

			// Determine whether the sprite is on the screen
			IsOnScreen = Position.X - ScreenOffset < Settings.GameDimensions.X &&
							 Position.X + Texture.Width + ScreenOffset > 0 &&
							 Position.Y - ScreenOffset < Settings.GameDimensions.Y &&
							 Position.Y + Texture.Height + ScreenOffset > 0;
		}

		/// <summary>
		/// Sets the location of the origin
		/// </summary>
		/// 
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void OriginSet(float x, float y)
		{
			Vector2 newLocation = new Vector2(x, y);
			Origin = newLocation;
		}

		/// <summary>
		/// Update the <see cref="Bounds"/>. Also checking to
		/// see if the sprite is going off bounds or not. 
		/// </summary> 
		/// <param name="gameTime"></param>
		public virtual void Update(GameTime gameTime)
		{
			if (currentTransform != Settings.Transform && ShouldTransform)
			{
				currentTransform = Settings.Transform;
				TransformBounds();
			}
			if((DateTime.Now - lastUpdated).TotalSeconds > .2)
			{
				lastUpdated = DateTime.Now;
				if(AnimationFrames != null && AnimationFrames.Length > 2)
				{
					if (currentFrame < AnimationFrames.Length - 2)
						currentFrame++;
					else
						currentFrame = 0;
					//Texture = AnimationFrames[currentFrame];
				}
			}
		}

		/// <summary>
		/// Handle the rendering of <see cref="Texture"/>.
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			Draw(gameTime, spriteBatch, Color.White);
			
		}

		/// <summary>
		/// Handle the rendering of <see cref="Texture"/>.
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		/// <param name="color">Color overlay</param>
		public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch, Color color)
		{
			// Only draw if the item is on the screen
			if (IsOnScreen)
			{
				if (IsAnimated)
				{
					spriteBatch.Draw(AnimationFrames[currentFrame], Origin, color);
				}
				else
				{
					spriteBatch.Draw(Texture, Origin, color);

					if (Settings.ActiveDebug)
					{
						spriteBatch.Draw(BoundsTexture, Bounds, Color.Purple * 0.50f);
					}
				}
			}
		}

		/// <summary>
		/// Returns a string of the sprites information.
		/// This includes the current <see cref="Texture"/> and <see cref="Bounds"/>.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return "Texture: " + Texture +
					 ", Origin:  " + Origin +
					 ", Bounds:  " + Bounds;
		}

		#endregion
	}
}