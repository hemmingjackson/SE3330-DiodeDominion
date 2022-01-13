using Diode_Dominion.DiodeDominion.Textures;
using Diode_Dominion.Engine.Controls;
using Diode_Dominion.Engine.Controls.Buttons;
using Diode_Dominion.Engine.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Diode_Dominion.DiodeDominion.Screens
{
	/// <summary>
	/// Title screen for the diode dominion game
	/// </summary>
	internal class TitleScreen : GameScreen
	{
		#region Fields
		private const int ExitButtonXPosition = 2;
		private const int ExitButtonYPosition = 2;
		private const float PlayButtonYPosition = 2.5f;
		private const int PlayButtonXPosition = 2;
		/// <summary>
		/// Half the width of the button used
		/// </summary>
		private const int HalfButtonWidth = 100;
		/// <summary>
		/// Button to play the game
		/// </summary>
		private List<Component> components;
		/// <summary>
		/// Background used by the screen
		/// </summary>
		private Texture2D background;
		#endregion

		#region Methods
		/// <summary>
		/// Loads textures for the buttons and the background
		/// </summary>
		/// <param name="contentManager"></param>

		public override void LoadContent(ContentManager contentManager)
		{
			base.LoadContent(contentManager);

			// Create a button and add it to the container
			ButtonBuilder btnBuilder = new ButtonBuilder();
			background = Content.Load<Texture2D>(TextureLocalization.TitleScreen);
			
			btnBuilder.WithTexture(Content.Load<Texture2D>(TextureLocalization.BaseButton))
				 .WithPosition(
				new Vector2((Settings.GameDimensions.X / PlayButtonXPosition) - HalfButtonWidth, 
				Settings.GameDimensions.Y / PlayButtonYPosition))
				 .WithText("Play Game!");
			Button playGame = btnBuilder.Build();
			components.Add(playGame);
		
			btnBuilder.WithTexture(Content.Load<Texture2D>(TextureLocalization.BaseButton))
				 .WithPosition(
				new Vector2(
					(Settings.GameDimensions.X / ExitButtonXPosition) - HalfButtonWidth, 
					(Settings.GameDimensions.Y / ExitButtonYPosition)))
				 .WithText("Exit Game");
			Button exitGame = btnBuilder.Build();
			components.Add(exitGame);


			playGame.Click += PlayGame_Click;
			exitGame.Click += ExitGame_Click;
			foreach (Component component in components)
			{
				component.Sprite.ShouldTransform = false;
			}
		}
		/// <summary>
		/// Handles the event of where the user pushes the exit game button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ExitGame_Click(object sender, EventArgs e)
		{
			Environment.Exit(1);
		}
		/// <summary>
		/// Handles the event of where the user pushes the play game button.
		/// Creates a new instance of a colonist creation screen and pushes 
		/// the screen to the stack
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PlayGame_Click(object sender, EventArgs e)
		{
			ColonistCreationScreen colonistCreationScreen = new ColonistCreationScreen();
			ScreenManager.Instance.AddScreen(colonistCreationScreen);
		}

		/// <summary>
		/// Updates components in the screen
		/// </summary>
		/// <param name="gameTime"></param>

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			foreach (Component comp in components)
			{
				comp.Update(gameTime);
			}
		}
		/// <summary>
		/// Initializes the components in the title screen
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();
			components = new List<Component>();

		}
		/// <summary>
		/// Draws components on the screen
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Begin();
			base.Draw(gameTime, spriteBatch);
			spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
			foreach (Component comp in components)
			{
				comp.Draw(gameTime, spriteBatch);
			}
			spriteBatch.End();
		}

		#endregion
	}
}
