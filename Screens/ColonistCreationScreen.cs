using Diode_Dominion.Engine.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Diode_Dominion.Engine.Controls.Buttons;
using Diode_Dominion.Engine.Controls;
using Diode_Dominion.DiodeDominion.Entities.Colonists;
using Diode_Dominion.Engine.Entities;
using Diode_Dominion.DiodeDominion.Screens.CommandPattern;
using Diode_Dominion.DiodeDominion.Textures;


namespace Diode_Dominion.DiodeDominion.Screens
{
	/// <summary>
	/// Screen that allows the users to create multiple colonists
	/// </summary>
	internal class ColonistCreationScreen : GameScreen
	{
		#region Fields

		/// <summary>
		/// Invoker used for handling the undoing and redoing of skill incrementing/decrementing 
		/// and colonist randomization
		/// </summary>
		private CommandInvoker colonistCreationInvoker;

		/// <summary>
		/// List of components used in the colonist creation screen
		/// </summary>
		private List<Component> components;

		/// <summary>
		/// Texture of the background
		/// </summary>
		private Texture2D background;

		/// <summary>
		/// Font used for the screen
		/// </summary>
		private SpriteFont font;

		/// <summary>
		/// Texture used for the colonist portrait
		/// </summary>
		private Texture2D portrait;

		/// <summary>
		/// ColonistStats for each colonist
		/// </summary>
		private ColonistStatRandomizer[] colonistStats;

		/// <summary>
		/// Max skill points allowed for each skill
		/// </summary>
		private const int SkillPointMax = 10;

		/// <summary>
		/// The number of colonists that the game will start with
		/// </summary>
		private const int NumberOfColonists = 3;

		/// <summary>
		/// Current colonist selected by the user
		/// </summary>
		public int CurrentColonist { get; set; }

		#endregion
		#region BUTTON_LOCATIONS

		/// <summary>
		/// Y Offset of the arrow buttons
		/// </summary>
		private const int ArrowBtnComp = 220;

		/// <summary>
		/// X Offset of the left arrow button
		/// </summary>
		private const int LeftArrowXComp = 20;

		/// <summary>
		/// X Offset of the right arrow button
		/// </summary>
		private const int RightArrowXComp = 380;

		/// <summary>
		/// X Offset of the randomize colonist buttons
		/// </summary>
		private const int RandomizeColonistXComp = 10;

		/// <summary>
		/// y Offset of the variety of buttons located on the bottom of the 
		/// screen
		/// </summary>
		private const int ButtonButtonsYOffset = 660;

		/// <summary>
		/// X offset for the colonist portrait
		/// </summary>
		private const int PortraitXComp = 45;

		/// <summary>
		/// Y offset for the colonist portrait
		/// </summary>
		private const int PortraitYComp = 65;

		/// <summary>
		/// Width of the colonist portrait
		/// </summary>
		private const int PortraitWidthAndHeight = 320;

		/// <summary>
		/// X offset for the skill text printed on screen
		/// </summary>
		private const int SkillTextXComp = 640;

		/// <summary>
		/// Min Y offset for the skill point buttons
		/// </summary>
		private const int SkillYMin = 70;

		/// <summary>
		/// Space in between skill increment/decrement buttons
		/// </summary>
		private const int SkillYOffset = 60;

		/// <summary>
		/// Y Location of skill text
		/// </summary>
		private const int SkillText = 10;

		/// <summary>
		/// Location used for the y component for the increment buttons
		/// </summary>
		private const int IncrementButtonYOffset = 60;

		/// <summary>
		/// Location used for the x component for the increment buttons
		/// </summary>
		private const int IncrementButtonXOffset = 520;

		/// <summary>
		/// Location used for the x component for the decrement buttons
		/// </summary>
		private const int DecrementButtonXOffset = 580;

		/// <summary>
		/// X Comp used for skill points left text
		/// </summary>
		private const int SkillPointsLeftXComp = 800;

		/// <summary>
		/// X Comp used for colonist name and health
		/// </summary>
		private const int ColonistInfoXComp = 45;

		/// <summary>
		/// Y Comp used for colonist name
		/// </summary>
		private const int ColonistNameYComp = 420;

		/// <summary>
		/// Y Comp used for colonist health
		/// </summary>
		private const int ColonistHealthYComp = 460;

		#endregion

		/// <summary>
		/// Used to draw objects/text on screen
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			spriteBatch.Begin();
			base.Draw(gameTime, spriteBatch);

			spriteBatch.Draw(portrait, new Vector2(PortraitXComp, PortraitYComp), Color.White);
			spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

			Rectangle destinationRectangle = 
				new Rectangle(PortraitXComp, PortraitYComp, PortraitWidthAndHeight, PortraitWidthAndHeight);
			spriteBatch.Draw(portrait, destinationRectangle, Color.White);

			spriteBatch.DrawString(font, "Current Colonist: "
				+ (CurrentColonist + 1), new Vector2(0, 0), Color.Black);
			spriteBatch.DrawString(font, "Skill Points Left: " 
				+ colonistStats[CurrentColonist].ColonistPointsLeft, new Vector2(SkillPointsLeftXComp, SkillText), Color.Black);
			spriteBatch.DrawString(font, "Skills:", new Vector2(SkillTextXComp, SkillText), Color.Black);

			spriteBatch.DrawString(font, "Name: " 
				+ colonistStats[CurrentColonist].NameOfColonist, new Vector2(ColonistInfoXComp, ColonistNameYComp), Color.Black);
			spriteBatch.DrawString(font, "Health: "
				+ colonistStats[CurrentColonist].ColonistHealth, new Vector2(ColonistInfoXComp, ColonistHealthYComp), Color.Black);

			int offset = SkillYMin;
			spriteBatch.DrawString(font, "Mining: " 
				+ colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.MINING], new Vector2(SkillTextXComp, offset), Color.Black);
			offset += SkillYOffset;
			spriteBatch.DrawString(font, "Building: "
				+ colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.BUILDING], new Vector2(SkillTextXComp, offset), Color.Black);
			offset += SkillYOffset;
			spriteBatch.DrawString(font, "Melee: " 
				+ colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.MELEE], new Vector2(SkillTextXComp, offset), Color.Black);
			offset += SkillYOffset;
			spriteBatch.DrawString(font, "Harvesting: " 
				+ colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.HARVESTING], new Vector2(SkillTextXComp, offset), Color.Black);
			offset += SkillYOffset;
			spriteBatch.DrawString(font, "Crafting: " 
				+ colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.CRAFTING], new Vector2(SkillTextXComp, offset), Color.Black);
			offset += SkillYOffset;
			spriteBatch.DrawString(font, "Cooking: "
				+ colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.COOKING], new Vector2(SkillTextXComp, offset), Color.Black);
			offset += SkillYOffset;
			spriteBatch.DrawString(font, "Doctoring: " 
				+ colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.DOCTORING], new Vector2(SkillTextXComp, offset), Color.Black);

			//Drawing components
			foreach (Component comp in components)
			{
				comp.Draw(gameTime, spriteBatch);
			}
			spriteBatch.End();
		}
		/// <summary>
		/// Updates the components in the screen
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			//container.Update(gameTime);
			foreach (Component comp in components)
			{
				comp.Update(gameTime);
			}
		}
		/// <summary>
		/// Used to initialize anything that does not require the content manager
		/// </summary>
		public override void Initialize()
		{
			base.Initialize();
			colonistCreationInvoker = new CommandInvoker();
			CurrentColonist = 0;
			components = new List<Component>();

			colonistStats = new ColonistStatRandomizer[NumberOfColonists];
			for (int i = 0; i < NumberOfColonists; i++)
			{
				colonistStats[i] = new ColonistStatRandomizer(i);
			}
		}
		/// <summary>
		/// Used to initialize anything that requires the content manager.
		/// </summary>
		/// <param name="contentManager"></param>
		public override void LoadContent(ContentManager contentManager)
		{

			base.LoadContent(contentManager);

			portrait = Content.Load<Texture2D>(TextureLocalization.DDColonistProfile);
			font = Content.Load<SpriteFont>(TextureLocalization.Font);
			background = Content.Load<Texture2D>(TextureLocalization.ColonistCreationScreen);

			CreateAssortedButtons();
			CreateIncrementButtons();
			CreateDecrementButtons();

			foreach (Component component in components)
         {
				component.Sprite.ShouldTransform = false;
			}
		}
		/// <summary>
		/// Creates buttons that do not belong in a certain category
		/// </summary>
		private void CreateAssortedButtons()
		{
			ButtonBuilder btnBuilder = new ButtonBuilder();

	
			btnBuilder.WithTexture(Content.Load<Texture2D>(TextureLocalization.ArrowLeftButton))
				.WithPosition(new Vector2(LeftArrowXComp, ArrowBtnComp))
				.WithText("");
			Button arrowLeft = btnBuilder.Build();

			btnBuilder.WithTexture(Content.Load<Texture2D>(TextureLocalization.ArrowRightButton))
			.WithPosition(new Vector2(RightArrowXComp, ArrowBtnComp))
			.WithText("");
			Button arrowRight = btnBuilder.Build();


			btnBuilder.WithTexture(Content.Load<Texture2D>(TextureLocalization.BaseButton))
				.WithPosition(new Vector2(800, ButtonButtonsYOffset))
				.WithText("Undo last change");

			Button undoButton = btnBuilder.Build();

			btnBuilder.WithTexture(Content.Load<Texture2D>(TextureLocalization.BaseButton))
			.WithPosition(new Vector2(500, ButtonButtonsYOffset))
			.WithText("Redo last change");
			Button redoButton = btnBuilder.Build();

			btnBuilder.WithTexture(Content.Load<Texture2D>(TextureLocalization.BaseButton))
				.WithPosition(new Vector2(1070, ButtonButtonsYOffset))
				.WithText("Create Colonists");

			Button createColonists = btnBuilder.Build();

			btnBuilder.WithText("Randomize Colonist!")
				.WithPosition(new Vector2(RandomizeColonistXComp, ButtonButtonsYOffset));
			Button randomizeColonist = btnBuilder.Build();


			components.Add(undoButton);
			components.Add(redoButton);
			components.Add(createColonists);
			components.Add(arrowRight);
			components.Add(arrowLeft);
			components.Add(randomizeColonist);


			undoButton.Click += UndoButton_Click;
			redoButton.Click += RedoButton_Click;
			randomizeColonist.Click += RandomizeColonist_Click;
			createColonists.Click += CreateColonists_Click;
			arrowRight.Click += ArrowRight_Click;
			arrowLeft.Click += ArrowLeft_Click;
		}


		/// <summary>
		/// Randomizes the stats for the colonist
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RandomizeColonist_Click(object sender, EventArgs e)
		{
			colonistCreationInvoker.AddUndoCommand(new ColonistRandomizeCommand(colonistStats[CurrentColonist]));
			colonistStats[CurrentColonist].RandomizeStats();
		}

		/// <summary>
		/// Creates all buttons that increment a skill
		/// </summary>
		private void CreateIncrementButtons()
		{
			ButtonBuilder btnBuilder = new ButtonBuilder();

			btnBuilder.WithTexture(Content.Load<Texture2D>(TextureLocalization.IncrementButton));

			int buttonOffset = IncrementButtonYOffset;
			btnBuilder.WithPosition(new Vector2(IncrementButtonXOffset, buttonOffset));
			Button incrementMining = btnBuilder.Build();
			buttonOffset += IncrementButtonYOffset;
			btnBuilder.WithPosition(new Vector2(IncrementButtonXOffset, buttonOffset));
			Button incrementBuilding = btnBuilder.Build();
			buttonOffset += IncrementButtonYOffset;
			btnBuilder.WithPosition(new Vector2(IncrementButtonXOffset, buttonOffset));
			Button incrementMelee = btnBuilder.Build();
			buttonOffset += IncrementButtonYOffset;
			btnBuilder.WithPosition(new Vector2(IncrementButtonXOffset, buttonOffset));
			Button incrementHarvesting = btnBuilder.Build();
			buttonOffset += IncrementButtonYOffset;
			btnBuilder.WithPosition(new Vector2(IncrementButtonXOffset, buttonOffset));
			Button incrementCrafting = btnBuilder.Build();
			buttonOffset += IncrementButtonYOffset;
			btnBuilder.WithPosition(new Vector2(IncrementButtonXOffset, buttonOffset));
			Button incrementCooking = btnBuilder.Build();
			buttonOffset += IncrementButtonYOffset;
			btnBuilder.WithPosition(new Vector2(IncrementButtonXOffset, buttonOffset));
			Button incrementDoctoring = btnBuilder.Build();


			components.Add(incrementHarvesting);
			components.Add(incrementMelee);
			components.Add(incrementCrafting);
			components.Add(incrementDoctoring);
			components.Add(incrementMining);
			components.Add(incrementBuilding);
			components.Add(incrementCooking);

			incrementHarvesting.Click += IncrementHarvesting_Click;
			incrementMelee.Click += IncrementMelee_Click;
			incrementCrafting.Click += IncrementCrafting_Click;
			incrementDoctoring.Click += IncrementDoctoring_Click;
			incrementMining.Click += IncrementMining_Click;
			incrementBuilding.Click += IncrementBuilding_Click;
			incrementCooking.Click += IncrementCooking_Click;

		}
		/// <summary>
		/// Creates all buttons that decrement a skill
		/// </summary>
		private void CreateDecrementButtons()
		{
			ButtonBuilder btnBuilder = new ButtonBuilder();

			btnBuilder.WithTexture(Content.Load<Texture2D>(TextureLocalization.DecrementButton));

			int buttonOffset = IncrementButtonYOffset;
			btnBuilder.WithPosition(new Vector2(DecrementButtonXOffset, buttonOffset));
			Button decrementMining = btnBuilder.Build();
			buttonOffset += IncrementButtonYOffset;
			btnBuilder.WithPosition(new Vector2(DecrementButtonXOffset, buttonOffset));
			Button decrementBuilding = btnBuilder.Build();
			buttonOffset += IncrementButtonYOffset;
			btnBuilder.WithPosition(new Vector2(DecrementButtonXOffset, buttonOffset));
			Button decrementMelee = btnBuilder.Build();
			buttonOffset += IncrementButtonYOffset;
			btnBuilder.WithPosition(new Vector2(DecrementButtonXOffset, buttonOffset));
			Button decrementHarvesting = btnBuilder.Build();
			buttonOffset += IncrementButtonYOffset;
			btnBuilder.WithPosition(new Vector2(DecrementButtonXOffset, buttonOffset));
			Button decrementCrafting = btnBuilder.Build();
			buttonOffset += IncrementButtonYOffset;
			btnBuilder.WithPosition(new Vector2(DecrementButtonXOffset, buttonOffset));
			Button decrementCooking = btnBuilder.Build();
			buttonOffset += IncrementButtonYOffset;
			btnBuilder.WithPosition(new Vector2(DecrementButtonXOffset, buttonOffset));
			Button decrementDoctoring = btnBuilder.Build();

			components.Add(decrementHarvesting);
			components.Add(decrementMelee);
			components.Add(decrementCrafting);
			components.Add(decrementDoctoring);
			components.Add(decrementMining);
			components.Add(decrementBuilding);
			components.Add(decrementCooking);

			decrementHarvesting.Click += DecrementHarvesting_Click;
			decrementMelee.Click += DecrementMelee_Click;
			decrementCrafting.Click += DecrementCrafting_Click;
			decrementDoctoring.Click += DecrementDoctoring_Click;
			decrementMining.Click += DecrementMining_Click;
			decrementBuilding.Click += DecrementBuilding_Click;
			decrementCooking.Click += DecrementCooking_Click;
		}
		/// <summary>
		/// On click event for when cooking is incremented
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void IncrementCooking_Click(object sender, EventArgs e)
		{
			if (colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.COOKING] < SkillPointMax && (colonistStats[CurrentColonist].ColonistPointsLeft > 0))
			{
				colonistCreationInvoker.AddUndoCommand(new SkillIncrementCommand(SkillTypes.COOKING, colonistStats[CurrentColonist]));
				colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.COOKING]++;
				colonistStats[CurrentColonist].ColonistPointsLeft--;
			}
		}
		private void RedoButton_Click(object sender, EventArgs e)
		{
			colonistCreationInvoker.RedoLastCommand();
			
		}

		private void UndoButton_Click(object sender, EventArgs e)
		{
			colonistCreationInvoker.UndoLastCommand();
		}

		/// <summary>
		/// On click event for when builder is incremented
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void IncrementBuilding_Click(object sender, EventArgs e)
		{
			if (colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.BUILDING] < SkillPointMax && (colonistStats[CurrentColonist].ColonistPointsLeft > 0))
			{
				colonistCreationInvoker.AddUndoCommand(new SkillIncrementCommand(SkillTypes.BUILDING, colonistStats[CurrentColonist]));
				colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.BUILDING]++;
				colonistStats[CurrentColonist].ColonistPointsLeft--;
			}
		}
		/// <summary>
		/// On click event for when mining is incremented
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void IncrementMining_Click(object sender, EventArgs e)
		{
			if (colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.MINING] < SkillPointMax && (colonistStats[CurrentColonist].ColonistPointsLeft > 0))
			{
				colonistCreationInvoker.AddUndoCommand(new SkillIncrementCommand(SkillTypes.MINING, colonistStats[CurrentColonist]));
				colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.MINING]++;
				colonistStats[CurrentColonist].ColonistPointsLeft--;
			}
		}
		/// <summary>
		/// On click event for when doctoring is incremented
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void IncrementDoctoring_Click(object sender, EventArgs e)
		{
			if (colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.DOCTORING] < SkillPointMax && (colonistStats[CurrentColonist].ColonistPointsLeft > 0))
			{
				colonistCreationInvoker.AddUndoCommand(new SkillIncrementCommand(SkillTypes.DOCTORING, colonistStats[CurrentColonist]));
				colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.DOCTORING]++;
				colonistStats[CurrentColonist].ColonistPointsLeft--;
			}
		}
		/// <summary>
		/// On click event for when crafting is incremented
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void IncrementCrafting_Click(object sender, EventArgs e)
		{
			if (colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.CRAFTING] < SkillPointMax && (colonistStats[CurrentColonist].ColonistPointsLeft > 0))
			{
				colonistCreationInvoker.AddUndoCommand(new SkillIncrementCommand(SkillTypes.CRAFTING, colonistStats[CurrentColonist]));
				colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.CRAFTING]++;
				colonistStats[CurrentColonist].ColonistPointsLeft--;
			}
		}
		/// <summary>
		/// On click event for when melee is incremented
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void IncrementMelee_Click(object sender, EventArgs e)
		{
			if (colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.MELEE] < SkillPointMax && (colonistStats[CurrentColonist].ColonistPointsLeft > 0))
			{
				colonistCreationInvoker.AddUndoCommand(new SkillIncrementCommand(SkillTypes.MELEE, colonistStats[CurrentColonist]));
				colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.MELEE]++;
				colonistStats[CurrentColonist].ColonistPointsLeft--;
			}
		}
		/// <summary>
		/// On click event for when harvesting is incremented
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void IncrementHarvesting_Click(object sender, EventArgs e)
		{
			if (colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.HARVESTING] < SkillPointMax && (colonistStats[CurrentColonist].ColonistPointsLeft > 0))
			{
				colonistCreationInvoker.AddUndoCommand(new SkillIncrementCommand(SkillTypes.HARVESTING, colonistStats[CurrentColonist]));
				colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.HARVESTING]++;
				colonistStats[CurrentColonist].ColonistPointsLeft--;
			}
		}
		/// <summary>
		/// On click event for when cooking is decremented
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DecrementCooking_Click(object sender, EventArgs e)
		{
			if (colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.COOKING] > 0)
			{
				colonistCreationInvoker.AddUndoCommand(new SkillDecrementCommand(SkillTypes.COOKING, colonistStats[CurrentColonist]));
				colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.COOKING]--;
				colonistStats[CurrentColonist].ColonistPointsLeft++;
			}
		}
		/// <summary>
		/// On click event for when building is decremented
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DecrementBuilding_Click(object sender, EventArgs e)
		{
			if (colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.BUILDING] > 0)
			{
				colonistCreationInvoker.AddUndoCommand(new SkillDecrementCommand(SkillTypes.BUILDING, colonistStats[CurrentColonist]));
				colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.BUILDING]--;
				colonistStats[CurrentColonist].ColonistPointsLeft++;
			}
		}
		/// <summary>
		/// On click event for when mining is decremented
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DecrementMining_Click(object sender, EventArgs e)
		{
			if (colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.MINING] > 0)
			{
				colonistCreationInvoker.AddUndoCommand(new SkillDecrementCommand(SkillTypes.MINING, colonistStats[CurrentColonist]));
				colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.MINING]--;
				colonistStats[CurrentColonist].ColonistPointsLeft++;
			}
		}
		/// <summary>
		/// On click event for when doctoring is decremented
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DecrementDoctoring_Click(object sender, EventArgs e)
		{
			if (colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.DOCTORING] > 0)
			{
				colonistCreationInvoker.AddUndoCommand(new SkillDecrementCommand(SkillTypes.DOCTORING, colonistStats[CurrentColonist]));
				colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.DOCTORING]--;
				colonistStats[CurrentColonist].ColonistPointsLeft++;
			}
		}
		/// <summary>
		/// On click event for when crafting is decremented
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DecrementCrafting_Click(object sender, EventArgs e)
		{
			if (colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.CRAFTING] > 0)
			{
				colonistCreationInvoker.AddUndoCommand(new SkillDecrementCommand(SkillTypes.CRAFTING, colonistStats[CurrentColonist]));
				colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.CRAFTING]--;
				colonistStats[CurrentColonist].ColonistPointsLeft++;
			}
		}
		/// <summary>
		/// On click event for when melee is decremented
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DecrementMelee_Click(object sender, EventArgs e)
		{
			if (colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.MELEE] > 0)
			{
				colonistCreationInvoker.AddUndoCommand(new SkillDecrementCommand(SkillTypes.MELEE, colonistStats[CurrentColonist]));
				colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.MELEE]--;
				colonistStats[CurrentColonist].ColonistPointsLeft++;
			}
		}
		/// <summary>
		/// On click event for when harvesting is decremented
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DecrementHarvesting_Click(object sender, EventArgs e)
		{
			if (colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.HARVESTING] > 0)
			{
				colonistCreationInvoker.AddUndoCommand(new SkillDecrementCommand(SkillTypes.HARVESTING, colonistStats[CurrentColonist]));
				colonistStats[CurrentColonist].ListOfSkills[(int)SkillTypes.HARVESTING]--;
				colonistStats[CurrentColonist].ColonistPointsLeft++;
			}
		}
		/// <summary>
		/// Changes which colonist the user is editing when they press the 
		/// left arrow key
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ArrowLeft_Click(object sender, EventArgs e)
		{
			int colonistNum = CurrentColonist;
			if (CurrentColonist > 0)
				CurrentColonist--;
			else
				CurrentColonist = NumberOfColonists - 1;
			colonistCreationInvoker.AddUndoCommand(new ChangeColonistCommand(this, colonistNum, CurrentColonist));
		}
		/// <summary>
		/// Changes which colonist the user is editing when they press the 
		/// right arrow key
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ArrowRight_Click(object sender, EventArgs e)
		{
			int colonistNum = CurrentColonist;
			if (CurrentColonist < NumberOfColonists - 1)
				CurrentColonist++;
			else
				CurrentColonist = 0;
			colonistCreationInvoker.AddUndoCommand(new ChangeColonistCommand(this, colonistNum, CurrentColonist));
		}
		/// <summary>
		/// Creates the colonists that the user has created and creates an instance of the game creation screen
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CreateColonists_Click(object sender, EventArgs e)
		{
			Colonist[] colonists = new Colonist[NumberOfColonists];

			// Add the stats for each of the colonists
			for (int i = 0; i < NumberOfColonists; i++)
			{
				// Build the new colonist
				colonists[i] = colonistStats[i].CreateColonist();
				colonists[i].LoadContent(Settings.Content);
			}

			WorldCreationScreen worldCreationScreen = new WorldCreationScreen(colonists);
			worldCreationScreen.CreateComponents();

			ScreenManager.Instance.AddScreen(worldCreationScreen);
		}

	}
}