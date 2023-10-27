using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance {  get; private set; }  

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one InputManager" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public bool IsMouseButtonDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    public Vector2 GetMouseScreenPosition()
    {
        return Input.mousePosition;
    }

    public Vector2 GetCameraMoveVector()
    {
        Vector2 inputMoveDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDirection.y = +1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDirection.y = -1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDirection.x = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDirection.x = +1f;
        }

        return inputMoveDirection;
    }

    public Vector3 GetCameraRotateVector()
    {
        Vector3 rotationVector = Vector3.zero;

        if (Input.GetKey(KeyCode.Q))
        {
            rotationVector.y = +1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            rotationVector.y = -1f;
        }

        return rotationVector;
    }

    public float GetCameraZoomAmount()
    {
        float zoomAmount = 0;

        if (Input.mouseScrollDelta.y > 0)
        {
            zoomAmount = -1f;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            zoomAmount = +1f;
        }

        return zoomAmount;
    } 
}
