using Marmary.HellmenRaaun.Application.Global;
using Marmary.SettingsSystem.UnitySettings;
using TMPro;
using UnityEngine;
using VContainer;

// TODO: Refactor this to use SettingsManager
namespace Marmary.SettingsSystem.UnityComponents
{
    /// <summary>
    ///     Handles the initialization and value changes of a TMP_Dropdown for language selection.
    ///     Integrates with <see cref="I2LanguageSettings" /> to update and reflect the current language.
    /// </summary>
    public class SetLanguageDropdownTMP : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///     Reference to the language settings manager.
        /// </summary>
        private I2LanguageSettings _i2LanguageSettings;

        #endregion

        #region Constructors and Injected

        /// <summary>
        ///     Injects the <see cref="SettingsManager" /> dependency and retrieves the <see cref="I2LanguageSettings" />.
        /// </summary>
        /// <param name="settingsManager">The settings manager containing language settings.</param>
        [Inject]
        public void Construct(SettingsManager settingsManager)
        {
            // Constructor injection for SettingsManager
            _i2LanguageSettings = settingsManager.LanguageSettings as I2LanguageSettings;
        }

        #endregion

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

            var currentLanguage = _i2LanguageSettings.GetCurrentMemory();
            var languages = _i2LanguageSettings.GetOptions();

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
        ///     Updates the current language in <see cref="I2LanguageSettings" />.
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

            _i2LanguageSettings.Set(dropdown.options[index].text);
        }

        #endregion
    }
}