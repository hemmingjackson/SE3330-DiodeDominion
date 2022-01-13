using Diode_Dominion.DiodeDominion.Entities.Colonists;

namespace Diode_Dominion.DiodeDominion.Screens.CommandPattern
{
	class ColonistRandomizeCommand : ICommand
	{
		/// <summary>
		/// Reference to the colonist that is being randomized
		/// </summary>
		private readonly ColonistStatRandomizer colonistStats;
		/// <summary>
		/// ColonistRandomizer that stores the previous stats
		/// </summary>
		private readonly ColonistStatRandomizer previousStats;
		/// <summary>
		/// ColonistRandomizer that stores the next stats
		/// </summary>
		private ColonistStatRandomizer nextStats;
		/// <summary>
		/// Determines if execute should undo/redo
		/// </summary>
		private bool shouldUndo;
		public ColonistRandomizeCommand(ColonistStatRandomizer colonist)
		{
			shouldUndo = true;
			colonistStats = colonist;
			previousStats = new ColonistStatRandomizer(colonistStats);
		}
		/// <summary>
		/// Sets the colonist to the value that it was previously
		/// </summary>
		public void Execute()
		{
			if(shouldUndo)
			{
				nextStats = new ColonistStatRandomizer(colonistStats);
				colonistStats.SetStats(previousStats);
			
				
			}
			else
			{
				colonistStats.SetStats(nextStats);
			}
			shouldUndo = !shouldUndo;
		}
	}
}
