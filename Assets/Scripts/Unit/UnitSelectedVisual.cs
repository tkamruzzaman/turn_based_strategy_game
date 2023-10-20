using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    private Unit unit;
    private Renderer _renderer;

    private void Awake()
    {
        unit = GetComponentInParent<Unit>();
        _renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnUnitSelectedUnitChanged;

        UpdateVisual();
    }

    private void UnitActionSystem_OnUnitSelectedUnitChanged(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (UnitActionSystem.Instance.GetSelectedUnit() == unit)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show() => _renderer.enabled = true;

    private void Hide() => _renderer.enabled = false;

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnUnitSelectedUnitChanged;
    }
}