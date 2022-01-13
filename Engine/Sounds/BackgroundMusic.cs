using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;

namespace Diode_Dominion.Engine.Sounds
{
	/// <summary>
	/// Holds functionality for the background music that plays within the game world
	/// </summary>
	internal class BackgroundMusic
	{
		#region Fields

		/// <summary>
		/// How much the volume should scale by from the passed in valueS
		/// </summary>
		private const float VolumeScale = 0.1f;

		#endregion

		#region Properties

		/// <summary>
		/// List of possible background music that can be played within the game
		/// </summary>
		internal List<Song> Songs { get; }

		/// <summary>
		/// Current song that is playing
		/// </summary>
		internal Song CurrentSong { get; private set; }

		/// <summary>
		/// Volume of the background music. Plays at 1/10th volume
		/// </summary>
		private float Volume { get; set; } = 1f;

		#endregion

		internal BackgroundMusic()
		{
			Songs = new List<Song>();

			// Set media player settings
			MediaPlayer.Volume = Volume * VolumeScale;

			MediaPlayer.ActiveSongChanged += MediaPlayer_ActiveSongChanged;
		}
		
		#region Methods

		/// <summary>
		/// Called when a song finishes
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MediaPlayer_ActiveSongChanged(object sender, System.EventArgs e)
		{
			if (MediaPlayer.State == MediaState.Stopped)
			{
				int nextSongIndex = Songs.IndexOf(CurrentSong) + 1;

				// Going outside of range
				if (nextSongIndex >= Songs.Count)
				{
					nextSongIndex = 0;
				}

				// Change to the song
				ChangeSong(nextSongIndex);
			}
		}

		/// <summary>
		/// Adds a song to the list of possible background music that can
		/// play
		/// </summary>
		/// 
		/// <param name="song">Song that can play in the background</param>
		internal void AddSong(Song song)
		{
			Songs.Add(song);

			// Check if that was the fist song added
			if (Songs.Count == 1)
			{
				CurrentSong = song;

				MediaPlayer.Play(song);
			}
		}

		/// <summary>
		/// Removes a specific song from the list of background songs
		/// </summary>
		/// 
		/// <param name="song"></param>
		internal void RemoveSong(Song song)
		{
			Songs.Remove(song);
		}

		/// <summary>
		/// Pause the current background song that is playing
		/// </summary>
		internal void PauseSong()
		{
			MediaPlayer.Pause();
		}

		/// <summary>
		/// Resumes the current song that is playing
		/// </summary>
		internal void ResumeSong()
		{
			MediaPlayer.Resume();
		}

		/// <summary>
		/// Switches the song that is playing to the index
		/// of the list of songs within this class
		/// </summary>
		/// 
		/// <param name="songNumber">Song number to play</param>
		internal void ChangeSong(int songNumber)
		{
			CurrentSong = Songs[songNumber];
			
			MediaPlayer.Play(Songs[songNumber]);
		}

		/// <summary>
		/// Changes the volume of the music
		/// </summary>
		/// <param name="volume">Range from 0 (mute) to 1 (full volume)</param>
		internal void ChangeVolume(float volume)
		{
			MediaPlayer.Volume = volume * VolumeScale;
		}

		#endregion
	}
}
