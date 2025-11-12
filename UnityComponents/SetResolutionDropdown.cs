using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

//TODO: DOCUMENT THIS CLASS

namespace Marmary.SettingsSystem.UnityComponents
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class SetResolutionDropdown : MonoBehaviour
    {
        #region Fields

        private List<Resolution> _resolutionsOptions;

        #endregion

        #region Unity Event Functions

        private void Awake()
        {
            Debug.Log("SetResolutionDropdown Awake");
            var dropdown = GetComponent<TMP_Dropdown>();
            _resolutionsOptions = new List<Resolution>();

            // Filtrar las resoluciones para evitar duplicados
            _resolutionsOptions = Screen.resolutions
                .GroupBy(res => new { res.width, res.height })
                .Select(group => group.First())
                .ToList();

            var resolutionOptions = _resolutionsOptions
                .Select(res => $"{res.height} X {res.width}")
                .ToList();

            foreach (var option in resolutionOptions) Debug.Log(option);

            // Fill the dropdown elements
            dropdown.ClearOptions();
            dropdown.AddOptions(resolutionOptions);

            dropdown.value =
                resolutionOptions.IndexOf(Screen.currentResolution.height + " X " + Screen.currentResolution.width);
            dropdown.onValueChanged.RemoveListener(OnValueChanged);
            dropdown.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnEnable()
        {
            var dropdown = GetComponent<TMP_Dropdown>();
            _resolutionsOptions = new List<Resolution>();

            // Filtrar las resoluciones para evitar duplicados
            _resolutionsOptions = Screen.resolutions
                .GroupBy(res => new { res.width, res.height })
                .Select(group => group.First())
                .ToList();

            var resolutionOptions = _resolutionsOptions
                .Select(res => $"{res.height} X {res.width}")
                .ToList();

            // Fill the dropdown elements
            dropdown.ClearOptions();
            dropdown.AddOptions(resolutionOptions);

            dropdown.value =
                resolutionOptions.IndexOf(Screen.currentResolution.height + " X " + Screen.currentResolution.width);
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

            Screen.SetResolution(_resolutionsOptions[index].width, _resolutionsOptions[index].height,
                Screen.fullScreen);
        }

        #endregion
    }
}