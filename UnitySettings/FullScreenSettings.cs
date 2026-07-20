using System.Collections.Generic;
using DTT.ExtendedDebugLogs;
using Marmary.SaveSystem;
using UnityEngine;

namespace Marmary.SettingsSystem.UnitySettings
{
    /// <summary>
    ///     Manages the fullscreen display setting for the application.
    ///     Inherits from <see cref="SettingsConfigureBase{T}" /> with a boolean value indicating fullscreen state.
    /// </summary>
    public sealed class FullScreenSettings : SettingsConfigureBase<bool>, ISettingsOptions<bool>
    {
        #region Constructors and Injected

        /// <summary>
        ///     Initializes a new instance of the <see cref="FullScreenSettings" /> class.
        ///     Applies the fullscreen state loaded from the settings repository.
        /// </summary>
        /// <param name="settingsRepository">The repository containing the settings data.</param>
        /// <param name="defaultValue">The factory default fullscreen state the setting resets to.</param>
        public FullScreenSettings(SaveRepository<bool> settingsRepository, bool defaultValue)
            : base(settingsRepository, defaultValue)
        {
            Set(settingsRepository.Value);
        }

        #endregion

        #region ISettingsOptions Members

        /// <summary>
        ///     Gets the list of possible fullscreen options.
        /// </summary>
        /// <returns>A list containing true (enabled) and false (disabled).</returns>
        public List<bool> GetOptions()
        {
            return new List<bool>
            {
                true,
                false
            };
        }

        /// <summary>
        ///     Gets the list of possible fullscreen options as strings.
        /// </summary>
        /// <returns>A list containing "True" and "False".</returns>
        public List<string> GetOptionsToString()
        {
            return new List<string>
            {
                "True",
                "False"
            };
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Sets the fullscreen mode and updates the settings repository.
        /// </summary>
        /// <param name="value">If true, enables fullscreen; otherwise, disables it.</param>
        public override void Set(bool value)
        {
            var fullScreenToSet = value;
            Screen.fullScreen = fullScreenToSet;
            settingsRepository.Value = fullScreenToSet;
            DebugEx.Log($"Fullscreen changed to {fullScreenToSet}", SettingTag.Screen);
        }

        /// <summary>
        ///     Sets the fullscreen mode from a string value.
        /// </summary>
        /// <param name="value">A string representing the fullscreen state ("True" or "False").</param>
        public override void SetFromString(string value)
        {
            if (bool.TryParse(value, out var result))
                Set(result);
            else
                DebugEx.LogError($"Invalid value for FullScreenSettings: {value}", SettingTag.Screen);
        }

        /// <summary>
        ///     Retrieves the current fullscreen state of the system.
        ///     Returns whether the application is currently running in fullscreen mode.
        /// </summary>
        /// <returns>A boolean value where true indicates fullscreen mode is enabled, and false indicates it is disabled.</returns>
        public override bool GetCurrentSystem()
        {
            return Screen.fullScreen;
        }

        /// <summary>
        ///     Retrieves the current memory value from the settings repository.
        /// </summary>
        /// <returns>
        ///     The current memory value.
        /// </returns>
        public override bool GetCurrentMemory()
        {
            return settingsRepository.Value;
        }

        #endregion
    }
}