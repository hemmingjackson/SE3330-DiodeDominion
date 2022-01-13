using Diode_Dominion.DiodeDominion.Entities;
using Diode_Dominion.DiodeDominion.Entities.Animals;
using Diode_Dominion.DiodeDominion.Entities.Colonists;
using Diode_Dominion.DiodeDominion.Entities.Items;
using Diode_Dominion.DiodeDominion.Entities.Items.Weapons;
using Diode_Dominion.DiodeDominion.Entities.StaticEntities.Crops;
using Diode_Dominion.DiodeDominion.Entities.StaticEntities.Stockpiles;
using Diode_Dominion.DiodeDominion.Events;
using Diode_Dominion.DiodeDominion.Textures;
using Diode_Dominion.Engine.Containers;
using Diode_Dominion.Engine.Controls.Buttons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Diode_Dominion.DiodeDominion.World.UI.CommandPromptUI
{
	internal class CommandPromptDialogBox : EntityUi
	{
		#region Fields

		private readonly ConglomerateStockpile stockpile;

		private readonly IReadOnlyCollection<ConglomerateStockpile> stockpiles;
		private Vector2 entityOldLoc;

		#endregion

		/// <summary>
		/// Buttons inside EntityUi Container
		/// </summary>
		public List<Button> CommandButtons { get; set; }

		public List<Entity> Lists { get; } = new List<Entity>();

		public Crop Crop { get; }


		/// <summary>
		/// Create the UI with buttons to specify which colonist completes the intended task.
		/// </summary>
		/// <param name="entityList">List of entities that currently populate the game world</param>
		/// <param name="item">Item that the colonist will go and pick up</param>
		public CommandPromptDialogBox(IReadOnlyList<Entity> entityList, Item item)
		{
			Entity = item;

			CreateContainer(item.EntitySprite.Origin.X + 40, item.EntitySprite.Origin.Y + 25);
			CreateButtons(entityList);
			FormatButtons();
			Resize();
			CreateExitButton();
			foreach (Button b in CommandButtons)
			{
				b.Click += Colonist_Retrieve_Click;
			}
		}

		/// <summary>
		/// Create the UI with buttons to specify which colonist completes the intended task.
		/// </summary>
		/// <param name="entityList">List of entities that currently populate the game world</param>
		/// <param name="item">Item that the colonist will go and pick up</param>
		/// <param name="stockpile">Stockpile location where the colonist will store materials</param>
		public CommandPromptDialogBox(IReadOnlyList<Entity> entityList, Item item, ConglomerateStockpile stockpile)
		{
			Entity = item;
			this.stockpile = stockpile;

			CreateContainer(item.EntitySprite.Origin.X + 40, item.EntitySprite.Origin.Y + 25);
			CreateButtons(entityList);
			FormatButtons();
			Resize();
			CreateExitButton();
			foreach (Button b in CommandButtons)
			{
				b.Click += Colonist_Move_To_Stockpile;
			}
		}

		/// <summary>
		/// UI for sending a item to a certain stockpile
		/// </summary>
		/// <param name="stockpile">Stockpile location where the colonist will store materials</param>
		/// <param name="entityList">List of entities that currently populate the game world</param>
		/// <param name="item">Item that the colonist will go and pick up</param>
		public CommandPromptDialogBox(IReadOnlyCollection<ConglomerateStockpile> stockpile, IReadOnlyCollection<Entity> entityList, Item item)
		{
			stockpiles = stockpile;
			Entity = item;
			Entities = entityList;

			CreateContainer(item.EntitySprite.Origin.X + 40, item.EntitySprite.Origin.Y + 25);
			CommandButtons = new List<Button>();
			foreach(ConglomerateStockpile pile in stockpile)
			{
				if(!pile.IsFull())
					AddButtonsToList(Settings.Content, "Stockpile " + stockpile.ToList().IndexOf(pile));
			}
			foreach (Button button in CommandButtons)
			{
				button.Click += Colonist_Send_To_Stock;
			}
			if(CommandButtons.Count == 0)
			{
				AddButtonsToList(Settings.Content, "No Stockpiles to add to!");
			}
			AddButtonsToContainer();
			FormatButtons();
			Resize();
			CreateExitButton();

		
		}

		/// <summary>
		/// UI for sending a colonist to attack an animal
		/// </summary>
		/// <param name="entityList">List of entities that currently populate the game world</param>
		/// <param name="animal">Specified animal for the colonist to navigate towards</param>
		public CommandPromptDialogBox(IReadOnlyList<Entity> entityList, Animal animal)
		{
			Entity = animal;
			Entities = entityList;

			CreateContainer(animal.EntitySprite.Origin.X + 40, animal.EntitySprite.Origin.Y + 25);
			CreateButtons(entityList);
			FormatButtons();
			Resize();
			CreateExitButton();
			foreach (Button b in CommandButtons)
			{
				b.Click += Colonist_Attack_Click;
			}
		}
		
		/// <summary>
		/// Triggers an event that send a colonist to store an item in a stock pile
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Colonist_Send_To_Stock(object sender, EventArgs e)
		{
			// Obtain the stockpile number
			Button buttonClicked = (Button) sender;
			string[] btnName = buttonClicked.Text.Split(' ');
			int stockpileNumber = Convert.ToInt32(btnName[1]);

			Close();
			if (Entity is Item item)
			{
				new CommandPromptDialogBox(Entities.ToList(), item, stockpiles.ElementAt(stockpileNumber)).Open();
			}
		}

		/// <summary>
		/// Triggers an event that send a colonist to the stock pile's location
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Colonist_Move_To_Stockpile(object sender, EventArgs e)
		{
			foreach (Entity entity in Entities)
			{
				// Check for the proper name
				if (((Button)sender).Text == entity.Name)
				{
					// Send off the event
					entity.BaseAi.Update(new PickUpItemEvent(entity.Id, (Item)Entity));
					entity.BaseAi.Update(new MoveEntityEvent(entity.Id, stockpile.CenterLocation.X, stockpile.CenterLocation.Y));
					entity.BaseAi.Update(new DepositItemInStockpileEvent(entity.Id, (Item)Entity, stockpile));

					break;
				}
			}
			Close();
		}

		/// <summary>
		/// Triggers an event that send a colonist to the location of an item dropped in the game world.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Colonist_Retrieve_Click(object sender, EventArgs e)
		{
			
			foreach (Entity entity in Entities)
			{
				// Check for the proper name
				if (((Button)sender).Text == entity.Name)
				{
					// Send off the event
					entity.BaseAi.Update(new PickUpItemEvent(entity.Id, (Item)Entity));
					entity.AddHoldable((Item)Entity);
					break;
				}
			}
			Close();
		}

		/// <summary>
		/// Handles the click for when the user wants the colonist to attack an entity
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Colonist_Attack_Click(object sender, EventArgs e)
		{
			foreach (Entity entity in Entities)
			{
				// Check for the proper name
				if (((Button)sender).Text == entity.Name)
				{
					// Call the attack event, still waiting for implementation
					List<Weapon> weapons = entity.Items.Where(p => p is Weapon).Cast<Weapon>().ToList();
					new WeaponChoiceUi(weapons, entity, Entity).Open();
					//entity.BaseAi.Update(new DamageDealtEvent(entity, Entity));
					break;
				}
			}
			Close();
		}

		/// <summary>
		/// Create the Entity ui Container that holds the UI components
		/// </summary>
		/// <param name="xCoordinate">X Coordinate for the parent container</param>
		/// <param name="yCoordinate">X Coordinate for the parent container</param>
		public void CreateContainer( float xCoordinate, float yCoordinate )
		{
			Color[] color = new Color[Width * Height];
			for (int i = 0; i < color.Length; i++)
			{
				color[i] = Color.Black;
			}

			// Create the container for the UI
			Texture2D containerTexture = new Texture2D(Settings.SpriteBatch.GraphicsDevice, Width, Height);
			containerTexture.SetData(color);
			Container = new EntityContainer(containerTexture, (int)xCoordinate, (int)yCoordinate);
		}

		/// <summary>
		/// Create a button for each colonist in the entityList and adds it to the Entity Ui Container
		/// </summary>
		/// <param name="entityList">List of entities that currently populate the game world</param>
		public void CreateButtons(IReadOnlyList<Entity> entityList)
		{
			CommandButtons = new List<Button>();
			foreach (Entity entitySubset in entityList)
			{
				if (entitySubset is Colonist)
				{
					AddButtonsToList(Settings.Content, entitySubset.Name);
					Entities = entityList.Where(entity => entity is Colonist).ToList();
				}
			}
			AddButtonsToContainer();
		}

		/// <summary>
		/// Formats the buttons within the EntityUi Container
		/// </summary>
		public void FormatButtons()
		{
			float formatY = 5;
			foreach (Button button in CommandButtons)
			{
				button.Sprite.Origin = new Vector2(Container.Sprite.Origin.X + 60, Container.Sprite.Origin.Y + formatY);
				formatY += 50;
			}
		}

		/// <summary>
		/// Adds buttons to the class list of known buttons for updating and drawing purposes
		/// </summary>
		/// <param name="contentManager"></param>
		/// /// <param name="name"></param>
		public void AddButtonsToList(ContentManager contentManager, String name)
		{
			CommandButtons.Add(new Button(contentManager.Load<Texture2D>(TextureLocalization.CommandButton), new Vector2(0, 0), name));
		}

		/// <summary>
		/// Adds buttons from the class list CommandButtons to the EntityUi container.
		/// </summary>
		public void AddButtonsToContainer()
		{
			foreach (Button button in CommandButtons)
			{
				button.PenColor = Color.White;
				Container.AddComponent(button);
			}
		}

		/// <summary>
		/// Handles Updating the UI
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
		{
			if (Entity.EntitySprite.Origin != entityOldLoc)
			{
				Container.Sprite.Origin = new Vector2(Entity.EntitySprite.Origin.X + 50,
					 Entity.EntitySprite.Origin.Y);
				entityOldLoc = Entity.EntitySprite.Origin;
			}
			
			Container.Update(gameTime);
			foreach (Button button in CommandButtons)
			{
				button.Update(gameTime);
			}
		}

		/// <summary>
		/// Handles Drawing the UI
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			Container.Draw(gameTime, spriteBatch);
			foreach (Button button in CommandButtons)
			{
				button.Draw(gameTime, spriteBatch);
			}
		}

		/// <summary>
		/// Handling opening the UI
		/// </summary>
		public override void Open()
		{
			//this.CreateContainer();
			UserInterfaceManager.Instance.AddUserInterface(this);
		}

		/// <summary>
		/// Handling closing the UI
		/// </summary>
		public override void Close()
		{
			UserInterfaceManager.Instance.RemoveUserInterface(this);
		}
	}
}
