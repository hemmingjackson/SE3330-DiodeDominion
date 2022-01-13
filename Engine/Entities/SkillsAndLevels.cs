using System.Collections.Generic;

namespace Diode_Dominion.Engine.Entities
{
	/// <summary>
	/// This class deals with creating a mapping of a colonist's skills and their
	/// corresponding levels as well as the logic for adding skills/levels and
	/// increasing/decreasing skill levels.
	/// </summary>
	public class SkillsAndLevels
	{
        #region Properties

        public Dictionary<SkillTypes, int> SkillAndLevel { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates the dictionary for the mapping of skill and the skill's level
        /// </summary>
        public SkillsAndLevels()
		{
			SkillAndLevel = new Dictionary<SkillTypes, int>();
		}

        #endregion

        #region Methods

        /// <summary>
        /// This method takes in a skillType and an amount and increases the skill by that amount.
        /// </summary>
        /// <param name="currentSkill"></param>
        /// <param name="increaseAmount"> amount skill is being increased by </param>
        public void IncreaseSkill(SkillTypes currentSkill, int increaseAmount)
		{
			int currentLevel = SkillAndLevel[currentSkill]; //retrieves current level for given skill
			currentLevel += increaseAmount;
			SkillAndLevel[currentSkill] = currentLevel;
		}

		/// <summary>
		/// This method takes in a skillType and an amount and decreases the skill by that amount.
		/// </summary>
		/// <param name="currentSkill"> skill that is being decreased </param>
		/// <param name="decreaseAmount"> amount skill is being decreased by </param>
		public void DecreaseSkill(SkillTypes currentSkill, int decreaseAmount)
		{
			int currentLevel = SkillAndLevel[currentSkill]; //retrieves current level for given skill
			currentLevel -= decreaseAmount;
			SkillAndLevel[currentSkill] = currentLevel;
		}

		/// <summary>
		/// This method takes in a skill and amount and adds both to skillAndLevel dictionary
		/// </summary>
		/// <param name="skillType"> skill that is being added </param>
		/// <param name="value"> level of skill being added </param>
		public void AddSkill(SkillTypes skillType, int value)
        {
			SkillAndLevel.Add(skillType, value);
		}

        #endregion
    }
}