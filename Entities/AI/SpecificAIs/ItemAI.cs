
using Diode_Dominion.DiodeDominion.Entities.Items;
using Diode_Dominion.DiodeDominion.Events;
using Diode_Dominion.DiodeDominion.World.Tiles;

namespace Diode_Dominion.DiodeDominion.Entities.AI.SpecificAIs
{
	/// <summary>
	/// Used to add/remove processing functionality for an item.
	/// Some functionality from the base AI is removed to stop it
	/// from behaving in unexpected ways.
	/// </summary>
	public class ItemAI : BaseAi
	{
		/// <summary>
		/// Create an AI object to with limited functionality from the base AI
		/// </summary>
		/// 
		/// <param name="item">Item that possesses the AI</param>
		/// <param name="tiles"></param>
		public ItemAI(Item item, MapTile[,] tiles) : base(item, tiles)
		{

		}

		/// <summary>
		/// Items should not be able to till ground
		/// </summary>
		/// <param name="tileTill"></param>
		public override void TillGroundEvent(TileTillEvent tileTill) { }

		/// <summary>
		/// Items should not be able to plant crops
		/// </summary>
		/// <param name="plantCrop"></param>
		public override void PlantCropEvent(PlantCropEvent plantCrop) { }

		/// <summary>
		/// Items should not be able to deposit items
		/// </summary>
		/// <param name="depositItem"></param>
		public override void DepositItemEvent(DepositItemInStockpileEvent depositItem) { }

		/// <summary>
		/// Items should not be able to pickup items
		/// </summary>
		/// <param name="pickUpItem"></param>
		public override void PickUpItemEvent(PickUpItemEvent pickUpItem) { }

		/// <summary>
		/// Items should not be able to drop items
		/// </summary>
		/// <param name="dropItem"></param>
		public override void DropItemEvent(DropItemEvent dropItem) { }

		/// <summary>
		/// Items should not be able to harvest static entities
		/// </summary>
		/// <param name="harvestStaticEntity"></param>
		public override void HarvestStaticEntityEvent(HarvestStaticEntityEvent harvestStaticEntity) { }

		/// <summary>
		/// Items should not be able grow
		/// </summary>
		/// <param name="cropGrowthEvent"></param>
		public override void CropGrowthEvent(CropGrowthEvent cropGrowthEvent) { }
	}
}
