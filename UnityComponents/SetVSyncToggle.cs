using Marmary.HellmenRaaun.Application.Global;
using Marmary.HellmenRaaun.Application.Settings;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Marmary.Libraries.Settings
{
    /// <summary>
    ///     Handles the binding between a UI Toggle and the application's VSync setting.
    ///     Synchronizes the toggle state with the VSync setting and updates the setting when the toggle changes.
    /// </summary>
    public class SetVSyncToggle : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///     Reference to the UI Toggle component.
        /// </summary>
        private Toggle _toggle;

        /// <summary>
        ///     Reference to the VSync settings manager.
        /// </summary>
        private VSyncSettings _vSyncSettings;

        #endregion

        /// <summary>
        ///     Injects the <see cref="VSyncSettings" /> dependency.
        /// </summary>
        /// <param name="settingsManager"> The settings manager containing the VSync settings.</param>
        [Inject]
        public void Construct(SettingsManager settingsManager)
        {
            _vSyncSettings = settingsManager.VSyncSettings as VSyncSettings;
        }

        #region Unity Event Functions

        /// <summary>
        ///     Unity event function called on script initialization.
        ///     Initializes the toggle reference, sets the initial VSync state, and subscribes to toggle changes.
        /// </summary>
        private void Start()
        {
            _toggle = GetComponent<Toggle>();
            _toggle.isOn = _vSyncSettings.GetCurrent();
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