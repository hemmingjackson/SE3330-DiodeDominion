using Diode_Dominion.DiodeDominion.Entities.Colonists;
using Diode_Dominion.Engine.Entities;

namespace Diode_Dominion.DiodeDominion.Screens.CommandPattern
{
	internal class SkillIncrementCommand : ICommand
	{
		private int skillDelta;
		/// <summary>
		/// Type of skill to increment
		/// </summary>
		private readonly SkillTypes skillToIncrement;
		/// <summary>
		/// 
		/// </summary>
		private readonly ColonistStatRandomizer colonistStats;
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="skillToIncrement">Skill that is incremented</param>
		/// <param name="colonist">The colonist randomizer that will have the skill incremented</param>
		public SkillIncrementCommand(SkillTypes skillToIncrement, ColonistStatRandomizer colonist)
		{
			this.colonistStats = colonist;
			this.skillToIncrement = skillToIncrement;
			skillDelta = -1;
		}
		/// <summary>
		/// Increments the skill
		/// </summary>
		public void Execute()
		{
			colonistStats.ListOfSkills[(int)skillToIncrement] += skillDelta;
			colonistStats.ColonistPointsLeft -= skillDelta;
			skillDelta *= -1;
		}
	}
}
