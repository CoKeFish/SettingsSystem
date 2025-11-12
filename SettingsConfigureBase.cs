using System.Collections.Generic;
using Marmary.SaveSystem;

namespace Marmary.SettingsSystem
{
    /// <summary>
    ///     Abstract base class for configuring a specific type of application setting.
    ///     Provides a contract for setting, retrieving, and listing available options for a setting.
    /// </summary>
    /// <typeparam name="T">The type of the setting value.</typeparam>
    public abstract class SettingsConfigureBase<T>
    {
        #region Fields

        /// <summary>
        ///     The global save context used for persisting and retrieving settings.
        /// </summary>
        protected readonly SaveRepositoryGeneric<T> SettingsRepository;

        /// <summary>
        ///     The default value for the setting, used to initialize or reset the configuration
        ///     when no specific value has been provided by the user.
        /// </summary>
        private readonly T _defaultValue;

        #endregion

        #region Constructors and Injected

        /// <summary>
        ///     Provides a base implementation for configuring application settings with a specific type.
        /// </summary>
        /// <typeparam name="T">The type of the setting to configure.</typeparam>
        protected SettingsConfigureBase(SaveRepositoryGeneric<T> settingsRepository, T defaultValue)
        {
            SettingsRepository = settingsRepository;
            _defaultValue = defaultValue;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Sets the setting to the specified value.
        /// </summary>
        /// <param name="value">The value to set.</param>
        public abstract void Set(T value);

        /// <summary>
        ///     Sets the setting to the specified value and saves the updated value to persistent storage.
        /// </summary>
        /// <param name="value">The value to set and save.</param>
        public void SetAndSave(T value)
        {
            Set(value);
            Save();
        }

        /// <summary>
        ///     Sets the setting from a string value.
        /// </summary>
        /// <param name="value">The string value to set.</param>
        public abstract void SetFromString(string value);

        /// <summary>
        ///     Parses the provided string and sets the setting value, then persists the updated value to storage.
        /// </summary>
        /// <param name="value">The string representation of the value to parse, set, and save.</param>
        public void SetAndSaveFromString(string value)
        {
            SetFromString(value);
            Save();
        }

        /// <summary>
        ///     Retrieves the current system-stored value of the setting.
        /// </summary>
        /// <returns>The current system-stored value of the setting.</returns>
        public abstract T GetCurrentSystem();

        /// <summary>
        ///     Retrieves the current value of the setting from memory.
        /// </summary>
        /// <returns>The current memory-stored value of the setting.</returns>
        public abstract T GetCurrentMemory();


        /// <summary>
        ///     Retrieves the current system setting value as a string representation.
        /// </summary>
        /// <returns>A string representation of the current system setting value.</returns>
        public abstract string GetCurrentSystenToString();

        /// <summary>
        ///     Retrieves the current in-memory value of the setting as a string representation.
        /// </summary>
        /// <returns>A string representation of the current in-memory value of the setting.</returns>
        public abstract string GetCurrentMemoryToString();

        /// <summary>
        ///     Gets the list of available options for the setting.
        /// </summary>
        /// <returns>A list of available options of type <typeparamref name="T" />.</returns>
        public abstract List<T> GetOptions();

        /// <summary>
        ///     Gets the list of available options for the setting as strings.
        /// </summary>
        /// <returns>A list of available options as strings.</returns>
        public abstract List<string> GetOptionsToString();

        /// <summary>
        ///     Resets the setting to its default value and persists the change to the save repository.
        /// </summary>
        public void ResetAndSave()
        {
            Reset();
            Save();
        }


        /// <summary>
        ///     Resets the setting to its default value.
        /// </summary>
        private void Reset()
        {
            Set(_defaultValue);
        }

        /// <summary>
        ///     Persists the current configuration state of the setting to the save repository.
        /// </summary>
        private void Save()
        {
            SettingsRepository.SaveData();
        }

        #endregion
    }
}