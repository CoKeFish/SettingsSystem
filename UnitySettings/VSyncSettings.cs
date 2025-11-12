using System.Collections.Generic;
using Marmary.HellmenRaaun.Application.Save;
using Marmary.HellmenRaaun.Core;
using Marmary.HellmenRaaun.Domain;
using UnityEngine;

namespace Marmary.HellmenRaaun.Application.Settings
{
    /// <summary>
    /// Manages the VSync setting for the application.
    /// Provides methods to set, retrieve, and serialize the VSync state,
    /// and persists changes using a settings repository.
    /// </summary>
    public sealed class VSyncSettings : SettingsConfigureBase<bool>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VSyncSettings"/> class.
        /// Sets the initial VSync value from the settings repository.
        /// </summary>
        /// <param name="settingsRepository">The repository containing settings data.</param>
        public VSyncSettings(SaveRepositoryGeneric<SettingsData> settingsRepository) : base(settingsRepository)
        {
            Set(settingsRepository.Value.VSync);
        }

        /// <summary>
        /// Sets the VSync state and saves the updated value to the repository.
        /// </summary>
        /// <param name="value">If true, enables VSync; otherwise, disables it.</param>
        public override void Set(bool value)
        {
            QualitySettings.vSyncCount = value ? 1 : 0;
            SettingsRepository.Value.VSync = value;
            SettingsRepository.SaveData();
        }

        /// <summary>
        /// Sets the VSync state from a string value.
        /// Logs an error if the value cannot be parsed.
        /// </summary>
        /// <param name="value">A string representing the VSync state ("True" or "False").</param>
        public override void SetFromString(string value)
        {
            if (bool.TryParse(value, out var result))
            {
                Set(result);
            }
            else
            {
                Debug.LogError($"Invalid value for VSyncSettings: {value}");
            }
        }

        /// <summary>
        /// Gets the current VSync state.
        /// </summary>
        /// <returns>True if VSync is enabled; otherwise, false.</returns>
        public override bool GetCurrent()
        {
            return QualitySettings.vSyncCount > 0;
        }

        /// <summary>
        /// Gets the current VSync state as a string.
        /// </summary>
        /// <returns>"True" if VSync is enabled; otherwise, "False".</returns>
        public override string GetCurrentToString()
        {
            return GetCurrent().ToString();
        }

        /// <summary>
        /// Gets the list of possible VSync options.
        /// </summary>
        /// <returns>A list containing true and false values.</returns>
        public override List<bool> GetOptions()
        {
            return new List<bool> { true, false };
        }

        /// <summary>
        /// Gets the list of possible VSync options as strings.
        /// </summary>
        /// <returns>A list containing "True" and "False".</returns>
        public override List<string> GetOptionsToString()
        {
            return new List<string> { "True", "False" };
        }
    }
}