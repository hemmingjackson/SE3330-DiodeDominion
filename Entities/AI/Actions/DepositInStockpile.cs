using Diode_Dominion.DiodeDominion.Entities.Items;
using Diode_Dominion.DiodeDominion.Entities.StaticEntities.Stockpiles;

namespace Diode_Dominion.DiodeDominion.Entities.AI.Actions
{
	/// <summary>
	/// Type of action to deposit an item into a stockpile
	/// </summary>
	public class DepositInStockpile : IAction
	{
		private readonly Item item;
		private readonly ConglomerateStockpile stockpile;

		public DepositInStockpile(Item item, ConglomerateStockpile stockpile)
		{
			this.item = item;
			this.stockpile = stockpile;
		}

		/// <summary>
		/// Add the item to the stockpile
		/// </summary>
		/// <returns></returns>
		public bool Update()
		{
			stockpile.AddItem(item);
			return true;
		}
	}
}
