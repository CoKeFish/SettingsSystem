using VContainer;

namespace Marmary.SettingsSystem.UnityComponents
{
    /// <summary>
    ///     Controls a UI slider to adjust the application's sound-effects volume.
    /// </summary>
    public class SetSfxVolumeSlider : VolumeSliderBase
    {
        /// <summary>
        ///     Injects the keyed sound-effects volume settings dependency used to drive the slider.
        /// </summary>
        /// <param name="volumeSettings">The keyed settings instance for sound-effects volume configuration.</param>
        [Inject]
        private void Construct([Key(CommonSettings.SfxVolume)] SettingsConfigureBase<float> volumeSettings)
        {
            Bind(volumeSettings);
        }
    }
}