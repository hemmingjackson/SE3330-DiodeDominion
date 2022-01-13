namespace Diode_Dominion.DiodeDominion.Screens.CommandPattern
{
	/// <summary>
	/// Commands that can be used for the command pattern to undo/redo
	/// actions.
	/// </summary>
	public interface ICommand
	{
		void Execute();
	}
}
