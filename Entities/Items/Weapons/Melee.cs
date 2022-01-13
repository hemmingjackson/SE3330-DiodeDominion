namespace Diode_Dominion.DiodeDominion.Entities.Items.Weapons
{
	public class Melee
	{
		#region Properties

		private double targetHealth;
		private double damage;
		private readonly Weapon weapon;

		#endregion

		#region Constructors

		public Melee(double targetHealth, Weapon weapon)
		{
			this.targetHealth = targetHealth;
			this.weapon = weapon;
		}

        #endregion

        #region Methods

		public double CalculateTargetEntityDamage()
		{
			damage = ((targetHealth * weapon.DamageMultiplier * weapon.AttackSpeedMultiplier) / (double)weapon.AttackRange);
			return damage;
		}

        #endregion
    }
}
