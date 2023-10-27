using System;
using UnityEngine;

[SelectionBase]
public class InteractSphere : MonoBehaviour, IInteractable
{
    private const float INTERACTION_TIME = 0.5f;

    [SerializeField] private Material redMaterial;
    [SerializeField] private Material greenMaterial;

    private Renderer meshRenderer;
    private GridPosition gridPosition;
    private bool isRed;
    private Action onInteractionComplete;
    private bool isActive;
    private float interactTimer;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<Renderer>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);

        SetColorRed();
    }

    private void Update()
    {
        if (!isActive) { return; }

        interactTimer -= Time.deltaTime;
        if (interactTimer <= 0f)
        {
            isActive = false;
            onInteractionComplete?.Invoke();
        }
    }

    private void SetColorRed()
    {
        isRed = true;
        meshRenderer.material = redMaterial;
    }

    private void SetColorGreen()
    {
        isRed = false;  
        meshRenderer.material = greenMaterial;
    }

    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        interactTimer = INTERACTION_TIME;

        if (isRed)
        {
            SetColorGreen();
        }
        else
        {
            SetColorRed();
        }

    }
}
