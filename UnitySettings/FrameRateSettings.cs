using System;
using System.Collections.Generic;
using System.Linq;
using Marmary.SaveSystem;
using UnityEngine;

namespace Marmary.SettingsSystem.UnitySettings
{
    /// <summary>
    ///     Manages dynamic frame rate settings based on available hardware options.
    ///     Detects supported frame rates and allows setting and retrieving the current frame rate.
    /// </summary>
    internal class FrameRateSettings : SettingsConfigureBase<int>
    {
        #region Fields

        /// <summary>
        ///     List of available frame rate options detected from the hardware.
        /// </summary>
        private readonly List<int> _frameRateOptions;

        #endregion

        #region Constructors and Injected

        /// <summary>
        ///     Initializes a new instance of <see cref="FrameRateSettings" />, detecting possible hardware options.
        ///     If the saved frame rate is invalid, initializes with the first available option.
        /// </summary>
        /// <param name="settingsRepository">Repository for saving and loading settings data.</param>
        public FrameRateSettings(SaveRepositoryGeneric<int> settingsRepository)
            : base(settingsRepository, ResolveDefault(settingsRepository))
        {
            _frameRateOptions = DetectAvailableFrameRates();
            // If the saved value is not valid, initialize with the first available.
            Set(!_frameRateOptions.Contains(settingsRepository.Value)
                ? _frameRateOptions.First() //Set default?
                : settingsRepository.Value);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Sets the frame rate to the specified value if it is available; otherwise, uses the first available option.
        ///     Updates the settings repository and saves the data.
        /// </summary>
        /// <param name="value">The desired frame rate value.</param>
        public sealed override void Set(int value)
        {
            var frameRateToSet = _frameRateOptions.Contains(value) ? value : _frameRateOptions.First();
            Application.targetFrameRate = frameRateToSet;
            SettingsRepository.Value = frameRateToSet;
        }

        /// <summary>
        ///     Sets the frame rate from a string value, parsing it to an integer.
        ///     Logs an error if the value is invalid.
        /// </summary>
        /// <param name="value">String representation of the desired frame rate.</param>
        public override void SetFromString(string value)
        {
            if (int.TryParse(value, out var frameRate))
                Set(frameRate);
            else
                Debug.LogError("Invalid frame rate value, ignoring.");
        }

        /// <summary>
        ///     Gets the current target frame rate set in the application.
        /// </summary>
        /// <returns>The current target frame rate.</returns>
        public override int GetCurrentSystem()
        {
            return Application.targetFrameRate;
        }

        /// <summary>
        ///     Retrieves the current memory value stored in the settings repository.
        /// </summary>
        /// <returns>
        ///     The current memory value as an integer.
        /// </returns>
        public override int GetCurrentMemory()
        {
            return SettingsRepository.Value;
        }

        /// <summary>
        ///     Retrieves the current system setting and converts it to its string representation.
        /// </summary>
        /// <returns>A string representing the current system setting.</returns>
        public override string GetCurrentSystenToString()
        {
            return GetCurrentSystem().ToString();
        }

        /// <summary>
        ///     Retrieves the current memory value as a string representation.
        /// </summary>
        /// <returns>
        ///     A string representation of the current memory value.
        /// </returns>
        public override string GetCurrentMemoryToString()
        {
            return GetCurrentMemory().ToString();
        }

        /// <summary>
        ///     Gets a list of available frame rate options.
        /// </summary>
        /// <returns>List of available frame rates.</returns>
        public override List<int> GetOptions()
        {
            return new List<int>(_frameRateOptions);
        }

        /// <summary>
        ///     Gets a list of available frame rate options as strings.
        /// </summary>
        /// <returns>List of available frame rates as string values.</returns>
        public override List<string> GetOptionsToString()
        {
            return _frameRateOptions.Select(fr => fr.ToString()).ToList();
        }

        /// <summary>
        ///     Resolves the default frame rate to be used when no valid existing value is found in the repository.
        ///     Determines the appropriate default from detected available options and repository data.
        /// </summary>
        /// <param name="repository">Repository that stores the saved frame rate value.</param>
        /// <returns>The resolved default frame rate, either from the repository or from the available options.</returns>
        private static int ResolveDefault(SaveRepositoryGeneric<int> repository)
        {
            var options = DetectAvailableFrameRates();
            if (options.Count == 0) return repository.Value;

            return options.Contains(repository.Value) ? repository.Value : options.First();
        }

        /// <summary>
        ///     Detects the unique frame rates supported by the system by inspecting available screen resolutions.
        /// </summary>
        /// <returns>Ordered list of unique supported frame rates.</returns>
        private static List<int> DetectAvailableFrameRates()
        {
            var frameRates = new HashSet<int>();
            foreach (var res in Screen.resolutions)
            {
#if UNITY_2022_2_OR_NEWER
                frameRates.Add((int)Math.Round(res.refreshRateRatio.value));
#else
                frameRates.Add(res.refreshRate);
#endif
            }

            return frameRates.OrderBy(x => x).ToList();
        }

        #endregion
    }
}