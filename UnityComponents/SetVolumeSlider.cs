using VContainer;

namespace Marmary.SettingsSystem.UnityComponents
{
    /// <summary>
    ///     Controls a UI slider to adjust the application's master volume.
    /// </summary>
    public class SetVolumeSlider : VolumeSliderBase
    {
        /// <summary>
        ///     Injects the keyed master volume settings dependency used to drive the slider.
        /// </summary>
        /// <param name="volumeSettings">The keyed settings instance for master volume configuration.</param>
        [Inject]
        private void Construct([Key(CommonSettings.MasterVolume)] SettingsConfigureBase<float> volumeSettings)
        {
            Bind(volumeSettings);
        }
    }
}