using UIWidgets;
using UIWidgets.Extensions;
using UnityEngine;
using VContainer;

namespace Marmary.SettingsSystem.UnityComponents
{
    /// <summary>
    ///     Component that manages a combobox UI element for selecting the application's frame rate.
    ///     Populates the combobox with available frame rate options and updates the frame rate setting when changed.
    /// </summary>
    [RequireComponent(typeof(ComboboxString))]
    public class SetFrameRateCombobox : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///     Reference to the injected frame rate settings configuration.
        /// </summary>
        private SettingsConfigureBase<int> _frameRateSettings;

        /// <summary>
        ///     Reference to the ComboboxString component used for frame rate selection.
        /// </summary>
        private ComboboxString _combobox;

        #endregion

        #region Constructors and Injected

        /// <summary>
        ///     Injects the keyed frame rate settings dependency used to populate the combobox.
        /// </summary>
        /// <param name="frameRateSettings">The keyed settings instance for frame rate configuration.</param>
        [Inject]
        public void Construct([Key(SettingsType.FrameRate)] SettingsConfigureBase<int> frameRateSettings)
        {
            _frameRateSettings = frameRateSettings;
        }

        #endregion

        #region Unity Event Functions

        /// <summary>
        ///     Unity Start event. Initializes the combobox with frame rate options and sets up event listeners.
        /// </summary>
        private void Start()
        {
            _combobox = GetComponent<ComboboxString>();

            // Get available frame rate options as strings
            var resolutionOptions = _frameRateSettings.GetOptionsToString();

            // Fill the combobox elements
            using (_combobox.ListView.DataSource.BeginUpdate())
            {
                _combobox.ListView.DataSource.Clear();
                _combobox.ListView.DataSource.AddRange(resolutionOptions.ToObservableList());
            }


            // Set the selected index to match the current screen refresh rate
            _combobox.ListView.SelectedIndex =
                resolutionOptions.IndexOf(_frameRateSettings.GetCurrentMemoryToString());
            _combobox.ListView.OnSelectObject.AddListener(OnValueChanged);
        }

        /// <summary>
        ///     Unity OnDestroy event. Removes the event listener from the combobox to prevent memory leaks.
        /// </summary>
        private void OnDestroy()
        {
            _combobox?.ListView?.OnSelectObject.RemoveListener(OnValueChanged);
        }

        #endregion

        #region Event Functions

        /// <summary>
        ///     Event handler called when the combobox selection changes.
        ///     Updates the frame rate setting based on the selected value.
        /// </summary>
        /// <param name="index">The selected index in the combobox.</param>
        private void OnValueChanged(int index)
        {
            if (index < 0) index = 0;
            _frameRateSettings.SetFromString(_combobox.DataSource[index]);
        }

        #endregion
    }
}