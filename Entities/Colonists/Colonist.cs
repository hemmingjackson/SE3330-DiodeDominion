using Diode_Dominion.Engine.Entities;
using System;
using System.Collections.Generic;
using Diode_Dominion.DiodeDominion.Entities.Items;
using Diode_Dominion.DiodeDominion.Events;
using Diode_Dominion.Engine.Fonts;
using Diode_Dominion.Engine.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Diode_Dominion.DiodeDominion.Textures;

namespace Diode_Dominion.DiodeDominion.Entities.Colonists
{
	/// <summary>
	/// Default entity type that will populate the game world
	/// </summary>
	public class Colonist : Entity
	{
		#region Fields

		private const int DefaultStartLocation = 150;

		private const string DefaultName = "Unknown";
		private const int DefaultHealth = 40;
		private const int DefaultSkillValue = 0;
		private const MoodTypes DefaultMood = MoodTypes.HAPPY;
		public const double MAX_BATTERY_LIFE = 100;

		
		public List<Item> Inventory = new List<Item>();
		#endregion

		public SkillsAndLevels Skills { get; } = new SkillsAndLevels();

		public int PreviousTime { get; set; } = 1;

		public double TimeTillRecharge { get; set; }

		public MoodTypes Mood
		{
			get; set;
		}

		public List<MoodTypes> MoodsList { get; }

		public double MaxBatteryLife => MAX_BATTERY_LIFE;

		#region Constructors
		/// <summary>
		/// Used to create the colonist given the information from the colonist builder.  It also sets some default values that
		/// are the same for all colonists regardless of skillset.
		/// </summary>
		/// <param name="colonistBuilder">The builder that contains the skillset for the current colonist</param>
		public Colonist(ColonistBuilder colonistBuilder)
		{
			try
			{
				MoodTypes mood = DefaultMood;
				Mood = mood;
				TimeTillRecharge = MAX_BATTERY_LIFE;

				MoodsList = new List<MoodTypes> {mood};
				this.Name = colonistBuilder.Name ?? DefaultName;

				if (colonistBuilder.Health.Equals(0))
				{
					Health = DefaultHealth;
					MaxHealth = DefaultHealth;
				}
				else
				{
					Health = colonistBuilder.Health;
					MaxHealth = colonistBuilder.Health;
				}

				Skills.AddSkill(SkillTypes.BUILDING,
					colonistBuilder.Building.Equals(0) ? DefaultSkillValue : colonistBuilder.Building);

				Skills.AddSkill(SkillTypes.COOKING,
					colonistBuilder.Cooking.Equals(0) ? DefaultSkillValue : colonistBuilder.Cooking);

				Skills.AddSkill(SkillTypes.CRAFTING,
					colonistBuilder.Crafting.Equals(0) ? DefaultSkillValue : colonistBuilder.Crafting);

				Skills.AddSkill(SkillTypes.DOCTORING,
					colonistBuilder.Doctoring.Equals(0) ? DefaultSkillValue : colonistBuilder.Doctoring);

				Skills.AddSkill(SkillTypes.HARVESTING,
					colonistBuilder.Harvesting.Equals(0) ? DefaultSkillValue : colonistBuilder.Harvesting);

				Skills.AddSkill(SkillTypes.MELEE,
					colonistBuilder.Melee.Equals(0) ? DefaultSkillValue : colonistBuilder.Melee);

				Skills.AddSkill(SkillTypes.MINING,
					colonistBuilder.Mining.Equals(0) ? DefaultSkillValue : colonistBuilder.Mining);
			}
			catch (InvalidOperationException e)
			{
				Console.WriteLine("Something went wrong. Restart the application." + e);
			}
		}
        #endregion

        #region Methods

        /// <summary>
        /// Handle telling the AI to update the entity if there is
        /// anything that should occur
        /// </summary>
        public override void Update()
        {
			if (Alive)
			{
				BaseAi.Update();
				BaseAi.Update(new UpdateHungerEvent(this));
			}
        }

		public virtual void LoadContent(ContentManager content)
		{
			EntitySprite = new OverlaySprite(
				new Vector2(DefaultStartLocation, DefaultStartLocation), 
				Settings.Content.Load<Texture2D>(TextureLocalization.ColonistSprite));
		}

		/// <summary>
		/// This method takes care of the UpdateInventoryEvent
		/// </summary>
		/// <param name="updateInventoryEvent"> Event that occurred </param>
		public override void UpdateInventoryEvent(UpdateInventoryEvent updateInventoryEvent)
		{
			if (Inventory.Count < 3)
			{
				Inventory.Add(updateInventoryEvent.Item);
			}
		}
		
		/// <summary>
		/// Adds mood parameter to colonist's mood list
		/// </summary>
		/// <param name="moods"> mood type of colonist </param>
		public void AddMood(MoodTypes moods)
		{
			MoodsList.Add(moods);
		}

		/// <summary>
		/// Removes mood parameter to colonist's mood list
		/// </summary>
		/// <param name="moods"> mood type of colonist </param>
		public void RemoveMood(MoodTypes moods)
		{
			MoodsList.Remove(moods);
		}

		/// <summary>
		/// Draw the colonists name to the screen
		/// </summary>
		/// <param name="gameTime"></param>
		/// <param name="spriteBatch"></param>
		internal override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			base.Draw(gameTime, spriteBatch);

			// Draw the name of the colonist to the screen
			Rectangle mouseLocation = new Rectangle((int)Settings.MouseTransformLocation.X, (int)Settings.MouseTransformLocation.Y, 1, 1);
			if (mouseLocation.Intersects(this.EntitySprite.Bounds))
				spriteBatch.DrawString(Font.DefaultFont, Name, new Vector2(EntitySprite.Origin.X, EntitySprite.Origin.Y + EntitySprite.Height), Color.White);
		}

		#endregion
	}
}
