using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text turnNumberText;

    [SerializeField] private Button endTurnButton;


    private void Awake()
    {
        endTurnButton.onClick.AddListener(() => 
        {
            TurnSystem.Instance.NextTurn();
            UpdateTurnNumberText();
        });
    }

    private void Start()
    {
        UpdateTurnNumberText();
    }

    private void UpdateTurnNumberText()
    {
        turnNumberText.text = $"Turn: {TurnSystem.Instance.GetTurnNumber()}";
    }
}
