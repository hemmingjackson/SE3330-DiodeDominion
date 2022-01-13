namespace Diode_Dominion.DiodeDominion.Entities.Items.Weapons
{
	/// <summary>
	/// A specific type of weapon that can be used to interact with different
	/// entities.
	/// </summary>
	public class LaserHandgun : Weapon
	{
		#region Constructors

		/// <summary>
		/// Creates laser handgun and sets its values
		/// </summary>
		public LaserHandgun() : base(WeaponType.LASER_HANDGUN)
		{
			this.DamageMultiplier = 0.3;
			this.AttackSpeedMultiplier = 3;
			this.AttackRange = AttackRange.LONG;
			this.WeaponQualityDrain = 1;
		}

        #endregion
    }
}
