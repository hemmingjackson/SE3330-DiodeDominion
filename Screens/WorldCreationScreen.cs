using System;
using System.Collections.Generic;
using Diode_Dominion.DiodeDominion.Entities;
using Diode_Dominion.DiodeDominion.Screens.CommandPattern;
using Diode_Dominion.DiodeDominion.World;
using Diode_Dominion.Engine.Controls;
using Diode_Dominion.Engine.Controls.Buttons;
using Diode_Dominion.Engine.Controls.EntityInfoComponents;
using Diode_Dominion.Engine.Fonts;
using Diode_Dominion.Engine.GameTimer;
using Diode_Dominion.Engine.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Diode_Dominion.DiodeDominion.Screens
{
	/// <summary>
	/// Allows for values to be specified when creating a game world
	/// </summary>
	public class WorldCreationScreen : GameScreen
	{
		#region Fields

		/// <summary>
		/// Random value chosen based on the current game time
		/// </summary>
		private readonly Random random;

		/// <summary>
		/// Holds the entities that are a part of the game world
		/// </summary>
		private readonly IEnumerable<Entity> entities;

		/// <summary>
		/// Components that are on the screen
		/// </summary>
		private readonly List<Component> components;

		/// <summary>
		/// Displays the value of the current game world seed
		/// </summary>
		private InfoComponent randomSeed;

		/// <summary>
		/// Invokes the undoing and redoing of the different seeds
		/// </summary>
		private readonly CommandInvoker invoker;

		#endregion
		
		#region Constructors
		
		internal WorldCreationScreen(IEnumerable<Entity> entities)
		{
			this.entities = entities;
			components = new List<Component>();
			random = new Random((int)GameTimer.Time);
			invoker = new CommandInvoker();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Create all of the components for the game world
		/// </summary>
		internal void CreateComponents()
		{
			Texture2D baseButtonTexture = Settings.Content.Load<Texture2D>("Button/BaseButton");
			
			// Create the string component to view the random seed
			int seed = random.Next();
			float centerX = Settings.GameDimensions.X / 2 - Font.DefaultFont.MeasureString(seed.ToString()).X;
			float centerY = Settings.GameDimensions.Y / 2 - Font.DefaultFont.MeasureString(seed.ToString()).Y;

			randomSeed = new InfoComponent(seed.ToString(), new Vector2(centerX, centerY))
			{
				PenColor = Color.White
			};

			components.Add(randomSeed);

			// Create the button to change the random seed
			Button changeRandomSeedButton =
				new Button(baseButtonTexture, new Vector2(centerX, centerY + 50), "Change Seed")
				{
					Sprite = {ShouldTransform = false}
				};

			// Create the button event to change the seed
			changeRandomSeedButton.Click += (sender, args) =>
			{
				int currentSeed = int.Parse(randomSeed.DisplayText);
				int nextSeed = random.Next();

				randomSeed.DisplayText = nextSeed.ToString();

				invoker.AddUndoCommand(new SeedChangeCommand(this, currentSeed, nextSeed));
			};

			components.Add(changeRandomSeedButton);

			// Create the button to go to the game world
			Button createGameWorldButton = new Button(baseButtonTexture,
				new Vector2(Settings.GameDimensions.X - baseButtonTexture.Width - 10,
					Settings.GameDimensions.Y - baseButtonTexture.Height - 10), "Create World")
			{
				Sprite = {ShouldTransform = false}
			};


			// Handle what should occur when the button is clicked
			createGameWorldButton.Click += (sender, args) =>
			{
				ScreenManager.Instance.GoBackOneScreen();
				ScreenManager.Instance.AddScreen(new GameWorld(entities, int.Parse(randomSeed.DisplayText)));
				GameTimer.GameStartTime = GameTimer.Time;
			};

			components.Add(createGameWorldButton);

			// Add the undo button
			Button undoSeedButton = new Button(baseButtonTexture,
				new Vector2(centerX - changeRandomSeedButton.Sprite.Width - 20, centerY + 50), "Undo Seed")
			{
				Sprite = {ShouldTransform = false}
			};

			undoSeedButton.Click += (sender, args) =>
			{
				invoker.UndoLastCommand();
			};

			components.Add(undoSeedButton);

			// Add the redo button
			Button redoSeedButton = new Button(baseButtonTexture,
				new Vector2(centerX + changeRandomSeedButton.Sprite.Width + 20, centerY + 50), "Redo Seed")
			{
				Sprite = {ShouldTransform = false}
			};

			redoSeedButton.Click += (sender, args) =>
			{
				invoker.RedoLastCommand();
			};

			components.Add(redoSeedButton);
		}

		/// <summary>
		/// Draws the components on the screen
		/// </summary>
		/// 
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Begin();

			base.Draw(gameTime, spriteBatch);

			foreach (Component component in components)
			{
				component.Draw(gameTime, spriteBatch);
			}

			spriteBatch.End();
		}

		/// <summary>
		/// Update the screen elements
		/// </summary>
		/// 
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			foreach (Component component in components)
			{
				component.Update(gameTime);
			}
		}

		/// <summary>
		/// Sets the seed for the world generation
		/// </summary>
		/// <param name="seed"></param>
		public void SetSeed(int seed)
		{
			randomSeed.DisplayText = seed.ToString();
		}

		#endregion
	}
}
