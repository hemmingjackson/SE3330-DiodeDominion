using Diode_Dominion.Engine.Events;

namespace Diode_Dominion.DiodeDominion.Events
{
	/// <summary>
	/// This event is used to tell the entity that it
	/// should stop all actions it is executing.
	/// </summary>
	public class StopActionsEvent : BaseEvent
	{
		/// <summary>
		/// Holds the Id value of the entity
		/// </summary>
		public int EntityId { get; }

		/// <summary>
		/// Create an event that notifies an entity that it should stop all actionable
		/// events
		/// </summary>
		public StopActionsEvent(int entityId) : base()
		{
			EntityId = entityId;
		}
	}
}
