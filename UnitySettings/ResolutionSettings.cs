using System.Collections.Generic;
using System.Linq;
using Marmary.SaveSystem;
using UnityEngine;

namespace Marmary.SettingsSystem.UnitySettings
{
    /// <summary>
    ///     Manages screen resolution settings, providing available options and applying user selections.
    ///     Inherits from <see cref="SettingsConfigureBase{Vector2Int}" />.
    /// </summary>
    public sealed class ResolutionSettings : SettingsConfigureBase<Vector2Int>
    {
        #region Fields

        /// <summary>
        ///     List of available screen resolutions detected from the system.
        /// </summary>
        private readonly List<Vector2Int> _resolutionOptions;

        #endregion

        #region Constructors and Injected

        /// <summary>
        ///     Initializes a new instance of the <see cref="ResolutionSettings" /> class.
        ///     Detects available resolutions and sets the current resolution based on saved settings.
        /// </summary>
        /// <param name="settingsRepository">Repository for saving and loading settings data.</param>
        public ResolutionSettings(SaveRepositoryGeneric<Vector2Int> settingsRepository)
            : base(settingsRepository, ResolveDefault(settingsRepository))
        {
            _resolutionOptions = DetectAvailableResolutions();
            if (_resolutionOptions.Count == 0)
            {
                var current = Screen.currentResolution;
                _resolutionOptions.Add(new Vector2Int(current.width, current.height));
            }

            // If the saved value is not valid, initialize with the first available.
            var savedValue = settingsRepository.Value;
            if (!_resolutionOptions.Contains(savedValue)) savedValue = _resolutionOptions[0];

            Set(savedValue);
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Sets the screen resolution to the specified value.
        /// </summary>
        /// <param name="value">Resolution to set as <see cref="Vector2Int" />.</param>
        public override void Set(Vector2Int value)
        {
            Screen.SetResolution(value.x, value.y, Screen.fullScreen);
            SettingsRepository.Value = value;
        }

        /// <summary>
        ///     Parses a resolution from a string and sets it.
        ///     Expected format: "width X height".
        /// </summary>
        /// <param name="value">Resolution string.</param>
        public override void SetFromString(string value)
        {
            if (TryParse(value, out var resolution))
                Set(resolution);
            else
                Debug.LogError("Invalid resolution format. Expected 'width X height'.");
        }

        /// <summary>
        ///     Gets the current screen resolution.
        /// </summary>
        /// <returns>Current resolution as <see cref="Vector2Int" />.</returns>
        public override Vector2Int GetCurrentSystem()
        {
            return new Vector2Int(Screen.width, Screen.height);
        }

        /// <summary>
        ///     Retrieves the current memory value stored in the settings repository.
        /// </summary>
        /// <returns>The current value of type <see cref="Vector2Int" /> from the settings repository.</returns>
        public override Vector2Int GetCurrentMemory()
        {
            return SettingsRepository.Value;
        }

        /// <summary>
        ///     Gets the current screen resolution as a formatted string.
        /// </summary>
        /// <returns>Current resolution in the format "width X height".</returns>
        public override string GetCurrentSystenToString()
        {
            return Parse(GetCurrentSystem());
        }

        /// <summary>
        ///     Retrieves the current memory setting as a string representation.
        ///     Converts the current configuration value into a readable string format.
        /// </summary>
        /// <returns>The string representation of the current memory setting.</returns>
        public override string GetCurrentMemoryToString()
        {
            return Parse(GetCurrentMemory());
        }

        /// <summary>
        ///     Gets the list of available resolution options.
        /// </summary>
        /// <returns>List of available resolutions as <see cref="Vector2Int" />.</returns>
        public override List<Vector2Int> GetOptions()
        {
            return new List<Vector2Int>(_resolutionOptions);
        }

        /// <summary>
        ///     Gets the list of available resolution options as formatted strings.
        /// </summary>
        /// <returns>List of resolution strings in the format "width X height".</returns>
        public override List<string> GetOptionsToString()
        {
            return _resolutionOptions.Select(Parse).ToList();
        }

        /// <summary>
        ///     Attempts to parse a resolution string in the format "width X height" into a <see cref="Vector2Int" />.
        /// </summary>
        /// <param name="value">The resolution string to parse.</param>
        /// <param name="result">The parsed <see cref="Vector2Int" /> if successful; otherwise, <see cref="Vector2Int.zero" />.</param>
        /// <returns>True if parsing was successful; otherwise, false.</returns>
        private static bool TryParse(string value, out Vector2Int result)
        {
            result = Vector2Int.zero;
            if (string.IsNullOrWhiteSpace(value))
                return false;

            // Remove spaces and split by 'X' or 'x'
            var tokens = value.ToUpper().Split('X');
            if (tokens.Length == 2 &&
                int.TryParse(tokens[0].Trim(), out var width) &&
                int.TryParse(tokens[1].Trim(), out var height))
            {
                result = new Vector2Int(width, height);
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Determines the default screen resolution to be applied based on the available options
        ///     and the saved repository value.
        /// </summary>
        /// <param name="repository">The repository containing the saved resolution data.</param>
        /// <returns>A <see cref="Vector2Int" /> representing the default resolution to be used.</returns>
        private static Vector2Int ResolveDefault(SaveRepositoryGeneric<Vector2Int> repository)
        {
            var options = DetectAvailableResolutions();
            if (options.Count == 0)
            {
                var current = Screen.currentResolution;
                return new Vector2Int(current.width, current.height);
            }

            return options.Contains(repository.Value) ? repository.Value : options[0];
        }

        /// <summary>
        ///     Detects and returns a list of available screen resolutions.
        /// </summary>
        /// <returns>List of available resolutions as <see cref="Vector2Int" />.</returns>
        private static List<Vector2Int> DetectAvailableResolutions()
        {
            return Screen.resolutions.Select(r => new Vector2Int(r.width, r.height)).ToList();
        }

        /// <summary>
        ///     Converts a <see cref="Vector2Int" /> resolution to its string representation in the format "width X height".
        /// </summary>
        /// <param name="value">The resolution to convert.</param>
        /// <returns>The formatted resolution string.</returns>
        private static string Parse(Vector2Int value)
        {
            var result = $"{value.x} X {value.y}";
            return result;
        }

        #endregion
    }
}