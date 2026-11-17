
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float forwardSpeed = 6f;
    public float laneDistance = 3f;
    public float laneChangeSpeed = 10f;

    public float jumpHeight = 2f;
    public float gravity = -20f;
    public float slideDuration = 1f;

    private CharacterController controller;
    private int currentLane = 1;
    private float verticalVelocity;

    private bool isSliding = false;
    private float slideTimer = 0f;
    private bool isGameOver = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {

        if (isGameOver)
            return;
        if (Keyboard.current == null)
            return;

        // Switch lanes
        if (Keyboard.current.aKey.wasPressedThisFrame ||
            Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            currentLane--;
        }

        if (Keyboard.current.dKey.wasPressedThisFrame ||
            Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            currentLane++;
        }

        currentLane = Mathf.Clamp(currentLane, 0, 2);

        // Jump when grounded
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        if (controller.isGrounded && !isSliding &&
            (Keyboard.current.spaceKey.wasPressedThisFrame ||
             Keyboard.current.upArrowKey.wasPressedThisFrame))
        {
            verticalVelocity =
                Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Start sliding
        if (!isSliding && controller.isGrounded &&
            (Keyboard.current.sKey.wasPressedThisFrame ||
             Keyboard.current.downArrowKey.wasPressedThisFrame))
        {
            StartSlide();
        }

        // Update sliding timer
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;

            if (slideTimer <= 0f)
            {
                StopSlide();
            }
        }

        // Apply gravity
        verticalVelocity += gravity * Time.deltaTime;

        // Calculate lane movement
        float targetX = (currentLane - 1) * laneDistance;

        float horizontalMovement = Mathf.MoveTowards(
            transform.position.x,
            targetX,
            laneChangeSpeed * Time.deltaTime
        ) - transform.position.x;

        // Move forward, sideways, and vertically
        Vector3 movement = new Vector3(
            horizontalMovement,
            verticalVelocity * Time.deltaTime,
            forwardSpeed * Time.deltaTime
        );

        controller.Move(movement);
    }

    void StartSlide()
    {
        isSliding = true;
        slideTimer = slideDuration;

        controller.height = 1f;
        controller.center = new Vector3(0, -0.5f, 0);

        Debug.Log("SLIDING STARTED");
        Debug.Log("Controller height: " + controller.height);
    }

    void StopSlide()
    {
        // Restore normal collision height
        controller.height = 2f;
        controller.center = Vector3.zero;

        isSliding = false;
    }




    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (isGameOver)
            return;

        if (hit.gameObject.GetComponent<Obstacle>() != null)
        {
            Debug.Log("Hit obstacle: " + hit.gameObject.name);
            Debug.Log("Sliding: " + isSliding);
            Debug.Log("Controller height: " + controller.height);

            isGameOver = true;
            forwardSpeed = 0f;

            Debug.Log("GAME OVER! Player hit an obstacle.");
        }
    }



}
