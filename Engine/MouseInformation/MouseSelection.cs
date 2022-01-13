using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Diode_Dominion.Engine.MouseInformation
{
	/// <summary>
	/// Used to determine and hold the position of the area selected by the mouse
	/// </summary>
	public class MouseSelection
	{
		#region Fields
		/// <summary>
		/// Length of side menu 
		/// </summary>
		private const int MenuSize = 125;
      /// <summary>
      /// Bool of if the mouse has been double clicked or not
      /// </summary>
      private static bool _doubleClicked;
      /// <summary>
      /// Time of when the mouse was last clicked
      /// </summary>
      private static DateTime _lastClicked;
      /// <summary>
      /// Max delay for when a mouse has been double clicked
      /// </summary>
      private const float MaxDelay = 0.5f;
      /// <summary>
      /// Min delay for when a mouse has been double clicked
      /// </summary>
      private const float MinDelay = 0.05f;
      /// <summary>
      /// State of the current mouse
      /// </summary>
		private static MouseState _currentMouse;
      /// <summary>
      /// State of the mouse from the previous update
      /// </summary>
		private static MouseState _previousMouse;
      /// <summary>
      /// Used to hold area 
      /// </summary>
      private static Rectangle _area;
		/// <summary>
		/// Used to hold the area. Area cannot be stored in SelectedArea property because it does 
		/// not update properly. Holds area after it has been converted into a rectangle
      /// that has only positive lengths/widths
		/// </summary>
		private static Rectangle _selectedArea;
		#endregion

		#region Properties
		/// <summary>
		/// Property that obtains the selected area in the world.
		/// </summary>
		public static Rectangle SelectedRegion => _selectedArea;

		#endregion

		#region Methods
		/// <summary>
		/// Updates the mouse selection
		/// </summary>
		public static void Update()
		{
         _previousMouse = _currentMouse;
         _currentMouse = Mouse.GetState();


         //Updating mouse selection if the button is pressed
         if (_currentMouse.LeftButton == ButtonState.Pressed && _currentMouse.X > MenuSize)
         {
            if (WasDoubleClicked())
            {
               if (_previousMouse.LeftButton == ButtonState.Released)
               {
                  _doubleClicked = true;
                  _area = new Rectangle((int)Settings.MouseTransformLocation.X,
                     (int)Settings.MouseTransformLocation.Y,
                     1, 1);
               }
               else
               {
                  _area.Width = (int)Settings.MouseTransformLocation.X - _area.X;
                  _area.Height = (int)Settings.MouseTransformLocation.Y - _area.Y;
               }
            }
            else if(_currentMouse.X > MenuSize)
				{
               _area = new Rectangle();
				}
            _lastClicked = DateTime.Now;
         }
         else if(_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
			{
            _doubleClicked = false;
			}

         //Checks intersection with different rectangle to reposition the XY point if width or height is negative
         //It's a rectangle for checking! A checktangle! Hahahahaha, please don't deduct points for this name
         Rectangle checkTangle = _area;
         if (_area.Width < 0)
         {
            checkTangle.X = _area.X + _area.Width;
            checkTangle.Width = -_area.Width;
         }
         if (_area.Height < 0)
         {
            checkTangle.Y = _area.Y + _area.Height;
            checkTangle.Height = -_area.Height;
         }
         _selectedArea = checkTangle;
      }

      /// <summary>
      /// Determines if the mouse has been double clicked. Checks to see if the status 
      /// of the mouse is already double clicked. Otherwise, check the time between now and 
      /// when it was last clicked
      /// </summary>
      /// <returns></returns>
      private static bool WasDoubleClicked()
		{
			double elapsedTime = (DateTime.Now - _lastClicked).TotalSeconds;
         return _doubleClicked || (elapsedTime > MinDelay && elapsedTime < MaxDelay);
		}
		#endregion
	}
}
