using Marmary.HellmenRaaun.Application.Global;
using Marmary.HellmenRaaun.Application.Settings;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Marmary.Libraries.Settings
{
    
    //TODO: Rename to SetMusicVolumeSlider
    
    /// <summary>
    /// Controls a UI slider to adjust the application's music volume.
    /// Binds the slider value to the music volume setting and updates the setting when the slider changes.
    /// </summary>
    public class SliderSoundController : MonoBehaviour
    {
        /// <summary>
        /// Reference to the music volume settings manager.
        /// </summary>
        private MusicVolumeSettings _musicVolumeSettings;

        /// <summary>
        /// Injects the <see cref="SettingsManager"/> dependency and retrieves the <see cref="MusicVolumeSettings"/>.
        /// </summary>
        /// <param name="settingsManager">The settings manager containing music volume settings.</param>
        [Inject]
        private void Construct(SettingsManager settingsManager)
        {
            _musicVolumeSettings = settingsManager.MusicVolumeSettings as MusicVolumeSettings;
        }

        #region Fields

        /// <summary>
        /// Reference to the UI Slider component.
        /// </summary>
        private Slider _slider;

        #endregion

        #region Unity Event Functions

        /// <summary>
        /// Unity event function called on script initialization.
        /// Initializes the slider reference, sets the initial slider value, and subscribes to value changes.
        /// </summary>
        private void Start()
        {
            _slider = GetComponent<Slider>();

            _slider.value = _musicVolumeSettings.GetCurrent();

            _slider.onValueChanged.AddListener(OnValueChanged);
        }

        /// <summary>
        /// Unity event function called when the object is destroyed.
        /// Unsubscribes from the slider value changed event.
        /// </summary>
        private void OnDestroy()
        {
            _slider.onValueChanged.RemoveListener(OnValueChanged);
        }

        #endregion

        #region Event Functions

        /// <summary>
        /// Event handler called when the slider value changes.
        /// Updates the music volume setting accordingly.
        /// </summary>
        /// <param name="value">The new value of the slider.</param>
        private void OnValueChanged(float value)
        {
            _musicVolumeSettings.Set(value);
        }

        #endregion
    }
}