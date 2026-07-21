#if UIWIDGETS_INSTALLED
using System;
using Marmary.Utils.Runtime;
using UIWidgets;
using UIWidgets.Extensions;
using UnityEngine;
using VContainer;

namespace Marmary.SettingsSystem.UnityComponents
{
    /// <summary>
    ///     Handles the initialization and value changes of the ComboboxString for language selection.
    ///     Integrates with the injected language settings to reflect and update the current language.
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
        ///     Reference to the injected language settings configuration.
        /// </summary>
        private SettingsConfigureBase<string> _languageSettings;

        /// <summary>
        ///     Reference to the injected language options provider.
        /// </summary>
        private ISettingsOptions<string> _languageOptions;

        #endregion

        #region Constructors and Injected

        /// <summary>
        ///     Dependency injection of the keyed language settings instance and its options provider.
        /// </summary>
        /// <param name="languageSettings">The keyed settings instance for language configuration.</param>
        /// <param name="languageOptions">The keyed options provider for available languages.</param>
        [Inject]
        public void Construct(
            [Key(CommonSettings.Language)] SettingsConfigureBase<string> languageSettings,
            [Key(CommonSettings.Language)] ISettingsOptions<string> languageOptions)
        {
            _languageSettings = languageSettings;
            _languageOptions = languageOptions;
        }

        #endregion

        #region Unity Event Functions

        /// <summary>
        ///     Unity event called when the component is enabled.
        ///     Initializes the ComboBox with available languages and sets the current selection.
        ///     Registers the value change event handler.
        /// </summary>
        [IgnoreUnityLifecycle]
        private void Start()
        {
            _combobox = GetComponent<ComboboxString>();
            if (!_combobox || _languageSettings == null)
                throw new InvalidOperationException(
                    "SetLanguageComboBox requires a ComboboxString component and a keyed language settings instance.");

            var currentLanguage = _languageSettings.GetCurrentMemory();
            var languages = _languageOptions.GetOptions();

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
#endif