using System.Collections;
using System.Collections.Generic;
using IntoTheCrypt.Models;
using TMPro;
using UnityEngine;

public class StatMenuController : MonoBehaviour
{
    #region Public

    #region Members
    [Tooltip("Health text")]
    public TextMeshProUGUI HealthText;
    [Tooltip("Strength text")]
    public TextMeshProUGUI StrengthText;
    [Tooltip("Dexterity text")]
    public TextMeshProUGUI DexterityText;
    #endregion

    #region Member Methods
    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    public void ToggleActive()
    {
        SetActive(!gameObject.activeSelf);
    }

    public void UpdateStats(Stats stats)
    {
        HealthText.text = $"{stats.HP}/{stats.MaxHP}";
        StrengthText.text = $"{stats.Strength}";
        DexterityText.text = $"{stats.Dexterity}";
    }
    #endregion

    #endregion

    #region Private

    #region Member Methods
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
    private void Start()
    {
        
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "<Pending>")]
    private void Update()
    {
        
    }
    #endregion

    #endregion
}
