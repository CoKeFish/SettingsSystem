using Marmary.HellmenRaaun.Application.Global;
using Marmary.HellmenRaaun.Application.Settings;
using TMPro;
using UnityEngine;
using VContainer;

// TODO: Refactor this to use SettingsManager
namespace Marmary.Libraries.Settings
{
    /// <summary>
    ///     Handles the initialization and value changes of a TMP_Dropdown for language selection.
    ///     Integrates with <see cref="LanguageSettings" /> to update and reflect the current language.
    /// </summary>
    public class SetLanguageDropdownTMP : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///     Reference to the language settings manager.
        /// </summary>
        private LanguageSettings _languageSettings;

        #endregion

        /// <summary>
        ///     Injects the <see cref="SettingsManager" /> dependency and retrieves the <see cref="LanguageSettings" />.
        /// </summary>
        /// <param name="settingsManager">The settings manager containing language settings.</param>
        [Inject]
        public void Construct(SettingsManager settingsManager)
        {
            // Constructor injection for SettingsManager
            _languageSettings = settingsManager.LanguageSettings as LanguageSettings;
        }

        #region Unity Event Functions

        /// <summary>
        ///     Unity event function called when the object becomes enabled and active.
        ///     Initializes the dropdown with available languages and sets the current selection.
        /// </summary>
        private void OnEnable()
        {
            var dropdown = GetComponent<TMP_Dropdown>();
            if (dropdown == null)
                return;

            var currentLanguage = _languageSettings.GetCurrent();
            var languages = _languageSettings.GetOptions();

            // Fill the dropdown elements
            dropdown.ClearOptions();
            dropdown.AddOptions(languages);

            dropdown.value = languages.IndexOf(currentLanguage);
            dropdown.onValueChanged.RemoveListener(OnValueChanged);
            dropdown.onValueChanged.AddListener(OnValueChanged);
        }

        #endregion

        #region Event Functions

        /// <summary>
        ///     Event handler for when the dropdown value changes.
        ///     Updates the current language in <see cref="LanguageSettings" />.
        /// </summary>
        /// <param name="index">The selected index in the dropdown.</param>
        private void OnValueChanged(int index)
        {
            var dropdown = GetComponent<TMP_Dropdown>();
            if (index < 0)
            {
                index = 0;
                dropdown.value = index;
            }

            _languageSettings.Set(dropdown.options[index].text);
        }

        #endregion
    }
}