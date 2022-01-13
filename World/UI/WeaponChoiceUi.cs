using System;
using System.Collections.Generic;
using System.Linq;
using Diode_Dominion.DiodeDominion.Entities;
using Diode_Dominion.DiodeDominion.Entities.Animals;
using Diode_Dominion.DiodeDominion.Entities.Colonists;
using Diode_Dominion.DiodeDominion.Entities.Items.Weapons;
using Diode_Dominion.DiodeDominion.Events;
using Diode_Dominion.DiodeDominion.Textures;
using Diode_Dominion.Engine.Containers;
using Diode_Dominion.Engine.Controls.Buttons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Diode_Dominion.DiodeDominion.World.UI
{
	internal class WeaponChoiceUi : EntityUi
	{

		/// <summary>
		/// Buttons inside EntityUi Container
		/// </summary>
		public List<Button> CommandButtons { get; set; }
		private List<Weapon> AvailableWeapons { get; }
		private Vector2 entityOldLoc;
		private Animal Target { get; }

		/// <summary>
		/// Create the UI with buttons to specify which colonist completes the intended task.
		/// </summary>
		/// <param name="weaponList"></param>
		/// <param name="attackingColonist"></param>
		/// <param name="targetAnimal"></param>
		public WeaponChoiceUi(List<Weapon> weaponList, Entity attackingColonist, Entity targetAnimal)
		{
			Entity = attackingColonist;
			AvailableWeapons = weaponList;
			Target = (Animal)targetAnimal;

			CreateContainer(targetAnimal.EntitySprite.Origin.X + 40, targetAnimal.EntitySprite.Origin.Y + 25);
			CreateButtons(AvailableWeapons);
			FormatButtons();
			Resize();
			CreateExitButton();
			foreach (Button b in CommandButtons)
			{
				b.Click += Choose_Weapon_Click;
			}
		}

		/// <summary>
		/// Triggers an event that send a colonist to the location of an item dropped in the game world.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Choose_Weapon_Click(object sender, EventArgs e)
		{
			if (AvailableWeapons.Count != 0)
			{
				foreach (Weapon weapon in 
					AvailableWeapons.Where(weapon => ((Button)sender).Text == weapon.Name))
				{
					// Send off the event
					Entity.BaseAi.Update(new DamageDealtEvent(Entity, Target, weapon));
					break;
				}
			}
			else
			{
				Entity.BaseAi.Update(new DamageDealtEvent(Entity, Target));
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
			if (!entityList.Any())
			{
				AddButtonsToList(Settings.Content, "Attack without weapon");
			}
			else
			{
				foreach (Entity entitySubset in entityList)
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
			if (Target.EntitySprite.Origin != entityOldLoc)
			{
				Container.Sprite.Origin = new Vector2(Target.EntitySprite.Origin.X + 50,
					 Target.EntitySprite.Origin.Y);
				entityOldLoc = Target.EntitySprite.Origin;
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
