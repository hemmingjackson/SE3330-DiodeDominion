using Diode_Dominion.Engine.Events;

namespace Diode_Dominion.DiodeDominion.Events
{
	/// <summary>
	/// Event that occurs when the entity should heal
	/// </summary>
	public class HealEvent : BaseEvent
	{
		/// <summary>
		/// Target that is being healed
		/// </summary>
		public int TargetId { get; }

		/// <summary>
		/// Amount that the target is healing
		/// </summary>
		public double Heal { get; }

		/// <summary>
		/// Creates a heal event that causes the entity to heal
		/// </summary>
		/// 
		/// <param name="targetId">Target id that is being healed</param>
		/// <param name="healAmount"></param>
		public HealEvent(int targetId, double healAmount)
		{
			TargetId = targetId;
			Heal = healAmount;
		}
	}
}
