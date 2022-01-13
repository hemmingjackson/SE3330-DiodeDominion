using Diode_Dominion.DiodeDominion.Textures;
using Diode_Dominion.Engine.Sprites;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Diode_Dominion.DiodeDominion.Entities.Items.Weapons
{
	/// <summary>
	/// A type of item that can be used to interact with different
	/// entities.
	/// </summary>
	public class Weapon : Item
	{
		private const int DefaultHealth = 50;

		#region Properties

		/// <summary>
		/// Type of weapon the item is
		/// </summary>
		public WeaponType WeaponType { get; protected set; }

		/// <summary>
		/// Amount quality of the weapon is drained by.
		/// The lower the value, the lower amount
		/// of health that is drained.
		/// </summary>
		public double WeaponQualityDrain { get; set; }

		/// <summary>
		/// The multiplier of the damage for specific weapon
		/// </summary>
		public double DamageMultiplier { get; set; }

		/// <summary>
		/// The speed at which attack occurs, using specific weapon.
		/// This value will be multiplied with the target entity's health and
		/// the weapon's damage multiplier.
		/// </summary>
		public double AttackSpeedMultiplier { get; set; }

		/// <summary>
		/// The range from which colonist can attack, using specific weapon
		/// </summary>
		public AttackRange AttackRange { get; protected set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Creates weapon using weapon type
		/// </summary>
		/// <param name="weapon"></param>
		public Weapon(WeaponType weapon)
		{
			Name = weapon.ToString();
			Health = DefaultHealth;
			WeaponType = weapon;
			EntitySprite = new Sprite(Coordinates);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Method to decrease the weapon health each time it is used
		/// </summary>
		public void DecreaseWeaponHealth()
		{
			// each round used (i.e., each time CalculateTargetEntityDamage called or in attack event), decrease health by...
			Health -= WeaponQualityDrain;
		}

		/// <summary>
		/// Loads correct sprite for weapon
		/// </summary>
		/// <param name="content">Content manager that can import textures</param>
		public void LoadContent(ContentManager content)
		{
			EntitySprite = new Sprite(EntitySprite.Origin,
				content.Load<Texture2D>(TextureLocalization.Weapons +
				Name[0] + Name.Substring(1).ToLowerInvariant()));
		}

		#endregion
	}
}
