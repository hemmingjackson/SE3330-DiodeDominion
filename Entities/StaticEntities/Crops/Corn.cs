namespace Diode_Dominion.DiodeDominion.Entities.StaticEntities.Crops
{
   /// <summary>
   /// Corn class of type crop
   /// </summary>
	public class Corn : Crop
	{
      #region Constructors

      public Corn(CropType cropType) : base(CropType.CORN)
      {
         CropType = cropType;
         Name = cropType.ToString();
      }

		#endregion
	}
}
