using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TMP_Text actionPointText;
    [SerializeField] private Image healthBarImage;

    private Unit unit;
    private HealthSystem healthSystem;

    private void Awake()
    {
        unit = GetComponentInParent<Unit>();
        healthSystem = GetComponentInParent<HealthSystem>();
    }

    private void Start()
    {
        unit.OnActionPointChanged += Unit_OnActionPointChanged;
        healthSystem.OnDamaged += HealthSystem_OnDamaged;

        UpdateActionPointsText();
        UpdateHealthBar();
    }

    private void Unit_OnActionPointChanged(object sender, EventArgs e)
    {
        UpdateActionPointsText();
    }
    private void HealthSystem_OnDamaged(object sender, EventArgs e)
    {
        UpdateHealthBar();
    }

    private void UpdateActionPointsText()
    {
        actionPointText.text = unit.GetActionPoints().ToString();
    }

    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = healthSystem.GetHealthNormalized();
    }

    private void OnDestroy()
    {
        unit.OnActionPointChanged -= Unit_OnActionPointChanged;
    }
}
