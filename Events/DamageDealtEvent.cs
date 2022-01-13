using Diode_Dominion.DiodeDominion.Entities;
using Diode_Dominion.DiodeDominion.Entities.Items.Weapons;
using Diode_Dominion.Engine.Events;

namespace Diode_Dominion.DiodeDominion.Events
{
    public class DamageDealtEvent : BaseEvent
    {
        #region Properties

        public Entity Attacker { get; }
        public Entity Target { get; }
        public Weapon Weapon { get; }
        public double Damage { get; set; }

        public float Time { get; set; }

        #endregion

        #region Constructors
        public DamageDealtEvent(Entity attacker, Entity target, double damage = 0)
        {
            Attacker = attacker;
            Target = target;
            Damage = damage;
        }

        public DamageDealtEvent(Entity attacker, Entity target, Weapon weapon, double damage = 0)
        {
            Attacker = attacker;
            Target = target;
            Weapon = weapon;
            Damage = damage;
        }

        #endregion
    }
}