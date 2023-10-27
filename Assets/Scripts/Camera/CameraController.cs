using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private const float MIN_FOLLOW_Y_OFFSET = 2.0f;
    private const float MAX_FOLLOW_Y_OFFSET = 12.0f;

    private CinemachineTransposer cinemachineTransposer;
    private Vector3 targetFollowOffset;

    private CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();

        cinemachineTransposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    }

    private void Update()
    {
        HandleMovement();

        HandleRotation();

        HandleZoom();
    }


    private void HandleMovement()
    {
        Vector2 inputMoveDirection = InputManager.Instance.GetCameraMoveVector();
        float moveSpeed = 10f;
        Vector3 moveVector = transform.forward * inputMoveDirection.y
                + transform.right * inputMoveDirection.x;
        transform.position += moveSpeed * Time.deltaTime * moveVector;
    }

    private void HandleRotation()
    {
        Vector3 rotationVector = InputManager.Instance.GetCameraRotateVector();
        float rotationSpeed = 100f;
        transform.eulerAngles += rotationSpeed * Time.deltaTime * rotationVector;
    }

    private void HandleZoom()
    {
        float zoomIncreaseAmount = 1f;
        float zoomSpeed = 5f;
        targetFollowOffset.y += InputManager.Instance.GetCameraZoomAmount() * zoomIncreaseAmount;
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, 
            targetFollowOffset, zoomSpeed * Time.deltaTime);
    }

}
