using System.Collections.Generic;
using Marmary.SaveSystem;
using UnityEngine;

namespace Marmary.SettingsSystem.UnitySettings
{
    /// <summary>
    ///     Manages the fullscreen display setting for the application.
    ///     Inherits from <see cref="SettingsConfigureBase{T}" /> with a boolean value indicating fullscreen state.
    /// </summary>
    public sealed class FullScreenSettings : SettingsConfigureBase<bool>
    {
        #region Constructors and Injected

        /// <summary>
        ///     Initializes a new instance of the <see cref="FullScreenSettings" /> class.
        ///     Sets the fullscreen state based on the saved settings.
        /// </summary>
        /// <param name="settingsRepository">The repository containing the settings data.</param>
        public FullScreenSettings(SaveRepositoryGeneric<bool> settingsRepository)
            : base(settingsRepository, ResolveDefault(settingsRepository))
        {
            Set(settingsRepository.Value);
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
            SettingsRepository.Value = fullScreenToSet;
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
                Debug.LogError($"Invalid value for FullScreenSettings: {value}");
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
            return SettingsRepository.Value;
        }

        /// <summary>
        ///     Converts the current system setting value to its string representation.
        /// </summary>
        /// <returns>
        ///     A string representation of the current system setting value.
        /// </returns>
        public override string GetCurrentSystenToString()
        {
            return GetCurrentSystem().ToString();
        }

        /// <summary>
        ///     Retrieves the current memory value as a string representation.
        /// </summary>
        /// <returns>
        ///     A string representation of the current memory value.
        /// </returns>
        public override string GetCurrentMemoryToString()
        {
            return GetCurrentMemory().ToString();
        }

        /// <summary>
        ///     Gets the list of possible fullscreen options.
        /// </summary>
        /// <returns>A list containing true (enabled) and false (disabled).</returns>
        public override List<bool> GetOptions()
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
        public override List<string> GetOptionsToString()
        {
            return new List<string>
            {
                "True",
                "False"
            };
        }


        /// <summary>
        ///     Resolves the default value for the fullscreen setting based on the saved repository data.
        /// </summary>
        /// <param name="repository">The repository containing the settings data.</param>
        /// <returns>The default fullscreen state as a boolean value.</returns>
        private static bool ResolveDefault(SaveRepositoryGeneric<bool> repository)
        {
            return repository.Value;
        }

        #endregion
    }
}