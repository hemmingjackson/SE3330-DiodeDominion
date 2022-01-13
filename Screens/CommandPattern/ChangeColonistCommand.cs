namespace Diode_Dominion.DiodeDominion.Screens.CommandPattern
{
	/// <summary>
	/// Class for changing which colonist is currently selected
	/// </summary>
	class ChangeColonistCommand : ICommand
	{
		/// <summary>
		/// Reference to the screen
		/// </summary>
		private readonly ColonistCreationScreen screen;
		/// <summary>
		/// Number representing the what the current colonist is about
		/// </summary>
		private readonly int currentColonist;
		/// <summary>
		/// Number representing the what the next colonist is
		/// </summary>
		private readonly int nextColonist;
		/// <summary>
		/// Determines if the command should undo or redo
		/// </summary>
		private bool shouldUndo;
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="screen"> Colonist creation screen that needs to be updated</param>
		/// <param name="currentColonist">The colonist that undoing should revert to</param>
		/// <param name="nextColonist">The colonist that the colonist undoes from/redo to</param>
		public ChangeColonistCommand(ColonistCreationScreen screen, int currentColonist, int nextColonist)
		{
			shouldUndo = true;
			this.screen = screen;
			this.currentColonist = currentColonist;
			this.nextColonist = nextColonist;
		}
		/// <summary>
		/// Execute function that sets the currentColonist accordingly
		/// </summary>
		public void Execute()
		{
			screen.CurrentColonist = shouldUndo ? currentColonist : nextColonist;
			shouldUndo = !shouldUndo;
		}
	}
}
