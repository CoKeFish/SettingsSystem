#if UNITY_EDITOR

using Marmary.Utils.Editor.ModuleSymbols;
using UnityEditor;

namespace Marmary.SettingsSystem.Editor

{
    // Symbol registration for SettingsSystem third-party integrations.
    // Only runs when MODULE_SYMBOLS_SYSTEM_ENABLED is defined.

#if MODULE_SYMBOLS_SYSTEM_ENABLED

    /// <summary>
    ///     Static class responsible for registering symbols for the SettingsSystem integrations.
    ///     This code path runs only when the <c>MODULE_SYMBOLS_SYSTEM_ENABLED</c> symbol is defined.
    /// </summary>
    [InitializeOnLoad]
    public static class SettingsSystemRegister

    {
        #region Constructors and Injected

        static SettingsSystemRegister()

        {
            var desc = new ModuleSymbolDescriptor
            {
                ModuleName = "Settings System",
                Options = new[]
                {
                    new SymbolOption
                    {
                        symbol = "FMOD_MODULE_ENABLED",
                        description = "Enables the FMOD volume integration (FMODVolumeSettings).",
                        enabledByDefault = false
                    },
                    new SymbolOption
                    {
                        symbol = "I2_MODULE_ENABLED",
                        description = "Enables the I2 Localization language integration (I2LanguageSettings).",
                        enabledByDefault = false
                    }
                }
            };

            ModuleSymbolRegistry.Register(desc);
        }

        #endregion
    }

#endif
}

#endif