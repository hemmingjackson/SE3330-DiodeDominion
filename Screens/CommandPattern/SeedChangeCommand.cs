using System;

namespace Diode_Dominion.DiodeDominion.Screens.CommandPattern
{
	public class SeedChangeCommand : ICommand
	{
		/// <summary>
		/// Instance of the world creation screen to execute the
		/// command on
		/// </summary>
		private readonly WorldCreationScreen worldCreationScreen;

		/// <summary>
		/// Previous seed for the game world
		/// </summary>
		private readonly int previousSeed;

		/// <summary>
		/// Next seed that the game world used
		/// </summary>
		private readonly int nextSeed;

		/// <summary>
		/// Whether the command should be undone.
		/// true -> command undone
		/// false -> command redone
		/// </summary>
		private bool shouldUndo = true;

		/// <summary>
		/// Creates the command to undo or redo the seed that is saved
		/// </summary>
		/// 
		/// <param name="creationScreen">Instance of the screen</param>
		/// <param name="previousSeed">Previous seed being used in the game</param>
		/// <param name="nextSeed">Current seed being used in the game</param>
		public SeedChangeCommand(WorldCreationScreen creationScreen, int previousSeed, int nextSeed)
		{
			worldCreationScreen = creationScreen;
			this.previousSeed = previousSeed;
			this.nextSeed = nextSeed;
		}

		/// <summary>
		/// Handles changing the seed to the value it previously held
		/// </summary>
		public void Execute()
		{
			int seedToSet = previousSeed;

			if (!shouldUndo)
			{
				seedToSet = nextSeed;
			}

			// Invert whether the command should undo/redo
			shouldUndo = !shouldUndo;
			
			// Update the display seed
			worldCreationScreen.SetSeed(Convert.ToInt32(seedToSet));
		}
	}
}
