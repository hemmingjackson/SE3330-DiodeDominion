using Diode_Dominion.DiodeDominion.Entities.Animals;
using Diode_Dominion.DiodeDominion.Events;
using Diode_Dominion.DiodeDominion.World.Tiles;

namespace Diode_Dominion.DiodeDominion.Entities.AI.SpecificAIs
{
	public class AnimalAI : BaseAi
	{
		public AnimalAI(Animal entity, MapTile[,] tiles) : base(entity, tiles) { }

		/// <summary>
		/// Animals should not be able to till ground
		/// </summary>
		/// <param name="tileTill"></param>
		public override void TillGroundEvent(TileTillEvent tileTill) { }

		/// <summary>
		/// Animals should not be able to plant crops
		/// </summary>
		/// <param name="plantCrop"></param>
		public override void PlantCropEvent(PlantCropEvent plantCrop) { }

		/// <summary>
		/// Animals should not be able to deposit items
		/// </summary>
		/// <param name="depositItem"></param>
		public override void DepositItemEvent(DepositItemInStockpileEvent depositItem) { }

		/// <summary>
		/// Animals should not be able to pickup items
		/// </summary>
		/// <param name="pickUpItem"></param>
		public override void PickUpItemEvent(PickUpItemEvent pickUpItem) { }

		/// <summary>
		/// Animals should not be able to drop items
		/// </summary>
		/// <param name="dropItem"></param>
		public override void DropItemEvent(DropItemEvent dropItem) { }

		/// <summary>
		/// Animals should not be able to harvest static entities
		/// </summary>
		/// <param name="harvestStaticEntity"></param>
		public override void HarvestStaticEntityEvent(HarvestStaticEntityEvent harvestStaticEntity) { }

		/// <summary>
		/// Animals should not be able grow
		/// </summary>
		/// <param name="cropGrowthEvent"></param>
		public override void CropGrowthEvent(CropGrowthEvent cropGrowthEvent) { }
	}
}
