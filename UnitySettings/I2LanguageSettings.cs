using System;
using System.Collections.Generic;
using I2.Loc;
using Marmary.SaveSystem;

//TODO: USE THE I2 MACRO 
namespace Marmary.SettingsSystem.UnitySettings
{
    /// <summary>
    ///     Manages language settings, including available languages and current selection.
    /// </summary>
    internal class I2LanguageSettings : SettingsConfigureBase<string>
    {
        #region Fields

        /// <summary>
        ///     List of available languages retrieved from the LocalizationManager.
        /// </summary>
        private readonly List<string> _languages;

        #endregion

        #region Constructors and Injected

        /// <summary>
        ///     Initializes a new instance of LanguageSettings with the device's current language.
        /// </summary>
        /// <param name="settingsRepository"></param>
        public I2LanguageSettings(SaveRepositoryGeneric<string> settingsRepository)
            : base(settingsRepository, ResolveDefault(settingsRepository))
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

        #region Methods

        /// <inheritdoc />
        public sealed override void Set(string value)
        {
            if (_languages.Contains(value))
            {
                LocalizationManager.CurrentLanguage = value;
                SettingsRepository.Value = value;
            }
            else
            {
                LocalizationManager.CurrentLanguage = LocalizationManager.GetCurrentDeviceLanguage();
                SettingsRepository.Value = LocalizationManager.GetCurrentDeviceLanguage();
            }
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
            return SettingsRepository.Value;
        }

        /// <summary>
        ///     Returns the current language as a string.
        /// </summary>
        public override string GetCurrentSystenToString()
        {
            return GetCurrentSystem();
        }

        /// <summary>
        ///     Retrieves the current in-memory value of the setting as a string.
        /// </summary>
        /// <returns>
        ///     A string representation of the current in-memory value of the setting.
        /// </returns>
        public override string GetCurrentMemoryToString()
        {
            return GetCurrentMemory();
        }

        /// <inheritdoc />
        public override List<string> GetOptions()
        {
            return _languages ?? new List<string>(LocalizationManager.GetAllLanguages());
        }

        /// <inheritdoc />
        public override List<string> GetOptionsToString()
        {
            return GetOptions();
        }

        /// <summary>
        ///     Resolves the default language setting based on saved preferences, available languages, and device language.
        /// </summary>
        /// <param name="repository">The repository containing the saved language preference.</param>
        /// <returns>The resolved default language as a string.</returns>
        private static string ResolveDefault(SaveRepositoryGeneric<string> repository)
        {
            if (LocalizationManager.Sources.Count == 0) LocalizationManager.UpdateSources();

            var availableLanguages = LocalizationManager.GetAllLanguages();
            if (availableLanguages == null || availableLanguages.Count == 0)
                return LocalizationManager.GetCurrentDeviceLanguage();

            var savedLanguage = repository.Value;
            if (!string.IsNullOrEmpty(savedLanguage) && availableLanguages.Contains(savedLanguage))
                return savedLanguage;

            return LocalizationManager.GetCurrentDeviceLanguage();
        }

        #endregion
    }
}