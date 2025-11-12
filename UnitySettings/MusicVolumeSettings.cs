using System;
using System.Collections.Generic;
using FMOD.Studio;
using FMODUnity;
using Marmary.HellmenRaaun.Application.Save;
using Marmary.HellmenRaaun.Core;
using Marmary.HellmenRaaun.Domain;
using UnityEngine;

//TODO: Rename to FMODVolumeSettings

namespace Marmary.HellmenRaaun.Application.Settings
{
    /// <summary>
    ///     Manages the music volume setting for the application.
    ///     Provides methods to set, retrieve, and serialize the music volume,
    ///     and persists changes using a settings repository.
    /// </summary>
    public sealed class MusicVolumeSettings : SettingsConfigureBase<float>
    {
        #region Fields

        /// <summary>
        ///     Reference to the FMOD music bus.
        /// </summary>
        private Bus _musicBus;

        #endregion

        /// <summary>
        ///     Initializes a new instance of the <see cref="MusicVolumeSettings" /> class.
        ///     Sets the initial music volume from the settings repository.
        /// </summary>
        /// <param name="settingsRepository">The repository containing settings data.</param>
        /// <param name="busName">The name of the FMOD bus for music.</param>
        public MusicVolumeSettings(SaveRepositoryGeneric<SettingsData> settingsRepository, string busName) : base(
            settingsRepository)
        {
            try
            {
                _musicBus = RuntimeManager.GetBus(busName);
                Set(settingsRepository.Value.VolumeMusic);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to initialize MusicVolumeSettings: {e.Message}");
            }
        }

        #region Methods

        /// <summary>
        ///     Sets the music volume and saves the updated value to the repository.
        /// </summary>
        /// <param name="value">The new music volume value.</param>
        public override void Set(float value)
        {
            _musicBus.setVolume(SettingsRepository.Value.VolumeMusic);
            SettingsRepository.Value.VolumeMusic = value;
            SettingsRepository.SaveData();
        }

        /// <summary>
        ///     Sets the music volume from a string value.
        ///     Logs an error if the value cannot be parsed.
        /// </summary>
        /// <param name="value">A string representing the music volume.</param>
        public override void SetFromString(string value)
        {
            if (float.TryParse(value, out var result))
                Set(result);
            else
                Debug.LogError($"Invalid value for MusicVolumeSettings: {value}");
        }

        /// <summary>
        ///     Gets the current music volume.
        /// </summary>
        /// <returns>The current music volume as a float.</returns>
        public override float GetCurrent()
        {
            _musicBus.getVolume(out var volume);
            return volume;
        }

        /// <summary>
        ///     Gets the current music volume as a formatted string.
        /// </summary>
        /// <returns>The current music volume as a string with two decimal places.</returns>
        public override string GetCurrentToString()
        {
            _musicBus.getVolume(out var volume);
            return volume.ToString("F2");
        }

        /// <summary>
        ///     Not applicable for MusicVolumeSettings, as there are no predefined options.
        /// </summary>
        /// <returns>Throws a <see cref="NotImplementedException" />.</returns>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public override List<float> GetOptions()
        {
            throw new NotImplementedException("MusicVolumeSettings does not have predefined options.");
        }

        /// <summary>
        ///     Not applicable for MusicVolumeSettings, as there are no predefined options.
        /// </summary>
        /// <returns>Throws a <see cref="NotImplementedException" />.</returns>
        /// <exception cref="NotImplementedException">Always thrown.</exception>
        public override List<string> GetOptionsToString()
        {
            throw new NotImplementedException("MusicVolumeSettings does not have predefined options.");
        }

        #endregion
    }
}