using Diode_Dominion.DiodeDominion.Entities.Items;
using Diode_Dominion.Engine.Events;

namespace Diode_Dominion.DiodeDominion.Events
{
	/// <summary>
	/// Handle passing the item that should be dropped
	/// </summary>
	public class DropItemEvent : BaseEvent
	{
		/// <summary>
		/// Entity that is holding the item
		/// </summary>
		public int EntityId { get; }

		/// <summary>
		/// Item that must be dropped
		/// </summary>
		public Item Item { get; }

		/// <summary>
		/// Hold the entity that will be dropping the item
		/// </summary>
		/// 
		/// <param name="entityId">Entity that has the item</param>
		/// <param name="item">Item that is to be dropped</param>
		public DropItemEvent(int entityId, Item item)
		{
			EntityId = entityId;
			Item = item;
		}
	}
}
