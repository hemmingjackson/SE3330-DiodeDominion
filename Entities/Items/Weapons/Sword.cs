namespace Diode_Dominion.DiodeDominion.Entities.Items.Weapons
{
	/// <summary>
	/// A specific type of weapon that can be used to interact with different
	/// entities.
	/// </summary>
	public class Sword : Weapon
    {
		#region Constructors

		/// <summary>
		/// Creates sword and sets its values
		/// </summary>
		public Sword() : base(WeaponType.SWORD)
		{
			this.DamageMultiplier = 0.1;
			this.AttackSpeedMultiplier = 2;
			this.AttackRange = AttackRange.SHORT;
			this.WeaponQualityDrain = 5;
		}

        #endregion
    }
}
