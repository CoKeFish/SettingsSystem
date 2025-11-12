using System.Collections.Generic;
using Marmary.HellmenRaaun.Application.Save;
using Marmary.HellmenRaaun.Domain;

namespace Marmary.HellmenRaaun.Core
{
    /// <summary>
    /// Abstract base class for configuring a specific type of application setting.
    /// Provides a contract for setting, retrieving, and listing available options for a setting.
    /// </summary>
    /// <typeparam name="T">The type of the setting value.</typeparam>
    public abstract class SettingsConfigureBase<T>
    {
        /// <summary>
        /// The global save context used for persisting and retrieving settings.
        /// </summary>
        protected SaveRepositoryGeneric<SettingsData> SettingsRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsConfigureBase{T}"/> class.
        /// </summary>
        /// <param name="settingsRepository">The global save context to use for settings operations.</param>
        protected SettingsConfigureBase(SaveRepositoryGeneric<SettingsData> settingsRepository)
        {
            SettingsRepository = settingsRepository;
        }

        /// <summary>
        /// Sets the setting to the specified value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public abstract void Set(T value);

        /// <summary>
        /// Sets the setting from a string value.
        /// </summary>
        /// <param name="value">The string value to set.</param>
        public abstract void SetFromString(string value);

        /// <summary>
        /// Gets the current value of the setting.
        /// </summary>
        /// <returns>The current value of the setting.</returns>
        public abstract T GetCurrent();
        
        /// <summary>
        /// Gets the current value of the setting as a string.
        /// </summary>
        public abstract string GetCurrentToString();

        /// <summary>
        /// Gets the list of available options for the setting.
        /// </summary>
        /// <returns>A list of available options of type <typeparamref name="T"/>.</returns>
        public abstract List<T> GetOptions();

        /// <summary>
        /// Gets the list of available options for the setting as strings.
        /// </summary>
        /// <returns>A list of available options as strings.</returns>
        public abstract List<string> GetOptionsToString();
    }
}