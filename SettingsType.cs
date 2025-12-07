namespace Marmary.SettingsSystem
{
    /// <summary>
    ///     Represents the setting type for vertical synchronization (VSync).
    ///     VSync is used to control the synchronization of the frame rate with the refresh rate
    ///     of the display to reduce screen tearing.
    /// </summary>
    public enum SettingsType
    {
        /// <summary>
        ///     Represents the setting type used to configure the frame rate of an application.
        /// </summary>
        FrameRate,

        /// <summary>
        ///     Represents the fullscreen setting of the application.
        ///     This value determines whether the application runs in fullscreen mode.
        /// </summary>
        Fullscreen,

        /// <summary>
        ///     Represents the setting type used to configure the language preferences of an application.
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
        MasterVolume
    }
}