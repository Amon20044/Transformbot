using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Robot : MonoBehaviour
{
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;
    private bool isRobotMode = false;

    private Vector3 moveDirection;

    private CharacterController characterController;
    private Animator animator;

    public InputAction TransformAction;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();

        TransformAction = InputSystem.actions.FindAction("Transform");
        if (TransformAction != null)
            Debug.Log("Transform Action found");

        // Enable input
        TransformAction.Enable();

        // Subscribe ONCE
        TransformAction.performed += ctx => StartCoroutine(Transform());

        animator.SetFloat("Blend", 0.0f);
    }

    private void Update()
    {
        Run();
    }

    private void Run()
    {
        if (!isRobotMode)
        {
            moveDirection = Vector3.forward * runSpeed;
            characterController.Move(moveDirection * Time.deltaTime);
            animator.SetFloat("Blend", 1.0f, 0.01f, Time.deltaTime);
        }
        else
        {
            moveDirection = Vector3.forward * runSpeed * 1.5f;
            characterController.Move(moveDirection * Time.deltaTime);
            animator.SetFloat("Blend", 1.0f, 0.01f, Time.deltaTime);
        }
    }

    // Robot to Car and Car to Robot transformation
    public IEnumerator Transform()
    {
        isRobotMode = !isRobotMode;

        if (isRobotMode)
        {
            animator.SetBool("isRobot", true);
            animator.SetBool("isVehicle", false);
        }
        else
        {
            animator.SetBool("isRobot", false);
            animator.SetBool("isVehicle", true);
        }

        yield return null; // coroutine keeps Unity happy
    }
}
