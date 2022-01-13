using System;
using System.Collections.Generic;
using System.Linq;
using Diode_Dominion.DiodeDominion.Entities;
using Diode_Dominion.DiodeDominion.Entities.Items;
using Diode_Dominion.DiodeDominion.Entities.StaticEntities.Stockpiles;
using Diode_Dominion.DiodeDominion.Textures;
using Diode_Dominion.DiodeDominion.World.UI.CommandPromptUI;
using Diode_Dominion.Engine.Containers;
using Diode_Dominion.Engine.Controls.Buttons;
using Diode_Dominion.Engine.Controls.EntityInfoComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Diode_Dominion.DiodeDominion.World.UI
{
	/// <summary>
	/// Displays information about a particular item
	/// </summary>
	class ItemInfoUi : EntityUi
	{
		#region Fields

		/// <summary>
		/// Information about the health of the static entity
		/// </summary>
		private InfoComponent nameInfo;


		/// <summary>
		/// Item the UI is based around
		/// </summary>
		internal Item ItemInfo { get; set; }

		/// <summary>
		/// Stockpiles that are available
		/// </summary>
		internal List<ConglomerateStockpile> Stockpile { get; set; }


		#endregion

		#region Constructors

		/// <summary>
		/// Create the UI with the proper colonist
		/// </summary>
		/// <param name="stockpile">All stockpiles in the game world</param>
		/// <param name="entities">Entities in the game world</param>
		/// <param name="item">Item for the UI</param>
		internal ItemInfoUi(List<ConglomerateStockpile> stockpile, IEnumerable<Entity> entities, Item item)
		{
			Entity = item;
			ItemInfo = item;
			Entities = entities;
			Stockpile = stockpile;
		}
		internal ItemInfoUi(IEnumerable<Entity> entities, Item item)
		{
			Entity = item;
			ItemInfo = item;
			Entities = entities;
		}
		#endregion

		#region Methods

		/// <summary>
		/// Handles Opening UI
		/// </summary>
		public override void Open()
		{
			CreateContainer();

			// Add all of the UI components
			CreateNameInfo();
			CreateItemTypeInfo();
			CreateItemDurabilityInfo();
			OpenCommandPromptButton();

			// Update the size of the Container if anything is going out of bounds
			Resize();

			// Created last so it is in the correct location if the container is resized 
			CreateExitButton();

			UserInterfaceManager.Instance.AddUserInterface(this);
		}

		/// <summary>
		/// Handles Updating UI
		/// </summary>
		/// <param name="gameTime"></param>
		public override void Update(GameTime gameTime)
		{
			Container.Update(gameTime);
			if (IdleClick())
			{
				Close();
			}
		}

		/// <summary>
		/// Handles Drawing UI
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			Container.Draw(gameTime, spriteBatch);
		}

		/// <summary>
		/// Handles closing UI
		/// </summary>
		public override void Close()
		{
			UserInterfaceManager.Instance.RemoveUserInterface(this);
		}

		#endregion

		#region UI Components

		/// <summary>
		/// Create the container that holds the UI components
		/// </summary>
		private void CreateContainer()
		{
			// Set the texture for the Container
			Color[] color = new Color[Width * Height];
			for (int i = 0; i < color.Length; i++)
			{
				color[i] = Color.Black;
			}

			// Create the container for the UI
			Texture2D containerTexture = new Texture2D(Settings.SpriteBatch.GraphicsDevice, Width, Height);
			containerTexture.SetData(color);
			Container = new EntityContainer(containerTexture, (int)Entity.EntitySprite.Origin.X + 50, (int)Entity.EntitySprite.Origin.Y);
		}

		/// <summary>
		/// Create the name info for the item
		/// </summary>
		private void CreateNameInfo()
		{
			nameInfo = new InfoComponent("Name: " + ItemInfo.Name, new Vector2(5, 5))
			{
				PenColor = Color.White
			};

			Container.AddComponent(nameInfo);
		}

		/// <summary>
		/// Creates the item type info for the item
		/// </summary>
		private void CreateItemTypeInfo()
		{
			nameInfo = new InfoComponent("Item type: " + ItemInfo.ItemType, new Vector2(5, 30))
			{
				PenColor = Color.White
			};

			Container.AddComponent(nameInfo);
		}

		/// <summary>
		/// Creates the durability for the item
		/// </summary>
		private void CreateItemDurabilityInfo()
		{
			nameInfo = new InfoComponent("Durability: " + ItemInfo.Health + "/" + ItemInfo.MaxHealth, new Vector2(5, 60))
			{
				PenColor = Color.White
			};

			Container.AddComponent(nameInfo);
		}

		/// <summary>
		/// Opens CommandPrompt to select colonist to perform action
		/// </summary>
		private void OpenCommandPromptButton()
		{
			const int btnLocation = 85;
			const int stockbtnLocation = 120;

			Button button = new Button(Settings.Content.Load<Texture2D>(TextureLocalization.CommandButton),
						new Vector2(0, btnLocation), "Pickup")
					{
						PenColor = Color.Green,
						HoverTextColor = Color.DarkGreen
					};

			Container.AddComponent(button);
			// Check which button is clicked
			button.Click += Button_Click;

			//Do not show if item is already in a stockpile
			if (Stockpile != null)
			{
				Button sendToStock = new Button(Settings.Content.Load<Texture2D>(TextureLocalization.CommandButton),
							new Vector2(45, stockbtnLocation), "Merge with Repo")
				{
					PenColor = Color.Green,
					HoverTextColor = Color.DarkGreen
				};

				Container.AddComponent(sendToStock);

				sendToStock.Click += SendToStock_Click;
			}
		}

		/// <summary>
		/// Creates a command prompt UI that asks a user which stockpile to send an item to
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SendToStock_Click(object sender, EventArgs e)
		{
			Close();
			if (Entity is Item item)
			{
				new CommandPromptDialogBox(Stockpile, Entities.ToList(), item).Open();
			}
		}

		/// <summary>
		/// Creates new command prompt UI based on the selected item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Button_Click(object sender, EventArgs e)
		{
			// Closes Info UI
			Close();
			if (Entity is Item item)
			{
				new CommandPromptDialogBox(Entities.ToList(), item).Open();
			}
		}

		#endregion

	}
}
