using Diode_Dominion.Engine.Events;

namespace Diode_Dominion.DiodeDominion.Events
{
	/// <summary>
	/// Create an event when there is an entity that must be moved
	/// </summary>
	public class MoveEntityEvent : BaseEvent
	{
		/// <summary>
		/// Holds a reference to the id of the entity that needs to move
		/// </summary>
		public int EntityId { get; }

		/// <summary>
		/// X location that the entity needs to move towards
		/// </summary>
		public float XLocation { get; }

		/// <summary>
		/// Y location that the entity needs to move towards
		/// </summary>
		public float YLocation { get; }

		/// <summary>
		/// Create an event object that dictates where the entity will need to move
		/// </summary>
		public MoveEntityEvent(int entityId, float xCoordinate, float yCoordinate)
		{
			EntityId = entityId;
			XLocation = xCoordinate;
			YLocation = yCoordinate;
		}
	}
}
