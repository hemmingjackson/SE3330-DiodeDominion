using Diode_Dominion.DiodeDominion.Entities.AI.Actions;
using Diode_Dominion.DiodeDominion.Entities.StaticEntities.Crops;
using Diode_Dominion.Engine.Events;

namespace Diode_Dominion.DiodeDominion.Events
{
	/// <summary>
	/// Crop growth event that calls the grow crop action
	/// </summary>
	public class CropGrowthEvent : BaseEvent
	{
		#region Fields

		public const double GROWTH_INCREMENT = 10;
		public const double MAX_GROWTH = 100;

		#endregion

		#region Properties
		
		public Crop Crop { get; }

		#endregion

		#region Constructors

		public CropGrowthEvent(Crop crop)
		{
			Crop = crop;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Calls grow crop action
		/// </summary>
		public void UpdateCropGrowth()
		{
			GrowCrop growCrop = new GrowCrop(Crop);
			growCrop.Update();
		}

		#endregion
	}
}
