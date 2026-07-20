#if I2_MODULE_ENABLED
using System;
using System.Collections.Generic;
using DTT.ExtendedDebugLogs;
using I2.Loc;
using Marmary.SaveSystem;

namespace Marmary.SettingsSystem.UnitySettings
{
    /// <summary>
    ///     Manages language settings, including available languages and current selection.
    /// </summary>
    public class I2LanguageSettings : SettingsConfigureBase<string>, ISettingsOptions<string>
    {
        #region Fields

        /// <summary>
        ///     List of available languages retrieved from the LocalizationManager.
        /// </summary>
        private readonly List<string> _languages;

        #endregion

        #region Constructors and Injected

        /// <summary>
        ///     Initializes a new instance of LanguageSettings, applying the saved language
        ///     (or the device language when the saved one is not available).
        /// </summary>
        /// <param name="settingsRepository">Repository for saving and loading the language.</param>
        /// <param name="defaultValue">The factory default language the setting resets to.</param>
        public I2LanguageSettings(SaveRepository<string> settingsRepository, string defaultValue)
            : base(settingsRepository, defaultValue)
        {
            if (LocalizationManager.Sources.Count == 0) LocalizationManager.UpdateSources();
            _languages = LocalizationManager.GetAllLanguages();
            if (_languages == null || _languages.Count == 0)
                throw new Exception("No languages found in LocalizationManager sources.");

            var initialLanguage = _languages.Contains(settingsRepository.Value)
                ? settingsRepository.Value
                : LocalizationManager.GetCurrentDeviceLanguage();
            Set(initialLanguage);
        }

        #endregion

        #region ISettingsOptions Members

        /// <inheritdoc />
        public List<string> GetOptions()
        {
            return _languages ?? new List<string>(LocalizationManager.GetAllLanguages());
        }

        /// <inheritdoc />
        public List<string> GetOptionsToString()
        {
            return GetOptions();
        }

        #endregion

        #region Methods

        /// <inheritdoc />
        public sealed override void Set(string value)
        {
            if (_languages.Contains(value))
            {
                LocalizationManager.CurrentLanguage = value;
                settingsRepository.Value = value;
            }
            else
            {
                LocalizationManager.CurrentLanguage = LocalizationManager.GetCurrentDeviceLanguage();
                settingsRepository.Value = LocalizationManager.GetCurrentDeviceLanguage();
            }

            DebugEx.Log($"Language changed to {LocalizationManager.CurrentLanguage}", SettingTag.Language);
        }

        /// <inheritdoc />
        public override void SetFromString(string value)
        {
            Set(value);
        }

        /// <inheritdoc />
        public override string GetCurrentSystem()
        {
            return LocalizationManager.CurrentLanguage;
        }

        /// <summary>
        ///     Retrieves the current memory value from the settings repository.
        /// </summary>
        /// <returns>The current memory value as a string.</returns>
        public override string GetCurrentMemory()
        {
            return settingsRepository.Value;
        }

        #endregion
    }
}
#endif