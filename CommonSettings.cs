namespace Marmary.SettingsSystem
{
    /// <summary>
    ///     Identifies the universal settings shipped with this library — options virtually
    ///     every game exposes. Used as the DI key to resolve the matching settings instance
    ///     and to compose its save key.
    /// </summary>
    /// <remarks>
    ///     Game-specific settings do NOT belong here: each game declares its own enum and
    ///     registers it through the same machinery (DI keys are objects, so different enum
    ///     types never collide). Convention: do not repeat member names across enums, as
    ///     the save key is composed from the member name.
    /// </remarks>
    public enum CommonSettings
    {
        /// <summary>
        ///     Represents the setting type used to configure the frame rate of the application.
        /// </summary>
        FrameRate,

        /// <summary>
        ///     Represents the fullscreen setting of the application.
        ///     This value determines whether the application runs in fullscreen mode.
        /// </summary>
        FullScreen,

        /// <summary>
        ///     Represents the setting type used to configure the language preferences of the application.
        /// </summary>
        Language,

        /// <summary>
        ///     Represents the setting type used to configure the resolution of the application.
        ///     Resolution determines the dimensions of the application's display in terms of width and height.
        /// </summary>
        Resolution,

        /// <summary>
        ///     Represents the setting type used to enable or disable vertical synchronization (VSync).
        ///     VSync helps to synchronize the application's frame rate with the display's refresh rate,
        ///     minimizing visual artifacts like screen tearing.
        /// </summary>
        VSync,

        /// <summary>
        ///     Represents the setting type for controlling the master volume level in the system.
        ///     This setting typically adjusts the overall audio output for the application.
        /// </summary>
        MasterVolume,

        /// <summary>
        ///     Represents the setting type for controlling the sound-effects volume level,
        ///     independent from music and master volume.
        /// </summary>
        SfxVolume
    }
}