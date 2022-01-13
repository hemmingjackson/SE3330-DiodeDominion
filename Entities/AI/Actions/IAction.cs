namespace Diode_Dominion.DiodeDominion.Entities.AI.Actions
{
	/// <summary>
	/// Describes the common functionality that all AI actions
	/// should inherit
	/// </summary>
	internal interface IAction
	{
		/// <summary>
		/// Updates the action. Will return true once the action is complete
		/// </summary>
		/// 
		/// <returns>Whether the action is complete</returns>
		bool Update();
	}
}
