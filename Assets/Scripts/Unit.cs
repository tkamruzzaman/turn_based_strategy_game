using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Unit : MonoBehaviour
{
    private const string ANIM_PARAM_IS_WALKING = "IsWalking";
    [SerializeField] private Animator animator;

    private Vector3 targetPosition;
    private float moveSpeed = 5f;
    private float rotaionSpeed = 7.5f;
    private float stoppingDistance = 0.1f;

    private void Awake()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveSpeed * Time.deltaTime * moveDirection;

            transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotaionSpeed);

            animator.SetBool(ANIM_PARAM_IS_WALKING, true);
        }
        else
        {
            animator.SetBool(ANIM_PARAM_IS_WALKING, false);
        }


    }

    public void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}