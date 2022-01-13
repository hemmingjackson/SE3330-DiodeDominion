using Diode_Dominion.DiodeDominion.Entities.Items.Tools;
using Diode_Dominion.DiodeDominion.Events;
using Diode_Dominion.Engine.GameTimer;

namespace Diode_Dominion.DiodeDominion.Entities.StaticEntities.Crops
{
	/// <summary>
	/// Crop class of type static entity
	/// </summary>
	public class Crop : StaticEntity
	{
		#region Fields

		// Crop Name and type are established
		public CropType CropType;

		// Deals with crop growth and growth type
		private const int DefaultGrowth = 1;
		public const double MAX_GROWTH = 100;
		private const GrowthType DefaultGrowthType = GrowthType.STAGE1;
		
		public int PreviousTime { get; set; } = 1;

		public double TimeTilGrowth { get; set; }

		// Uses game time to determine when crops are planted
		public float CropTime { get; set; }

		// Whether or not a crop is planted
		public bool IsPlanted { get; set; }

		// Growth status of crop
		public GrowthType Growth
		{
			get; set;
		}

		// Whether or not a crop is fully grown
		public bool Grown
		{
			get; set;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Establishes all initial values for crop
		/// </summary>
		/// <param name="cropType"></param>
		public Crop(CropType cropType) : base(StaticEntityType.CROP, ToolType.HOE)
      {
	      IsPlanted = true;
			CropTime = GameTimer.Time;
			Grown = false;
			CropType = cropType;
			Growth = DefaultGrowthType;
			Name = cropType.ToString();
			Health = DefaultGrowth;
			MaxHealth = MAX_GROWTH;
			TimeTilGrowth = MAX_GROWTH;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Handle telling the AI to update the entity if there is
		/// anything that should occur
		/// </summary>
		public override void Update()
		{
			if (!Grown)
			{
				BaseAi.Update();
				BaseAi.Update(new CropGrowthEvent(this));
			}
		}

		#endregion
	}
}
