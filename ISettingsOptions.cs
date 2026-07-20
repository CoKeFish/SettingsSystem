using System.Collections.Generic;

namespace Marmary.SettingsSystem
{
    /// <summary>
    ///     Optional contract for settings that expose a discrete list of selectable options
    ///     (e.g. resolutions, frame rates, languages). Settings without an enumerable option
    ///     set (e.g. a volume slider) simply do not implement it.
    /// </summary>
    /// <typeparam name="T">The type of the setting value.</typeparam>
    public interface ISettingsOptions<T>
    {
        /// <summary>
        ///     Gets the list of available options for the setting.
        /// </summary>
        /// <returns>A list of available options of type <typeparamref name="T" />.</returns>
        List<T> GetOptions();

        /// <summary>
        ///     Gets the list of available options for the setting as strings.
        /// </summary>
        /// <returns>A list of available options as strings.</returns>
        List<string> GetOptionsToString();
    }
}