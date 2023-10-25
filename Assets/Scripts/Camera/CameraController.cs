using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private const float MIN_FOLLOW_Y_OFFSET = 2.0f;
    private const float MAX_FOLLOW_Y_OFFSET = 12.0f;

    private float moveSpeed = 10;
    private float rotationSpeed = 100;
    private float zoomAmount = 1f;
    private float zoomSpeed = 5;

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
        Vector3 inputMoveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            inputMoveDirection.z = +1f;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputMoveDirection.z = -1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputMoveDirection.x = -1f;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputMoveDirection.x = +1f;
        }

        Vector3 moveVector = transform.forward * inputMoveDirection.z
            + transform.right * inputMoveDirection.x;
        transform.position += moveSpeed * Time.deltaTime * moveVector;
    }

    private void HandleRotation()
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

        transform.eulerAngles += rotationSpeed * Time.deltaTime * rotationVector;
    }

    private void HandleZoom()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            targetFollowOffset.y -= zoomAmount;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetFollowOffset.y += zoomAmount;
        }

        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, zoomSpeed * Time.deltaTime);
    }

}
