using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSysytemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainer;
    
    private void Start()
    {
        CreateUnitActionButton();
        UnitActionSystem.Instance.OnUnitSelectedUnitChanged += UnitActionSystem_OnUnitSelectedUnitChanged;
    }

    private void UnitActionSystem_OnUnitSelectedUnitChanged(object sender, System.EventArgs e)
    {
        CreateUnitActionButton();
    }

    private void CreateUnitActionButton()
    {
        foreach (Transform actionButtonTransform in actionButtonContainer)
        {
            Destroy(actionButtonTransform.gameObject);
        }

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            ActionButtonUI actionButtonUI = Instantiate(actionButtonPrefab, actionButtonContainer).GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);
        } 
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnUnitSelectedUnitChanged -= UnitActionSystem_OnUnitSelectedUnitChanged;
    }
}
