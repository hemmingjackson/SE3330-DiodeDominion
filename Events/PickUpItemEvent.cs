using Diode_Dominion.DiodeDominion.Entities.Items;
using Diode_Dominion.Engine.Events;

namespace Diode_Dominion.DiodeDominion.Events
{
	/// <summary>
	/// Used to tell an entity that it needs to go and pickup an item
	/// </summary>
	public class PickUpItemEvent : BaseEvent
	{
		/// <summary>
		/// Id of the entity on it's way to pick up an item
		/// </summary>
		public int EntityId { get; }

		/// <summary>
		/// Item that is going to be picked up
		/// </summary>
		public Item Item { get; }

		/// <summary>
		/// Creates an object that will hold information on the item that needs to be
		/// picked up
		/// </summary>
		/// 
		/// <param name="entityId">Id of the entity picking up the item</param>
		/// <param name="item">Item that will need to be picked up</param>
		public PickUpItemEvent(int entityId, Item item)
		{
			EntityId = entityId;
			Item = item;
		}
	}
}
