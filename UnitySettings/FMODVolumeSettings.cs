#if FMOD_MODULE_ENABLED
using System;
using DTT.ExtendedDebugLogs;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using Marmary.SaveSystem;
using UnityEngine;

namespace Marmary.SettingsSystem.UnitySettings
{
    /// <summary>
    ///     Manages the music volume setting for the application.
    ///     Provides methods to set, retrieve, and serialize the music volume,
    ///     and persists changes using a settings repository.
    /// </summary>
    /// <remarks>
    ///     The FMOD bus is resolved lazily on first use: settings are constructed eagerly
    ///     at container build, BEFORE FMOD loads its banks — touching RuntimeManager there
    ///     initializes FMOD too early and breaks audio. Never resolve the bus in the constructor.
    /// </remarks>
    public sealed class FMODVolumeSettings : SettingsConfigureBase<float>
    {
        #region Fields

        /// <summary>
        ///     Reference to the FMOD bus, resolved lazily by <see cref="EnsureBus" />.
        /// </summary>
        private Bus _musicBus;

        /// <summary>
        ///     The name of the FMOD bus to control (e.g. "bus:/" for master).
        /// </summary>
        private readonly string _busName;

        #endregion

        #region Constructors and Injected

        /// <summary>
        ///     Initializes a new instance of the <see cref="FMODVolumeSettings" /> class.
        ///     Does not touch FMOD; the bus is resolved lazily on first use.
        /// </summary>
        /// <param name="settingsRepository">The repository containing settings data.</param>
        /// <param name="defaultValue">The factory default volume the setting resets to.</param>
        /// <param name="busName">The name of the FMOD bus to control (e.g. "bus:/" for master).</param>
        public FMODVolumeSettings(SaveRepository<float> settingsRepository, float defaultValue, string busName)
            : base(settingsRepository, defaultValue)
        {
            _busName = busName;
            settingsRepository.Value = Mathf.Clamp01(settingsRepository.Value);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Sets the music volume and updates the value in the repository.
        /// </summary>
        /// <param name="value">The new music volume value.</param>
        public override void Set(float value)
        {
            var clampedValue = Mathf.Clamp01(value);
            if (EnsureBus()) _musicBus.setVolume(clampedValue);

            settingsRepository.Value = clampedValue;
            DebugEx.Log($"Volume changed to {clampedValue:F2}", SettingTag.Audio);
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
                DebugEx.LogError($"Invalid value for MusicVolumeSettings: {value}", SettingTag.Audio);
        }

        /// <summary>
        ///     Gets the current music volume.
        /// </summary>
        /// <returns>The current music volume as a float.</returns>
        public override float GetCurrentSystem()
        {
            if (EnsureBus() && _musicBus.getVolume(out var volume) == RESULT.OK) return volume;

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
        public override string GetCurrentSystemToString()
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
        ///     Resolves the FMOD bus if it is not valid yet. Safe to call before FMOD is
        ///     initialized: it simply reports failure and retries on the next use.
        ///     On first successful resolution, applies the persisted volume to the bus.
        /// </summary>
        /// <returns><c>true</c> when the bus is valid and usable; otherwise, <c>false</c>.</returns>
        private bool EnsureBus()
        {
            if (_musicBus.isValid()) return true;

            try
            {
                _musicBus = RuntimeManager.GetBus(_busName);
            }
            catch (Exception)
            {
                return false;
            }

            if (!_musicBus.isValid()) return false;

            // First successful resolve: apply the persisted volume.
            _musicBus.setVolume(settingsRepository.Value);
            return true;
        }

        #endregion
    }
}
#endif