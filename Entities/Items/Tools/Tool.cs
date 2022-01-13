namespace Diode_Dominion.DiodeDominion.Entities.Items.Tools
{
	/// <summary>
	/// A specific type of item that can be used to interact with different
	/// static entities and world objects.
	/// </summary>
	public class Tool : Item
	{

		#region Properties

		/// <summary>
		/// Type of tool that this tool is
		/// </summary>
		public ToolType ToolType { get; }

		/// <summary>
		/// Quality of the tool. Increasing the quality of the tool
		/// should allow it to perform actions faster
		/// </summary>
		public float ToolQuality { get; set; } = 1.0f;

		#endregion

		#region Constructors

		public Tool(ToolType tool) : base(tool.ToString(), 50, null)
		{
			// Texture pulled out of constructor
			// Settings.Content.Load<Texture2D>(TextureLocalization.Tools + tool)

			ToolType = tool;
			IsTool = true;
		}

		#endregion
	}
}
