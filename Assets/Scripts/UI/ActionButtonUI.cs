using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    private Button actionButton;
    private TMP_Text actionNameText;
    [SerializeField] private Image selectionImage;

    private BaseAction baseAction;

    private void Awake()
    {
        actionButton = GetComponent<Button>();
        actionNameText = GetComponentInChildren<TMP_Text>();
    }

    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;

        actionNameText.text = baseAction.GetActionName();

        actionButton.onClick.AddListener(() =>
        {
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
        });
    }

    public void UpdateSelectedVisual()
    {
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();

        selectionImage.gameObject.SetActive(baseAction == selectedBaseAction);
    }
}
