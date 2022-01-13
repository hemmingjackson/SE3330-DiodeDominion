using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Diode_Dominion.Engine.GameTimer
{
	/// <summary>
	/// This class contains the implementation of a game timer
	/// </summary>
    public class GameTimer : GameComponent
    {
        public static float Time { get; set; }

        public static float GameStartTime { get; set; }

        /// <summary>
		/// General Constructor for game timer
		/// </summary>
		/// <param name="game"></param>
		/// <param name="startTime"></param>
        public GameTimer(Game game, float startTime):base(game)
        {
            Time = startTime;
            GameStartTime = startTime;
            Started = false;
            Paused = false;
            Text = "";
        }

		/// <summary>
		/// Creates the update for the timer
		/// </summary>
		/// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Started && !Paused)
            {
                Time += deltaTime;
            }

            Text = ((int)Time).ToString();
            base.Update(gameTime);
        }


		/// <summary>
		/// Draws timer, giving it a font, text, position, and color
		/// </summary>
		/// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawString(Font, Text, Position, Color.White);
        }


		/// <summary>
		/// Getter/setter for timer Font
		/// </summary>
        public SpriteFont Font { get; set; }


		/// <summary>
		/// Getter/setter for timer Text
		/// </summary>
        public string Text { get; set; }


		/// <summary>
		/// Getter/setter for timer - Started condition
		/// </summary>
        public bool Started { get; set; }

		/// <summary>
		/// Getter/setter for timer - Paused condition
		/// </summary>
        public bool Paused { get; set; }

		/// <summary>
		/// Getter/setter for Position of the timer
		/// </summary>
        public Vector2 Position { get; set; }
    }
}
