using Marmary.Utils.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Marmary.SettingsSystem.UnityComponents
{
    /// <summary>
    ///     Base behaviour binding a UI slider to a float volume setting: initializes the
    ///     slider from the stored value and forwards slider changes to the setting.
    ///     Subclasses only provide the injection of the concrete keyed setting via
    ///     <see cref="Bind" />.
    /// </summary>
    [RequireComponent(typeof(Slider))]
    public abstract class VolumeSliderBase : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///     Reference to the bound volume settings configuration.
        /// </summary>
        private SettingsConfigureBase<float> _volumeSettings;

        /// <summary>
        ///     Reference to the UI Slider component.
        /// </summary>
        private Slider _slider;

        #endregion

        #region Methods

        /// <summary>
        ///     Binds the injected volume setting this slider drives. Call from the subclass's
        ///     injection method.
        /// </summary>
        /// <param name="volumeSettings">The keyed settings instance to drive.</param>
        protected void Bind(SettingsConfigureBase<float> volumeSettings)
        {
            _volumeSettings = volumeSettings;
        }

        #endregion

        #region Unity Event Functions

        /// <summary>
        ///     Unity event function called on script initialization.
        ///     Initializes the slider reference, sets the initial slider value, and subscribes to value changes.
        /// </summary>
        [IgnoreUnityLifecycle]
        private void Start()
        {
            _slider = GetComponent<Slider>();

            _slider.value = _volumeSettings.GetCurrentMemory();

            _slider.onValueChanged.AddListener(OnValueChanged);
        }

        /// <summary>
        ///     Unity event function called when the object is destroyed.
        ///     Unsubscribes from the slider value changed event.
        /// </summary>
        private void OnDestroy()
        {
            if (_slider) _slider.onValueChanged.RemoveListener(OnValueChanged);
        }

        #endregion

        #region Event Functions

        /// <summary>
        ///     Event handler called when the slider value changes.
        ///     Updates the bound volume setting accordingly.
        /// </summary>
        /// <param name="value">The new value of the slider.</param>
        private void OnValueChanged(float value)
        {
            _volumeSettings.Set(value);
        }

        #endregion
    }
}