
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Diode_Dominion.Engine.Sounds
{
	/// <summary>
	/// Singleton class that is responsible for holding all of the sounds that are playing within the game.
	/// All sounds that want to be played must be passed to this class. This class will manage all of the
	/// sounds that are playing within the game.
	/// </summary>
	public class SoundManager
	{
		#region Fields

		/// <summary>
		/// Singleton instance
		/// </summary>
		private static SoundManager _instance;

		/// <summary>
		/// Background music that is playing within the game
		/// </summary>
		private readonly BackgroundMusic backgroundMusic;

		/// <summary>
		/// Holds the list of currently playing sound effects within the game
		/// </summary>
		private readonly List<SoundEffectInstance> soundEffects;

		/// <summary>
		/// Whether the game was paused on the last update tick
		/// </summary>
		private bool wasGamePaused;

		/// <summary>
		/// How many game ticks there are before a sound effect is removed from the
		/// internal sound effect list.
		/// </summary>
		private const int RemovalInterval = 120;

		/// <summary>
		/// Current tick of the sound manager that will remove
		/// the complete sound effects from the game
		/// </summary>
		private int currentRemoveInterval;

		#endregion

		#region Properties

		/// <summary>
		/// Singleton reference of the sound class
		/// </summary>
		public static SoundManager Instance => _instance ?? (_instance = new SoundManager());

		/// <summary>
		/// Volume the the in-game sound effects play at.
		/// The value can range from 0 (mute) to 1 (full volume)
		/// </summary>
		private float SoundEffectVolume { get; set; } = 1f;

		/// <summary>
		/// Volume that the background music plays at.
		/// The value can range from 0 (mute) to 1 (full volume)
		/// </summary>
		private float BackgroundMusicVolume { get; set; } = 1f;

		#endregion

		#region Constructors

		private SoundManager()
		{
			backgroundMusic = new BackgroundMusic();
			soundEffects = new List<SoundEffectInstance>();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Plays a sound effect within the game world
		/// </summary>
		/// 
		/// <param name="soundEffect">Sound effect to play</param>
		public void PlaySoundEffect(SoundEffectInstance soundEffect)
		{
			// Add to the internal list for control
			soundEffects.Add(soundEffect);

			// Set the volume and immediately play
			soundEffect.Volume = SoundEffectVolume;
			soundEffect.Play();
		}

		public void PauseSoundEffects()
		{
			foreach (SoundEffectInstance soundEffect in soundEffects)
			{
				soundEffect.Pause();
			}
		}

		/// <summary>
		/// Resumes all sounds effects if they were paused
		/// </summary>
		public void ResumeSoundEffects()
		{
			foreach (SoundEffectInstance soundEffect in 
				soundEffects.Where(soundEffect => soundEffect.State == SoundState.Paused))
			{
				soundEffect.Resume();
			}
		}

		/// <summary>
		/// Updates the sound effects for when the game is paused/resumed.
		/// When the game is paused the music pauses and when the game is
		/// then resumed, the music will resume.
		/// </summary>
		public void Update()
		{
			// Check if the game is paused or does not have focus
			if (!Settings.IsGameActive || !Settings.DoesGameHaveFocus)
			{
				wasGamePaused = true;

				PauseSoundEffects();

				if (!Settings.DoesGameHaveFocus)
				{
					backgroundMusic.PauseSong();
				}
			}
			else if (wasGamePaused)
			{
				wasGamePaused = false;

				ResumeSoundEffects();
				backgroundMusic.ResumeSong();
			}
			else
			{
				TickSoundManager();
				RemoveCompleteSoundEffects();
			}
		}

		/// <summary>
		/// Stops all sound effects immediately
		/// </summary>
		public void ClearSoundEffects()
		{
			// Stop every sound effect
			foreach (SoundEffectInstance soundEffectInstance in soundEffects)
			{
				soundEffectInstance.Stop();
			}

			// Remove all current sound effects
			soundEffects.Clear();
		}

		/// <summary>
		/// Change the volume of the background music
		/// </summary>
		/// <param name="volume"></param>
		public void ChangeBackgroundMusicVolume(float volume)
		{
			BackgroundMusicVolume = volume;
			
			backgroundMusic.ChangeVolume(volume * Settings.Volume);
		}

		public void ChangeSoundEffectVolume(float volume)
		{
			foreach (SoundEffectInstance soundEffectInstance in soundEffects)
			{
				soundEffectInstance.Volume = volume;
			}
		}

		/// <summary>
		/// Adds a song to the list of background music that can play.
		/// If it is the first song it will automatically start playing
		/// </summary>
		/// 
		/// <param name="song">Song to add</param>
		public void AddBackgroundMusic(Song song)
		{
			backgroundMusic.AddSong(song);
		}

		/// <summary>
		/// Called on every tick of the game manger while the game is active
		/// </summary>
		private void TickSoundManager()
		{
			currentRemoveInterval++;

			if (currentRemoveInterval > RemovalInterval)
			{
				currentRemoveInterval = 0;
			}
		}

		/// <summary>
		/// Remove all sound effects that are finished playing
		/// </summary>
		private void RemoveCompleteSoundEffects()
		{
			// Only remove if at the tick limit
			if (currentRemoveInterval == RemovalInterval)
			{
				// Loop through all of the songs
				for (int i = 0; i < soundEffects.Count; i++)
				{
					// Check if the sound effect is stopped
					if (soundEffects[i].State == SoundState.Stopped)
					{
						// Remove the stopped sound effect
						soundEffects.Remove(soundEffects[i]);
					}
				}
			}
		}

		#endregion

	}
}
