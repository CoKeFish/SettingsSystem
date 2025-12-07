using Marmary.Utils.Runtime;
using UIWidgets;
using UIWidgets.Extensions;
using UnityEngine;
using VContainer;

namespace Marmary.SettingsSystem.UnityComponents
{
    /// <summary>
    ///     Handles the population and selection logic for a resolution combobox UI element.
    ///     Binds available screen resolutions to a <see cref="ComboboxString" /> and updates settings on user selection.
    /// </summary>
    [RequireComponent(typeof(ComboboxString))]
    public class SetResolutionCombobox : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///     Reference to the combobox UI component.
        /// </summary>
        private ComboboxString _combobox;

        /// <summary>
        ///     Reference to the injected resolution settings configuration.
        /// </summary>
        private SettingsConfigureBase<Vector2Int> _resolutionSettings;

        #endregion

        #region Constructors and Injected

        /// <summary>
        ///     Injects the keyed resolution settings dependency used for combobox population.
        /// </summary>
        /// <param name="resolutionSettings">The keyed settings instance for resolution configuration.</param>
        [Inject]
        public void Construct([Key(SettingsType.Resolution)] SettingsConfigureBase<Vector2Int> resolutionSettings)
        {
            _resolutionSettings = resolutionSettings;
        }

        #endregion

        #region Unity Event Functions

        /// <summary>
        ///     Unity Start event. Initializes the combobox with available resolution options and sets the current selection.
        /// </summary>
        [IgnoreUnityLifecycle]
        private void Start()
        {
            _combobox = GetComponent<ComboboxString>();

            // Get available resolution options as strings
            var resolutionOptions = _resolutionSettings.GetOptionsToString();

            // Fill the combobox elements
            using (_combobox.ListView.DataSource.BeginUpdate())
            {
                _combobox.ListView.DataSource.Clear();
                _combobox.ListView.DataSource.AddRange(resolutionOptions.ToObservableList());
            }

            // Set the currently selected resolution
            _combobox.ListView.SelectedIndex =
                resolutionOptions.IndexOf(_resolutionSettings.GetCurrentMemoryToString());

            // Register event listener for selection changes
            _combobox.ListView.OnSelectObject.AddListener(OnValueChanged);
        }

        /// <summary>
        ///     Unity OnDestroy event. Cleans up event listeners.
        /// </summary>
        private void OnDestroy()
        {
            _combobox?.ListView?.OnSelectObject.RemoveListener(OnValueChanged);
        }

        #endregion

        #region Event Functions

        /// <summary>
        ///     Handles changes in the combobox selection.
        ///     Updates the resolution setting based on the selected index.
        /// </summary>
        /// <param name="index">The selected index in the combobox.</param>
        private void OnValueChanged(int index)
        {
            if (index < 0) index = 0;

            var resolution = _combobox.ListView.DataSource[index];
            _resolutionSettings.SetFromString(resolution);
        }

        #endregion
    }
}