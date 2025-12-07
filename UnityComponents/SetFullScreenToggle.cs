using Marmary.Utils.Runtime;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Marmary.SettingsSystem.UnityComponents
{
    /// <summary>
    ///     Handles the binding between a UI Toggle and the application's full screen setting.
    ///     Synchronizes the toggle state with the full screen setting and updates the setting when the toggle changes.
    /// </summary>
    public class SetFullScreenToggle : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///     Reference to the injected full screen settings configuration.
        /// </summary>
        private SettingsConfigureBase<bool> _fullScreenSettings;

        /// <summary>
        ///     Reference to the UI Toggle component.
        /// </summary>
        private Toggle _toggle;

        #endregion

        #region Constructors and Injected

        /// <summary>
        ///     Injects the keyed fullscreen settings dependency used to synchronize the toggle.
        /// </summary>
        /// <param name="fullScreenSettings">The keyed settings instance for fullscreen configuration.</param>
        [Inject]
        public void Construct([Key(SettingsType.Fullscreen)] SettingsConfigureBase<bool> fullScreenSettings)
        {
            _fullScreenSettings = fullScreenSettings;
        }

        #endregion

        #region Unity Event Functions

        /// <summary>
        ///     Unity event function called on script initialization.
        ///     Initializes the toggle reference, sets the initial full screen state, and subscribes to toggle changes.
        /// </summary>
        [IgnoreUnityLifecycle]
        private void Start()
        {
            _toggle = GetComponent<Toggle>();

            _toggle.isOn = _fullScreenSettings.GetCurrentMemory();

            _toggle.onValueChanged.AddListener(OnValueChanged);
        }

        /// <summary>
        ///     Unity event function called when the object is destroyed.
        ///     Unsubscribes from the toggle value changed event.
        /// </summary>
        private void OnDestroy()
        {
            _toggle.onValueChanged.RemoveListener(OnValueChanged);
        }

        #endregion

        #region Event Functions

        /// <summary>
        ///     Event handler called when the toggle value changes.
        ///     Updates the full screen setting accordingly.
        /// </summary>
        /// <param name="value">The new value of the toggle.</param>
        private void OnValueChanged(bool value)
        {
            _fullScreenSettings.Set(value);
        }

        #endregion
    }
}