using Diode_Dominion.Engine.GameTimer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Diode_Dominion
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class DiodeDominionGame : Game
	{
		private readonly GraphicsDeviceManager graphics;
		private SpriteBatch spriteBatch;
		private GameTimer timer;

		public DiodeDominionGame()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			// Attempt to have equal times between game frames
			IsFixedTimeStep = true;

			// Do not attempt to synchronize the game frame rate with the monitors
			graphics.SynchronizeWithVerticalRetrace = false;

			// Attempt to have 120 frames per second within the game world
			TargetElapsedTime = new System.TimeSpan(0, 0, 0, 0, 1000 / Settings.Fps);

			// Apply changes made to the graphical settings
			graphics.ApplyChanges();
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			// Initialize the screen manager
			Engine.Screens.ScreenManager.Instance.Initialize();

			// Set the default screen dimensions based on the settings class
			Engine.Screens.ScreenManager.Instance.Dimensions = Settings.GameDimensions;

			// Set the dimensions of the game window
			graphics.PreferredBackBufferWidth = (int)Engine.Screens.ScreenManager.Instance.Dimensions.X;
			graphics.PreferredBackBufferHeight = (int)Engine.Screens.ScreenManager.Instance.Dimensions.Y;
			graphics.ApplyChanges();

			// Allow the mouse to be visible withing the game window
			IsMouseVisible = true;

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
			Settings.SpriteBatch = spriteBatch;
			Settings.Content = Content;

			// Set the default font at startup
			Engine.Fonts.Font.DefaultFont = Content.Load<SpriteFont>("Fonts/Font");

			Engine.Screens.ScreenManager.Instance.LoadContent(Content);

			// Creates Timer, giving its position and font.
			timer = new GameTimer(this, 0.0f)
			{
				Font = Content.Load<SpriteFont>("Fonts/TimerFont")
			};
			timer.Position = new Vector2((float)Window.ClientBounds.Width / 2 - timer.Font.MeasureString(timer.Text).X / 2, 0);
			Components.Add(timer);
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// 
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			Settings.DoesGameHaveFocus = IsActive;

			// Update the sound manager
			Engine.Sounds.SoundManager.Instance.Update();

			// Only allow the game to run while it has focus
			if (IsActive)
			{
				if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
					Exit();

				// Update the screen manager and held screens
				Engine.Screens.ScreenManager.Instance.Update(gameTime);
				
				base.Update(gameTime);
				
				// Starts timer upon start of game, pauses when the window loses focus.
				timer.Started = true;
				timer.Paused = !IsActive;
			}
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// 
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			// Only allow the game to draw while it has focus
			if (IsActive)
			{
				GraphicsDevice.Clear(Color.TransparentBlack);

				Engine.Screens.ScreenManager.Instance.Draw(gameTime, spriteBatch);

				//Draws Timer Sprite
				spriteBatch.Begin();
				timer.Draw(spriteBatch);
				spriteBatch.End();

				base.Draw(gameTime);
			}
		}
	}
}