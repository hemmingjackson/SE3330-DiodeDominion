using Diode_Dominion.DiodeDominion.Entities.Colonists;
using Diode_Dominion.Engine.Entities;

namespace Diode_Dominion.DiodeDominion.Screens.CommandPattern
{
	class SkillDecrementCommand : ICommand
	{
		private int skillDelta;
		/// <summary>
	   /// Type of skill to increment
	   /// </summary>
		private readonly SkillTypes skillToDecrement;
		/// <summary>
		/// 
		/// </summary>
		private readonly ColonistStatRandomizer colonistStats;
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="skillToIncrement">Skill that will be decremented</param>
		/// <param name="colonist">colonist that will have their skill decremented</param>
		public SkillDecrementCommand(SkillTypes skillToIncrement, ColonistStatRandomizer colonist)
		{
			skillDelta = 1;
			this.colonistStats = colonist;
			this.skillToDecrement = skillToIncrement;
		}
		/// <summary>
		/// Decrements the skill
		/// </summary>
		public void Execute()
		{
			colonistStats.ListOfSkills[(int)skillToDecrement] += skillDelta;
			colonistStats.ColonistPointsLeft -= skillDelta;
			skillDelta *= -1;
		}
	}
}
