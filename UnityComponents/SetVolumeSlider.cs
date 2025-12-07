using Marmary.Utils.Runtime;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Marmary.SettingsSystem.UnityComponents
{

    /// <summary>
    ///     Controls a UI slider to adjust the application's master volume.
    ///     Binds the slider value to the master volume setting and updates the setting when the slider changes.
    /// </summary>
    public class SetVolumeSlider : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///     Reference to the injected master volume settings configuration.
        /// </summary>
        private SettingsConfigureBase<float> _volumeSettings;

        /// <summary>
        ///     Reference to the UI Slider component.
        /// </summary>
        private Slider _slider;

        #endregion

        #region Constructors and Injected

        /// <summary>
        ///     Injects the keyed master volume settings dependency used to drive the slider.
        /// </summary>
        /// <param name="volumeSettings">The keyed settings instance for master volume configuration.</param>
        [Inject]
        private void Construct([Key(SettingsType.MasterVolume)] SettingsConfigureBase<float> volumeSettings)
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
            _slider.onValueChanged.RemoveListener(OnValueChanged);
        }

        #endregion

        #region Event Functions

        /// <summary>
        ///     Event handler called when the slider value changes.
        ///     Updates the music volume setting accordingly.
        /// </summary>
        /// <param name="value">The new value of the slider.</param>
        private void OnValueChanged(float value)
        {
            _volumeSettings.Set(value);
        }

        #endregion
    }
}