namespace Diode_Dominion.DiodeDominion.Entities.Items.Weapons
{
	/// <summary>
	/// A specific type of weapon that can be used to interact with different
	/// entities.
	/// </summary>
	public class Trident : Weapon
	{
		#region Constructors

		/// <summary>
		/// Creates trident and sets its values
		/// </summary>
		public Trident() : base(WeaponType.TRIDENT)
		{
			this.DamageMultiplier = 0.2;
			this.AttackSpeedMultiplier = 1;
			this.AttackRange = AttackRange.MEDIUM;
			this.WeaponQualityDrain = 3;
		}

		#endregion
	}
}
