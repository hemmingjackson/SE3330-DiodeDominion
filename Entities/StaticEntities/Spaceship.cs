namespace Diode_Dominion.DiodeDominion.Entities.StaticEntities
{
    public class Spaceship : StaticEntity
    {
        #region Constructors

        /// <summary>
        /// Creates the spaceship and sets values
        /// </summary>
        public Spaceship() : base(StaticEntityType.SPACESHIP)
        {
            MaxHealth = 100; // "health" in this sense would probably be the bio-fuel level
            Health = 100;
        }

        #endregion
    }
}
