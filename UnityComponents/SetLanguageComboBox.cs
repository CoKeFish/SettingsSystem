using System;
using Marmary.HellmenRaaun.Application.Global;
using Marmary.HellmenRaaun.Application.Settings;
using UIWidgets;
using UIWidgets.Extensions;
using UnityEngine;
using VContainer;

namespace Marmary.Libraries.Settings
{
    /// <summary>
    ///     Handles the initialization and value changes of the ComboboxString for language selection.
    ///     Integrates with <see cref="LanguageSettings" /> to reflect and update the current language.
    /// </summary>
    [RequireComponent(typeof(ComboboxString))]
    public class SetLanguageComboBox : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///     Reference to the ComboboxString component used for language selection.
        /// </summary>
        private ComboboxString _combobox;

        /// <summary>
        ///     Reference to the LanguageSettings instance used to get and set the current language.
        /// </summary>
        private LanguageSettings _languageSettings;

        #endregion

        /// <summary>
        ///     Dependency injection of SettingsManager and retrieval of LanguageSettings.
        /// </summary>
        /// <param name="settingsManager">The settings manager containing language settings.</param>
        [Inject]
        public void Construct(SettingsManager settingsManager)
        {
            _languageSettings = settingsManager.LanguageSettings as LanguageSettings;
        }

        #region Unity Event Functions

        /// <summary>
        ///     Unity event called when the component is enabled.
        ///     Initializes the ComboBox with available languages and sets the current selection.
        ///     Registers the value change event handler.
        /// </summary>
        private void Start()
        {
            _combobox = GetComponent<ComboboxString>();
            if (!_combobox || _languageSettings == null)
                throw new Exception(
                    "SetLanguageComboBox requires a ComboboxString component and LanguageSettings to be set.");

            var currentLanguage = _languageSettings.GetCurrent();
            var languages = _languageSettings.GetOptions();

            using (_combobox.ListView.DataSource.BeginUpdate())
            {
                _combobox.ListView.DataSource.Clear();
                _combobox.ListView.DataSource.AddRange(languages.ToObservableList());
            }


            _combobox.ListView.SelectedIndex = languages.IndexOf(currentLanguage);
            _combobox.ListView.OnSelectObject.AddListener(OnValueChanged);
        }

        /// <summary>
        ///     Unity event called when the component is destroyed.
        /// </summary>
        private void OnDestroy()
        {
            _combobox?.ListView?.OnSelectObject.RemoveListener(OnValueChanged);
        }

        #endregion

        #region Event Functions

        /// <summary>
        ///     Handler for ComboBox selection changes.
        ///     Updates the language in LanguageSettings based on the selected index.
        /// </summary>
        /// <param name="index">Selected index in the ComboBox.</param>
        private void OnValueChanged(int index)
        {
            if (index < 0)
            {
                index = 0;
                _combobox.ListView.SelectedIndex = index;
            }

            var selectedLanguage = _combobox.ListView.DataSource[index];
            _languageSettings.Set(selectedLanguage);
        }

        #endregion
    }
}