using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitActionSysytemUI : MonoBehaviour
{
    [Header("Action Button")]
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainer;
    [Header("Action Point")]
    [SerializeField] private TMP_Text actionPointsText;

    private readonly List<ActionButtonUI> actionButtonList = new();

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnUnitSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
        
        CreateUnitActionButton();
        UpdateSelectedVisual();
        UpdateActionPointsText();
    }

    private void UnitActionSystem_OnUnitSelectedUnitChanged(object sender, System.EventArgs e)
    {
        CreateUnitActionButton();
        UpdateSelectedVisual();
        UpdateActionPointsText();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, System.EventArgs e)
    {
        UpdateSelectedVisual();
    }

    private void UnitActionSystem_OnActionStarted(object sender, System.EventArgs e)
    {
        UpdateActionPointsText();
    }

    private void CreateUnitActionButton()
    {
        foreach (Transform actionButtonTransform in actionButtonContainer)
        {
            Destroy(actionButtonTransform.gameObject);
        }
        actionButtonList.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            ActionButtonUI actionButtonUI = Instantiate(actionButtonPrefab, actionButtonContainer).GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);

            actionButtonList.Add(actionButtonUI);
        } 
    }

    private void UpdateSelectedVisual()
    {
        foreach (ActionButtonUI actionButtonUI in actionButtonList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }

    }

    private void UpdateActionPointsText()
    {
        int actionPointsAmount = UnitActionSystem.Instance.GetSelectedUnit().GetActionPoints();
        actionPointsText.text = $"Action Points: {actionPointsAmount}";
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnUnitSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged -= UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnActionStarted -= UnitActionSystem_OnActionStarted;

    }
}
