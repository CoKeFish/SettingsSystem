using Marmary.SaveSystem;

namespace Marmary.SettingsSystem
{
    /// <summary>
    ///     Abstract base class for configuring a specific type of application setting.
    ///     Provides a contract for setting, retrieving, and persisting a setting value.
    /// </summary>
    /// <typeparam name="T">The type of the setting value.</typeparam>
    public abstract class SettingsConfigureBase<T> : IAutoSave
    {
        #region Fields

        /// <summary>
        ///     The global save context used for persisting and retrieving settings.
        /// </summary>
        protected readonly SaveRepository<T> settingsRepository;

        /// <summary>
        ///     The factory default value for the setting, used to reset the configuration
        ///     to its out-of-the-box state.
        /// </summary>
        private readonly T _defaultValue;

        #endregion

        #region Constructors and Injected

        /// <summary>
        ///     Provides a base implementation for configuring application settings with a specific type.
        /// </summary>
        /// <param name="settingsRepository">Repository for saving and loading the setting value.</param>
        /// <param name="defaultValue">The factory default value the setting resets to.</param>
        protected SettingsConfigureBase(SaveRepository<T> settingsRepository, T defaultValue)
        {
            this.settingsRepository = settingsRepository;
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
        public virtual string GetCurrentSystemToString()
        {
            return GetCurrentSystem().ToString();
        }

        /// <summary>
        ///     Retrieves the current in-memory value of the setting as a string representation.
        /// </summary>
        /// <returns>A string representation of the current in-memory value of the setting.</returns>
        public virtual string GetCurrentMemoryToString()
        {
            return GetCurrentMemory().ToString();
        }

        /// <summary>
        ///     Resets the setting to its factory default value and persists the change to the save repository.
        /// </summary>
        public void ResetAndSave()
        {
            Reset();
            Save();
        }


        /// <summary>
        ///     Resets the setting to its factory default value.
        /// </summary>
        private void Reset()
        {
            Set(_defaultValue);
        }

        /// <summary>
        ///     Persists the current configuration state of the setting to the save repository.
        /// </summary>
        public void Save()
        {
            settingsRepository.Save();
        }

        #endregion

        /// <summary>
        /// Saves the current state of the settings to the repository.
        /// Implements the <see cref="IAutoSave"/> interface method to ensure automatic persistence of configuration changes.
        /// </summary>
        public void AutoSave()
        {
            Save();
        }
    }
}