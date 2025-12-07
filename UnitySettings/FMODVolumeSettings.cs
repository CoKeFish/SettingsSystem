using System;
using System.Collections.Generic;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Marmary.SaveSystem;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Marmary.SettingsSystem.UnitySettings
{
    /// <summary>
    ///     Manages the music volume setting for the application.
    ///     Provides methods to set, retrieve, and serialize the music volume,
    ///     and persists changes using a settings repository.
    /// </summary>
    public sealed class FMODVolumeSettings : SettingsConfigureBase<float>
    {
        #region Fields

        /// <summary>
        ///     Reference to the FMOD music bus.
        /// </summary>
        private Bus _musicBus;

        #endregion

        #region Constructors and Injected

        /// <summary>
        ///     Initializes a new instance of the <see cref="FMODVolumeSettings" /> class.
        ///     Sets the initial music volume from the settings repository.
        /// </summary>
        /// <param name="settingsRepository">The repository containing settings data.</param>
        /// <param name="busName">The name of the FMOD bus for music.</param>
        public FMODVolumeSettings(SaveRepositoryGeneric<float> settingsRepository, string busName) : base(
            settingsRepository, ResolveDefault(settingsRepository))
        {
            try
            {
                _musicBus = RuntimeManager.GetBus(busName);
                Set(settingsRepository.Value);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to initialize MusicVolumeSettings: {e.Message}");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Sets the music volume and saves the updated value to the repository.
        /// </summary>
        /// <param name="value">The new music volume value.</param>
        public override void Set(float value)
        {
            var clampedValue = Mathf.Clamp01(value);
            if (_musicBus.isValid()) _musicBus.setVolume(clampedValue);

            settingsRepository.Value = clampedValue;
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
        public override float GetCurrentSystem()
        {
            if (_musicBus.isValid() && _musicBus.getVolume(out var volume) == RESULT.OK) return volume;

            return settingsRepository.Value;
        }

        /// <summary>
        ///     Retrieves the current memory value from the settings repository.
        /// </summary>
        /// <returns>
        ///     A float representing the current memory setting value.
        /// </returns>
        public override float GetCurrentMemory()
        {
            return settingsRepository.Value;
        }

        /// <summary>
        ///     Gets the current music volume as a formatted string.
        /// </summary>
        /// <returns>The current music volume as a string with two decimal places.</returns>
        public override string GetCurrentSystenToString()
        {
            return GetCurrentSystem().ToString("F2");
        }

        /// <summary>
        ///     Converts the current in-memory value of the setting to a string representation
        ///     formatted to two decimal places.
        /// </summary>
        /// <returns>
        ///     A string representing the current in-memory value of the setting, formatted to two decimal places.
        /// </returns>
        public override string GetCurrentMemoryToString()
        {
            return GetCurrentMemory().ToString("F2");
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

        /// <summary>
        ///     Resolves the default value for the music volume setting from the provided repository.
        ///     Ensures the value is clamped within the valid range [0, 1].
        /// </summary>
        /// <param name="repository">The repository containing the default volume value.</param>
        /// <returns>The clamped default volume value.</returns>
        private static float ResolveDefault(SaveRepositoryGeneric<float> repository)
        {
            return Mathf.Clamp01(repository.Value);
        }

        #endregion
    }
}