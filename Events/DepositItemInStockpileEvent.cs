using Diode_Dominion.DiodeDominion.Entities.Items;
using Diode_Dominion.DiodeDominion.Entities.StaticEntities.Stockpiles;
using Diode_Dominion.Engine.Events;

namespace Diode_Dominion.DiodeDominion.Events
{
	/// <summary>
	/// Event used to deposit an item in a stockpile
	/// </summary>
	public class DepositItemInStockpileEvent : BaseEvent
	{
		public int EntityId { get; }

		public Item Item { get; }

		public ConglomerateStockpile Stockpile { get; }

		public DepositItemInStockpileEvent(int entityId, Item item, ConglomerateStockpile stockpile)
		{
			EntityId = entityId;
			Item = item;
			Stockpile = stockpile;
		}
	}
}
