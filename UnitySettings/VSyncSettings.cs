using System.Collections.Generic;
using Marmary.SaveSystem;
using UnityEngine;

namespace Marmary.SettingsSystem.UnitySettings
{
    /// <summary>
    ///     Manages the VSync setting for the application.
    ///     Provides methods to set, retrieve, and serialize the VSync state,
    ///     and persists changes using a settings repository.
    /// </summary>
    public sealed class VSyncSettings : SettingsConfigureBase<bool>
    {
        #region Constructors and Injected

        /// <summary>
        ///     Initializes a new instance of the <see cref="VSyncSettings" /> class.
        ///     Sets the initial VSync value from the settings repository.
        /// </summary>
        /// <param name="settingsRepository">The repository containing settings data.</param>
        public VSyncSettings(SaveRepositoryGeneric<bool> settingsRepository)
            : base(settingsRepository, ResolveDefault(settingsRepository))
        {
            Set(settingsRepository.Value);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Sets the VSync state and saves the updated value to the repository.
        /// </summary>
        /// <param name="value">If true, enables VSync; otherwise, disables it.</param>
        public override void Set(bool value)
        {
            QualitySettings.vSyncCount = value ? 1 : 0;
            SettingsRepository.Value = value;
        }

        /// <summary>
        ///     Sets the VSync state from a string value.
        ///     Logs an error if the value cannot be parsed.
        /// </summary>
        /// <param name="value">A string representing the VSync state ("True" or "False").</param>
        public override void SetFromString(string value)
        {
            if (bool.TryParse(value, out var result))
                Set(result);
            else
                Debug.LogError($"Invalid value for VSyncSettings: {value}");
        }

        /// <summary>
        ///     Gets the current VSync state.
        /// </summary>
        /// <returns>True if VSync is enabled; otherwise, false.</returns>
        public override bool GetCurrentSystem()
        {
            return QualitySettings.vSyncCount > 0;
        }

        /// <summary>
        ///     Retrieves the current value stored in memory for the VSync setting.
        /// </summary>
        /// <returns>The current value of VSync stored in memory.</returns>
        public override bool GetCurrentMemory()
        {
            return SettingsRepository.Value;
        }

        /// <summary>
        ///     Retrieves the current system's VSync state as a string representation.
        /// </summary>
        /// <returns>A string indicating the VSync state: "True" if enabled; otherwise, "False".</returns>
        public override string GetCurrentSystenToString()
        {
            return GetCurrentSystem().ToString();
        }

        /// <summary>
        ///     Converts the current in-memory representation of the VSync setting to its string representation.
        /// </summary>
        /// <returns>A string representation of the current in-memory VSync setting.</returns>
        public override string GetCurrentMemoryToString()
        {
            return GetCurrentMemory().ToString();
        }

        /// <summary>
        ///     Gets the list of possible VSync options.
        /// </summary>
        /// <returns>A list containing true and false values.</returns>
        public override List<bool> GetOptions()
        {
            return new List<bool> { true, false };
        }

        /// <summary>
        ///     Gets the list of possible VSync options as strings.
        /// </summary>
        /// <returns>A list containing "True" and "False".</returns>
        public override List<string> GetOptionsToString()
        {
            return new List<string> { "True", "False" };
        }

        /// <summary>
        ///     Resolves the default value for the VSync setting using the provided settings repository.
        ///     Retrieves the stored value in the repository to serve as the default value.
        /// </summary>
        /// <param name="repository">The repository containing the saved VSync setting value.</param>
        /// <returns>The default VSync value retrieved from the repository.</returns>
        private static bool ResolveDefault(SaveRepositoryGeneric<bool> repository)
        {
            return repository.Value;
        }

        #endregion
    }
}