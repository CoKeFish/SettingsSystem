using System.Collections.Generic;
using DTT.ExtendedDebugLogs;
using Marmary.SaveSystem;
using UnityEngine;

namespace Marmary.SettingsSystem.UnitySettings
{
    /// <summary>
    ///     Manages the VSync setting for the application.
    ///     Provides methods to set, retrieve, and serialize the VSync state,
    ///     and persists changes using a settings repository.
    /// </summary>
    public sealed class VSyncSettings : SettingsConfigureBase<bool>, ISettingsOptions<bool>
    {
        #region Constructors and Injected

        /// <summary>
        ///     Initializes a new instance of the <see cref="VSyncSettings" /> class.
        ///     Applies the VSync value loaded from the settings repository.
        /// </summary>
        /// <param name="settingsRepository">The repository containing settings data.</param>
        /// <param name="defaultValue">The factory default VSync state the setting resets to.</param>
        public VSyncSettings(SaveRepository<bool> settingsRepository, bool defaultValue)
            : base(settingsRepository, defaultValue)
        {
            Set(settingsRepository.Value);
        }

        #endregion

        #region ISettingsOptions Members

        /// <summary>
        ///     Gets the list of possible VSync options.
        /// </summary>
        /// <returns>A list containing true and false values.</returns>
        public List<bool> GetOptions()
        {
            return new List<bool> { true, false };
        }

        /// <summary>
        ///     Gets the list of possible VSync options as strings.
        /// </summary>
        /// <returns>A list containing "True" and "False".</returns>
        public List<string> GetOptionsToString()
        {
            return new List<string> { "True", "False" };
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Sets the VSync state and updates the value in the repository.
        /// </summary>
        /// <param name="value">If true, enables VSync; otherwise, disables it.</param>
        public override void Set(bool value)
        {
            QualitySettings.vSyncCount = value ? 1 : 0;
            settingsRepository.Value = value;
            DebugEx.Log($"VSync changed to {value}", SettingTag.Render);
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
                DebugEx.LogError($"Invalid value for VSyncSettings: {value}", SettingTag.Render);
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
            return settingsRepository.Value;
        }

        #endregion
    }
}