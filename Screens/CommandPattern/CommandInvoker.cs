using System.Collections.Generic;

namespace Diode_Dominion.DiodeDominion.Screens.CommandPattern
{
	/// <summary>
	/// Allows for the execution of commands.
	/// Holds the information on how to undo/redo.
	/// </summary>
	public class CommandInvoker
	{
		/// <summary>
		/// Handles holding commands that can be undone
		/// </summary>
		private readonly Stack<ICommand> undo;

		/// <summary>
		/// Handles holding commands that can be redone
		/// if they were undone
		/// </summary>
		private readonly Stack<ICommand> redo;

		public CommandInvoker()
		{
			undo = new Stack<ICommand>();
			redo = new Stack<ICommand>();
		}

		/// <summary>
		/// Add the command in a way that it can be undone later.
		/// Clears the redo stack
		/// </summary>
		/// 
		/// <param name="command"></param>
		public void AddUndoCommand(ICommand command)
		{
			undo.Push(command);
			redo.Clear();
		}

		/// <summary>
		/// Returns the last command that was saved.
		/// If there are no commands, it will return null.
		/// </summary>
		/// 
		/// <returns>Last command issued</returns>
		public void UndoLastCommand()
		{
			if (undo.Count > 0)
			{
				ICommand command = undo.Pop();
				redo.Push(command);

				command.Execute();
			}
		}

		/// <summary>
		/// Returns that last command that was undone
		/// </summary>
		/// 
		/// <returns></returns>
		public void RedoLastCommand()
		{
			if (redo.Count > 0)
			{
				ICommand command = redo.Pop();

				undo.Push(command);

				command.Execute();
			}
		}

		
	}
}
