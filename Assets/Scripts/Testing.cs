using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] Unit unit;

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            unit.GetMoveAction().GetValidActionGridPosition();
        }
    }
}
