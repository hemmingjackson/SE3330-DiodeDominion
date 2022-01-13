namespace Diode_Dominion.Engine.Entities
{
	/// <summary>
	/// This class is specific for one skill and holds the level for that skill
	/// as well as the logic for increasing/decreasing those levels.
	/// </summary>
	public class Skill
	{
		#region Properties

		public int Level { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for Skill class
        /// </summary>
        /// <param name="level"> skill level </param>
        public Skill(int level)
		{
			Level = level;
		}

        #endregion

        #region Methods

        /// <summary>
        /// This method takes in an amount and increases the skill by that amount.
        /// </summary>
        /// <param name="increaseAmount"> amount skill is being increased by </param>
        public void IncreaseSkillLevel(int increaseAmount)
		{
			Level += increaseAmount;
		}

		/// <summary>
		/// This method takes in an amount and decreases the skill by that amount.
		/// </summary>
		/// <param name="decreaseAmount"> amount skill is being decreased by </param>
		public void DecreaseSkillLevel(int decreaseAmount)
		{
			Level -= decreaseAmount;
		}

        #endregion
    }
}