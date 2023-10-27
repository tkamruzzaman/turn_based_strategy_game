using System;
using UnityEngine;

[SelectionBase]
public class Door : MonoBehaviour, IInteractable
{
    public static event EventHandler<bool> OnAnyInteractableStatusChanged;

    private const string ANIM_PARAM_IS_OPEN = "IsOpen";
    private const float INTERACTION_TIME = 0.9f;

    [SerializeField] private bool isOpen;

    private GridPosition gridPosition;
    private Animator animator;
    private Action onInteractionComplete;
    private bool isActive;
    private float interactTimer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);

        Invoke(nameof(UpdateDoorInitialStatus), 0.15f);
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

    private void UpdateDoorInitialStatus()
    {
        if (isOpen) { OpenDoor(); }
        else { CloseDoor(); }
    }

    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        interactTimer = INTERACTION_TIME;

        if (isOpen) { CloseDoor(); }
        else { OpenDoor(); }
    }

    private void OpenDoor()
    {
        isOpen = true;
        animator.SetBool(ANIM_PARAM_IS_OPEN, isOpen);
        OnAnyInteractableStatusChanged?.Invoke(this, isOpen);
    }

    private void CloseDoor()
    {
        isOpen = false;
        animator.SetBool(ANIM_PARAM_IS_OPEN, isOpen);
        OnAnyInteractableStatusChanged?.Invoke(this, isOpen);
    }

    public GridPosition GetGridPosition() => gridPosition;


}
