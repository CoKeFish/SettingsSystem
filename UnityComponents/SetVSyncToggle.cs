using Marmary.Utils.Runtime;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Marmary.SettingsSystem.UnityComponents
{
    /// <summary>
    ///     Handles the binding between a UI Toggle and the application's VSync setting.
    ///     Synchronizes the toggle state with the VSync setting and updates the setting when the toggle changes.
    /// </summary>
    public class SetVSyncToggle : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///     Reference to the injected VSync settings configuration.
        /// </summary>
        private SettingsConfigureBase<bool> _vSyncSettings;

        /// <summary>
        ///     Reference to the UI Toggle component.
        /// </summary>
        private Toggle _toggle;

        #endregion

        #region Constructors and Injected

        /// <summary>
        ///     Injects the keyed VSync settings dependency.
        /// </summary>
        /// <param name="vSyncSettings">The keyed settings instance for VSync configuration.</param>
        [Inject]
        public void Construct([Key(SettingsType.VSync)] SettingsConfigureBase<bool> vSyncSettings)
        {
            _vSyncSettings = vSyncSettings;
        }

        #endregion

        #region Unity Event Functions

        /// <summary>
        ///     Unity event function called on script initialization.
        ///     Initializes the toggle reference, sets the initial VSync state, and subscribes to toggle changes.
        /// </summary>
        [IgnoreUnityLifecycle]
        private void Start()
        {
            _toggle = GetComponent<Toggle>();
            _toggle.isOn = _vSyncSettings.GetCurrentMemory();
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
        ///     Updates the VSync setting accordingly.
        /// </summary>
        /// <param name="value">The new value of the toggle.</param>
        private void OnValueChanged(bool value)
        {
            _vSyncSettings.Set(value);
        }

        #endregion
    }
}