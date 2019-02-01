using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class FPSPlayerController : MonoBehaviour
{
    // Unity components
    [Header("Prerequisite Settings")]
    public Animator Movement;
    public Camera FPSCamera;
    [Tooltip("Toggle locking the cursor to the center of the screen")]
    public bool LockCursor = true;
    [Space]
    // Player movement settings
    [Header("Movement Settings")]
    [Range(4, 20)] public int MoveSpeed = 8;
    [Range(2, 10)] public int RunMultiplier = 2;
    [Range(1, 10)] public int JumpHeight = 4;
    [Range(2, 6)] public int JumpBoost = 2;
    [Space]
    // Player look settings
    [Header("Look Settings")]
    [Tooltip("Toggle looking around the world")]
    public static bool LookInversion = false;
    [Tooltip("Toggle smoothing out the speed of the look sensitivity")]
    public static bool LookSmoothing = true;
    [Tooltip("Toggle unifying the look sensitivity(When active only change the Vertical)")]
    public static bool UnifiedSensitivity = false;
    [Range(1, 10)] public int VerticalSensitivity = 3;
    [Range(1, 10)] public int HorizontalSensitivity = 3;
    [Range(1, 5)] public int LookAcceleration = 1;
    [Range(1, 90)] public int AxisStop = 50;

    //Private variables
    private bool OnGround = true;
    private int currentJump = 0; //Multi jump variable
    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        CursorState();
        LockedSensitivity();
        MoveCharacter();
        LookRotation();
        if (Input.GetButtonDown("Jump") && (OnGround || JumpBoost > currentJump))
        {
            OnGround = false;
            rb.AddForce(Vector3.up * (JumpHeight * 25), ForceMode.Impulse);
            currentJump++;
        }
    }

    void OnCollisionEnter(Collision Col)
    {
        OnGround = true;
        currentJump = 0;
    }

    void CursorState()
    {
        //Change cursor state based on whether the game is paused
        if (PauseMenu.IsPaused)
            LockCursor = false;
        else
            LockCursor = true;

        //Check cursor state
        if (LockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void LockedSensitivity()
    {
        if (UnifiedSensitivity)
            HorizontalSensitivity = VerticalSensitivity;
    }

    void MoveCharacter()
    {
        //Checking the for the players movement
        //Animation for giving the player solid movement
        if (Movement != null)
        {
            //Back and forth direction
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
                Movement.SetBool("IsWalking", true);
            else
                Movement.SetBool("IsWalking", false);
        }

        //Back and forth direction
        if (Input.GetAxis("Vertical") != 0)
            transform.position += Input.GetAxis("Vertical") * transform.forward * Time.deltaTime * MoveSpeed;

        //Side to side direction
        if (Input.GetAxis("Horizontal") != 0)
            transform.position += Input.GetAxis("Horizontal") * transform.right * Time.deltaTime * MoveSpeed;
    }

    void LookRotation()
    {
        //Setup the players look rotation data
        float MouseX = Input.GetAxisRaw("Mouse X") * VerticalSensitivity;
        float MouseY = Input.GetAxisRaw("Mouse Y") * HorizontalSensitivity;

        //Check look inversion setting
        if (LookInversion)
            MouseX = -MouseX;
        else
            MouseY = -MouseY;

        //Enable/Disable look smoothing
        if (LookSmoothing)
        {
            MouseX *= Time.deltaTime;
            MouseY *= Time.deltaTime;
        }

        //Allow the player to look around
        transform.localRotation *= Quaternion.Euler(0, MouseX * (LookAcceleration * 10), 0);
        FPSCamera.transform.localRotation *= Quaternion.Euler(MouseY * (LookAcceleration * 10), 0, 0);
        FPSCamera.transform.localRotation = ClampRotationAroundXAxis(FPSCamera.transform.localRotation);
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        // Unity devs look rotation clamp code
        /// Which idiot thought is was a good idea to NOT make this a standard of the engine?
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, -AxisStop, AxisStop);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}
