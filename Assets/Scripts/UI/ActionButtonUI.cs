using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    private Button actionButton;
    private TMP_Text actionNameText;

    private void Awake()
    {
        actionButton = GetComponent<Button>();
        actionNameText = GetComponentInChildren<TMP_Text>();
    }

    public void SetBaseAction(BaseAction baseAction)
    {
        actionNameText.text = baseAction.GetActionName();

        actionButton.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
    }
}
