using Diode_Dominion.DiodeDominion.Entities;
using Diode_Dominion.DiodeDominion.Entities.StaticEntities;
using Diode_Dominion.Engine.Events;

namespace Diode_Dominion.DiodeDominion.Events
{
	/// <summary>
	/// Passes the objects needed to start the harvesting
	/// of a static entity
	/// </summary>
	public class HarvestStaticEntityEvent : BaseEvent
	{
		/// <summary>
		/// Entity that is harvesting the static entity
		/// </summary>
		public int EntityId { get; }

		/// <summary>
		/// Static entity that is being harvested
		/// </summary>
		public StaticEntity StaticEntity { get; }

		/// <summary>
		/// Creates the event that holds a reference to the <see cref="Entity"/>
		/// that will be harvesting the <see cref="StaticEntity"/>
		/// </summary>
		/// 
		/// <param name="entityId">Entity that is harvesting the static entity</param>
		/// <param name="staticEntity">Static Entity that is being harvested</param>
		public HarvestStaticEntityEvent(int entityId, StaticEntity staticEntity)
		{
			EntityId = entityId;
			StaticEntity = staticEntity;
		}
	}
}