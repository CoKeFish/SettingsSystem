using Marmary.HellmenRaaun.Application.Global;
using Marmary.SettingsSystem.UnitySettings;
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
        ///     Reference to the ComboboxString component used for frame rate selection.
        /// </summary>
        private ComboboxString _combobox;

        /// <summary>
        ///     Reference to the frame rate settings.
        /// </summary>
        private FrameRateSettings _frameRateSettings;

        #endregion

        #region Constructors and Injected

        /// <summary>
        ///     Injects the <see cref="SettingsManager" /> dependency and retrieves the <see cref="FrameRateSettings" />.
        /// </summary>
        /// <param name="settingsManager">The settings manager instance.</param>
        [Inject]
        public void Construct(SettingsManager settingsManager)
        {
            _frameRateSettings = settingsManager.FrameRateSettings as FrameRateSettings;
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