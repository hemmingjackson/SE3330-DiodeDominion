using System.Collections.Generic;
using Diode_Dominion.DiodeDominion.Entities;

namespace Diode_Dominion.DiodeDominion.World.Generators
{
	/// <summary>
	/// Defines common ways that the world will generate different aspects 
	/// </summary>
	public interface IGenerate
	{
		/// <summary>
		/// Cause the generation of some type of world generated component.
		/// </summary>
		/// 
		/// <returns>Enumerable of the generated entity</returns>
		IEnumerable<Entity> Generate();
	}
}
