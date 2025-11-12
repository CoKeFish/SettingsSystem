using System.Collections.Generic;
using Marmary.HellmenRaaun.Application.Save;
using Marmary.HellmenRaaun.Core;
using Marmary.HellmenRaaun.Domain;
using UnityEngine;

namespace Marmary.HellmenRaaun.Application.Settings
{
    /// <summary>
    ///     Manages the fullscreen display setting for the application.
    ///     Inherits from <see cref="SettingsConfigureBase{T}" /> with a boolean value indicating fullscreen state.
    /// </summary>
    public sealed class FullScreenSettings : SettingsConfigureBase<bool>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FullScreenSettings" /> class.
        ///     Sets the fullscreen state based on the saved settings.
        /// </summary>
        /// <param name="settingsRepository">The repository containing the settings data.</param>
        public FullScreenSettings(SaveRepositoryGeneric<SettingsData> settingsRepository) : base(settingsRepository)
        {
            Set(settingsRepository.Value.FullScreen);
        }

        #region Methods

        /// <summary>
        ///     Sets the fullscreen mode and updates the settings repository.
        /// </summary>
        /// <param name="value">If true, enables fullscreen; otherwise, disables it.</param>
        public override void Set(bool value)
        {
            Screen.fullScreen = value;
            SettingsRepository.Value.FullScreen = value;
            SettingsRepository.SaveData();
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
        ///     Gets the current fullscreen state.
        /// </summary>
        /// <returns>True if fullscreen is enabled; otherwise, false.</returns>
        public override bool GetCurrent()
        {
            return SettingsRepository.Value.FullScreen;
        }

        /// <summary>
        ///     Gets the current fullscreen state as a string.
        /// </summary>
        /// <returns>"True" if fullscreen is enabled; otherwise, "False".</returns>
        public override string GetCurrentToString()
        {
            return GetCurrent().ToString();
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

        #endregion
    }
}