using System;

namespace Diode_Dominion.DiodeDominion.Entities.AI.Actions
{
	/// <summary>
	/// Handle the functionality that should be included when the health of
	/// an <see cref="Entity"/> should be updated.
	/// </summary>
	public class UpdateHealth : IAction
	{
		private readonly Entity entity;

		/// <summary>
		/// Creates an update health object that can be used to handle the updating of
		/// an entity's health
		/// </summary>
		/// 
		/// <param name="entity">Entity that will be updated</param>
		public UpdateHealth(Entity entity)
		{
			this.entity = entity;
		}

		/// <summary>
		/// Decrements the health of the entity by the passed in value
		/// </summary>
		/// 
		/// <param name="damage">Damage that was dealt</param>
		public void DecrementHealth(double damage)
		{
			// Normalize the damage to prevent invalid values that would be very hard to track down
			entity.Health -= Math.Abs(damage);
			
			// Do not allow for health values less than 0
			if (entity.Health < 0d)
			{
				entity.Health = 0d;
			}
		}

		/// <summary>
		/// Increases the health by the passed in amount
		/// </summary>
		/// 
		/// <param name="increaseAmount">Amount to increase the health by</param>
		public void IncrementHealth(double increaseAmount)
		{
			// Add the normalized health
			entity.Health += Math.Abs(increaseAmount);

			// Do not allow for health values above the max
			if (entity.Health > entity.MaxHealth)
			{
				entity.Health = entity.MaxHealth;
			}
		}

		/// <summary>
		/// Action executes immediately for an update health event
		/// </summary>
		/// <returns></returns>
		public bool Update()
		{
			return true;
		}
	}
}
