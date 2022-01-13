using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Diode_Dominion.Engine.Containers
{
    /// <summary>
    /// A container that holds the items passed into it in a grid.
    /// The only specification is the number of columns and it will
    /// be divided into a left-leaning grid, with a variable number
    /// of rows based on the number of items passed in.
    /// </summary>
    public class GridContainer : Container
    {
        #region Fields

        /// <summary>
        /// Holds the previous number of columns until the location
        /// of the components are updated. Used to check if the locations
        /// of the components need to be updated.
        /// 
        /// Defaults to 0 to force a location update on the first update
        /// call.
        /// </summary>
        private int previousColumns;

        #endregion

        #region Properties

        /// <summary>
        /// Number of columns in the grid container.
        /// Defaults to 1 column
        /// </summary>
        public int Columns { get; set; } = 1;

        #endregion

        #region Contructors

        /// <summary>
        /// General constructor that allows for the creation of a grid container
        /// </summary>
        /// 
        /// <param name="texture">Texture of the container</param>
        /// <param name="xCoordinate">X coordinate of the container</param>
        /// <param name="yCoordinate">Y coordinate of the container</param>
        public GridContainer(Texture2D texture, int xCoordinate, int yCoordinate) : base(texture, xCoordinate, yCoordinate)
        {

        }

        /// <summary>
        /// Constructor that allows for the creation of a grid container with
        /// a set number of columns right away.
        /// </summary>
        /// 
        /// <param name="texture">Texture of the container</param>
        /// <param name="xCoordinate">X coordinate of the container</param>
        /// <param name="yCoordinate">Y coordinate of the container</param>
        /// <param name="columns">Number of columns in the container</param>
        public GridContainer(Texture2D texture, int xCoordinate, int yCoordinate, int columns) : base(texture, xCoordinate, yCoordinate)
        {
            Columns = columns;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates the components that are within the container.
        /// </summary>
        /// 
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            // Checks if there was a change in the number of columns
            if (previousColumns != Columns)
            {
                previousColumns = Columns;
                UpdateComponentLocations();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Updates the location of the components.
        /// Should only be called when a change in the number of columns is detected.
        /// </summary>
        private void UpdateComponentLocations()
        {
            int numberRows = (int)Math.Ceiling((float)ComponentManager.Count / Columns);
            float componentYSpacing = (Sprite.Texture.Height - Padding * numberRows) / (float)numberRows;
            float componentXSpacing = (Sprite.Texture.Width - Padding * Columns) / (float)Columns;

            // Use to make the columns
            for (int i = 0; i < Columns; i++)
            {
                // Use to make the rows
                for (int j = 0; j < numberRows; j++)
                {
                    // Grab the location of the component
                    Vector2 componentLoc = ComponentManager[(i + 1) * j].Sprite.Origin;

                    // Update the location
                    componentLoc.X = X + ((i + 1) * Padding) + i * componentXSpacing;
                    componentLoc.Y = Y + ((j + 1) * Padding) + j * componentYSpacing;

                    // Set the new location
                    ComponentManager[(i + 1) * j].Sprite.X = componentLoc.X;
                    ComponentManager[(i + 1) * j].Sprite.Y = componentLoc.Y;
                }
            }
        }

        #endregion
    }
}
