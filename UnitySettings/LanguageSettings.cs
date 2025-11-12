using System.Collections.Generic;
using I2.Loc;
using Marmary.HellmenRaaun.Application.Save;
using Marmary.HellmenRaaun.Core;
using Marmary.HellmenRaaun.Domain;
//TODO: Rename to I2LanguageSettings
namespace Marmary.HellmenRaaun.Application.Settings
{
    /// <summary>
    ///     Manages language settings, including available languages and current selection.
    /// </summary>
    internal class LanguageSettings : SettingsConfigureBase<string>
    {
        #region Fields

        /// <summary>
        ///     Stores the currently selected language.
        /// </summary>
        private string _currentLanguage;
        
        /// <summary>
        ///     List of available languages retrieved from the LocalizationManager.
        /// </summary>
        private readonly List<string> _languages;

        #endregion


        /// <summary>
        ///     Initializes a new instance of LanguageSettings with the device's current language.
        /// </summary>
        /// <param name="settingsRepository"></param>
        public LanguageSettings(SaveRepositoryGeneric<SettingsData> settingsRepository) : base(settingsRepository)
        {
            Set(settingsRepository.Value.Language);
            if (LocalizationManager.Sources.Count == 0) LocalizationManager.UpdateSources();
            _languages = LocalizationManager.GetAllLanguages();
            if (_languages == null || _languages.Count == 0)
            {
                throw new System.Exception("No languages found in LocalizationManager sources.");
            }
        }

        #region SettingsConfigureBase<string> Members

        /// <inheritdoc />
        public sealed override void Set(string value)
        {
            if (LocalizationManager.GetAllLanguages().Contains(value))
            {
                LocalizationManager.CurrentLanguage = value;
                SettingsRepository.Value.Language = value;
            }
            else
            {
                LocalizationManager.CurrentLanguage = LocalizationManager.GetCurrentDeviceLanguage();
                SettingsRepository.Value.Language = LocalizationManager.GetCurrentDeviceLanguage();
            }

            SettingsRepository.SaveData();
        }

        /// <inheritdoc />
        public override void SetFromString(string value)
        {
            Set(value);
        }

        /// <inheritdoc />
        public override string GetCurrent()
        {
            return LocalizationManager.CurrentLanguage;
        }

        /// <summary>
        ///     Returns the current language as a string.
        /// </summary>
        public override string GetCurrentToString()
        {
            return GetCurrent();
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

        #endregion
    }
}