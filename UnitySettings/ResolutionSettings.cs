using System.Collections.Generic;
using System.Linq;
using Marmary.HellmenRaaun.Application.Save;
using Marmary.HellmenRaaun.Core;
using Marmary.HellmenRaaun.Domain;
using UnityEngine;

namespace Marmary.HellmenRaaun.Application.Settings
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

        /// <summary>
        ///     Initializes a new instance of the <see cref="ResolutionSettings" /> class.
        ///     Detects available resolutions and sets the current resolution based on saved settings.
        /// </summary>
        /// <param name="settingsRepository">Repository for saving and loading settings data.</param>
        public ResolutionSettings(SaveRepositoryGeneric<SettingsData> settingsRepository) : base(settingsRepository)
        {
            _resolutionOptions = DetectAvailableResolutions();
            // If the saved value is not valid, initialize with the first available.
            Set(!_resolutionOptions.Contains(settingsRepository.Value.Resolution)
                ? _resolutionOptions[0]
                : settingsRepository.Value.Resolution);
        }

        #region Methods

        /// <summary>
        ///     Sets the screen resolution to the specified value.
        /// </summary>
        /// <param name="value">Resolution to set as <see cref="Vector2Int" />.</param>
        public override void Set(Vector2Int value)
        {
            Screen.SetResolution(value.x, value.y, Screen.fullScreen);
            SettingsRepository.Value.Resolution = value;
            SettingsRepository.SaveData();
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
        public override Vector2Int GetCurrent()
        {
            return new Vector2Int(Screen.width, Screen.height);
        }

        /// <summary>
        ///     Gets the current screen resolution as a formatted string.
        /// </summary>
        /// <returns>Current resolution in the format "width X height".</returns>
        public override string GetCurrentToString()
        {
            var currentResolution = GetCurrent();
            return Parse(currentResolution);
        }

        /// <summary>
        ///     Gets the list of available resolution options.
        /// </summary>
        /// <returns>List of available resolutions as <see cref="Vector2Int" />.</returns>
        public override List<Vector2Int> GetOptions()
        {
            return _resolutionOptions;
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