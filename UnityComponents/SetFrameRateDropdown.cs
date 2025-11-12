using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Marmary.Libraries.Settings
{
    // TODO: Refactor this class to use a settings repository for persistence
    // TODO: DOCUMENT THIS CLASS
    public class SetFrameRateDropdown : MonoBehaviour
    {
        #region Fields

        private List<RefreshRate> _refreshRates;

        #endregion

        #region Unity Event Functions

        private void Awake()
        {
            var dropdown = GetComponent<TMP_Dropdown>();
            _refreshRates = new List<RefreshRate>();

            // Filtrar las resoluciones para evitar duplicados

            foreach (var resolution in Screen.resolutions) _refreshRates.Add(resolution.refreshRateRatio);

            var resolutionOptions = _refreshRates
                .Select(res => $"{res}")
                .ToList();

            // Fill the dropdown elements
            dropdown.ClearOptions();
            dropdown.AddOptions(resolutionOptions);

            dropdown.value = resolutionOptions.IndexOf(Screen.currentResolution.refreshRateRatio + "");
            dropdown.onValueChanged.RemoveListener(OnValueChanged);
            dropdown.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnEnable()
        {
            var dropdown = GetComponent<TMP_Dropdown>();
            _refreshRates = new List<RefreshRate>();

            // Filtrar las resoluciones para evitar duplicados

            foreach (var resolution in Screen.resolutions)
            {
                var alreadyExists = false;

                foreach (var rate in _refreshRates)
                    if ((int)Math.Round(rate.value) == (int)Math.Round(resolution.refreshRateRatio.value))
                    {
                        alreadyExists = true;
                        break; // No es necesario seguir buscando
                    }

                if (!alreadyExists) _refreshRates.Add(resolution.refreshRateRatio);
            }

            //Foreach debug
            foreach (var rate in _refreshRates) Debug.Log("Frame rate " + rate.value);

            var resolutionOptions = _refreshRates
                .Select(res => $"{(int)Math.Round(res.value)}")
                .ToList();

            // Fill the dropdown elements
            dropdown.ClearOptions();
            dropdown.AddOptions(resolutionOptions);

            dropdown.value = resolutionOptions.IndexOf(Screen.currentResolution.refreshRateRatio + "");
            dropdown.onValueChanged.RemoveListener(OnValueChanged);
            dropdown.onValueChanged.AddListener(OnValueChanged);
        }

        #endregion

        #region Event Functions

        private void OnValueChanged(int index)
        {
            var dropdown = GetComponent<TMP_Dropdown>();
            if (index < 0)
            {
                index = 0;
                dropdown.value = index;
            }

            UnityEngine.Application.targetFrameRate = (int)Math.Round(_refreshRates[index].value);
        }

        #endregion
    }
}