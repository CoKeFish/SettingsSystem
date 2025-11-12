using Marmary.HellmenRaaun.Application.Global;
using Marmary.SettingsSystem.UnitySettings;
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
        ///     Reference to the full screen settings manager.
        /// </summary>
        private FullScreenSettings _fullScreenSettings;

        /// <summary>
        ///     Reference to the UI Toggle component.
        /// </summary>
        private Toggle _toggle;

        #endregion

        #region Constructors and Injected

        /// <summary>
        ///     Injects the <see cref="SettingsManager" /> dependency and retrieves the <see cref="FullScreenSettings" />.
        /// </summary>
        /// <param name="settingsManager">The settings manager containing full screen settings.</param>
        [Inject]
        public void Construct(SettingsManager settingsManager)
        {
            _fullScreenSettings = settingsManager.FullScreenSettings as FullScreenSettings;
        }

        #endregion

        #region Unity Event Functions

        /// <summary>
        ///     Unity event function called on script initialization.
        ///     Initializes the toggle reference, sets the initial full screen state, and subscribes to toggle changes.
        /// </summary>
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