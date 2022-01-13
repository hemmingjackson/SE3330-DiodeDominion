using Diode_Dominion.DiodeDominion.Entities.StaticEntities.Crops;
using Diode_Dominion.DiodeDominion.Textures;
using Diode_Dominion.Engine.GameTimer;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Diode_Dominion.DiodeDominion.Entities.AI.Actions
{
	/// <summary>
	/// Handles the growth of crops
	/// </summary>
	public class GrowCrop : IAction
	{
		#region Fields

		// Determines growth amount to progress to each stage
		public const double GROWTH_INCREMENT = 10;
		public const double GROWTH_DECREMENT = 50;
		public const double HEALTH_INCREMENT = 25;
		public const double STAGE_2_GROWTH = 25;
		public const double STAGE_3_GROWTH = 50;
		public const double STAGE_4_GROWTH = 75;
		public const double FULL_GROWTH = 100;

		#endregion

		#region Properties
		public Crop Crop { get; }

		#endregion

		#region Constructors

		public GrowCrop(Crop crop)
		{
			this.Crop = crop;
		}

		#endregion

		#region Methods
		/// <summary>
		/// Compares crop time with the current time to progress crop growth.
		/// Will update sprite texture and growth status over time
		/// </summary>
		/// <returns></returns>
		public bool Update()
		{
			if (Crop.IsPlanted)
			{
				if (Math.Abs(Crop.CropTime - GameTimer.Time) / GROWTH_INCREMENT > Crop.PreviousTime)
				{
					if (Crop.TimeTilGrowth != 0)
					{
						Crop.TimeTilGrowth -= GROWTH_DECREMENT;
						Crop.PreviousTime++;
					}
					else
					{
						Crop.TimeTilGrowth = 0;
						Crop.Health += HEALTH_INCREMENT;
						Crop.PreviousTime++;
						if (Crop.Health > STAGE_2_GROWTH)
						{
							Crop.Growth = GrowthType.STAGE2;
							Crop.EntitySprite.Texture = Settings.Content.Load<Texture2D>(TextureLocalization.CropStage2);
						}
						if (Crop.Health > STAGE_3_GROWTH)
						{
							Crop.Growth = GrowthType.STAGE3;
							Crop.EntitySprite.Texture = Settings.Content.Load<Texture2D>(TextureLocalization.CropStage3);
						}
						if (Crop.Health > STAGE_4_GROWTH)
						{
							Crop.Growth = GrowthType.STAGE4;
							Crop.EntitySprite.Texture = Settings.Content.Load<Texture2D>(TextureLocalization.CropStage4);
						}
						if (Crop.Health >= FULL_GROWTH)
						{
							Crop.Growth = GrowthType.GROWN;
							Crop.EntitySprite.Texture = Settings.Content.Load<Texture2D>(TextureLocalization.CropGrown);
							Crop.Health = FULL_GROWTH;
							Crop.Grown = true;
						}
					}
				}
			}
			return true;
		}

		#endregion
	}
}
