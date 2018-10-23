using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPC_Movement : MonoBehaviour
{
    #region Collision Variables
    public int m_nHorizontalRayCount = 5; // Amount of rays for horizontal collision detection
    public int m_nVerticalRayCount = 10; // Amount of rays for vertical collision detection
    public float m_fRayLength = 0.5f;
    public float m_fControllerHeight = 2f;
    public LayerMask m_CollisionMask; // Layer mask for collision checks
    public bool m_bIsGrounded = false; // Checks if player is grounded
    public int m_nJumpRayCount = 4;

    #endregion

    #region Movement Variables
    public float m_fMovementSpeed = 5.0f; // Base movement speed
    private float m_fMovementInput; // Vertical input
    public int m_nDirY; // Direction of vertical movement
    public int m_nDirZ; // Direction of horizontal movement
    public float m_fJumpForce = 20.0f; // Jump velocity
    public float m_fGravity = 5.0f; // Gravity velocity
    public float m_fVerticalVelocity = 0; // Current vertical velocity
    public float m_fHorizontalVelocity = 0; // Current vertical velocity
    public bool m_bCrouching = false;
    #endregion

    private float m_fCameraInput = 0; // Horizontal input 
    public float m_fCameraSpeed = 2f; // Speed of camera rotation

    private void Start()
    {

    }

    void Update()
    {
        MovementInput(); // Gets current movement input and direction
        CheckCollisions(); // Checks collisions based on velocity and direction
        Gravity(); // Applies gravity if needed
        if (Input.GetButtonDown("Jump")) // Checks if Jump button pressed
        {
            Jump(); // Calls jump function based on grounded value
        }
        Move(); // Applies velocity to player
    }

    public void MovementInput()
    {
        m_fMovementInput = Input.GetAxisRaw("Vertical"); // Gets all vertical axis input
        m_fHorizontalVelocity = m_fMovementInput * m_fMovementSpeed;
        m_fCameraInput = Input.GetAxisRaw("Horizontal"); // Horizontal movement    
        if (m_fVerticalVelocity > 0) // If vertical velocity is greater then 0
        {
            m_nDirY = 1; // Direction is 1 or forward/positive
        }
        else
        {
            m_nDirY = -1; // Direction is negative
        }
        if (Input.GetButton("Crouch")) // If crouch button being hit
        {
            m_bCrouching = true; // Crouching is true
        }
        else
        {
            m_bCrouching = false; // Crouhing is false
        }
    }


    //---------------------------------------------------------
    // Creates a grid of raycasts based on the direction of movement and velocity
    // If rays hit collision layer stop velocity in given direction 
    //---------------------------------------------------------
    public void CheckCollisions()
    {
        int count = 0; // Count of collisions detected
        for (int i = 0; i <= m_nJumpRayCount; i++) // Jump ray count
        {
            Vector3 startPos = this.transform.position - (transform.right * 0.5f) - (transform.forward * 0.5f); // Sets start positions of rays to back left
            startPos += i * ((transform.forward * (m_fRayLength / m_nJumpRayCount)) * 2); // Adds to ray positions to create a grid with number of rays
            for (int j = 0; j <= m_nJumpRayCount; j++) // Jump ray count
            {
                Debug.DrawRay(startPos + (transform.right * (j * ((m_fRayLength * 2) / m_nJumpRayCount))), Vector3.up * m_nDirY, Color.red);
                if (Physics.Raycast(startPos + (transform.right * (j * ((m_fRayLength * 2) / m_nJumpRayCount))), Vector3.up * m_nDirY, m_fControllerHeight * 0.5f, m_CollisionMask))
                {
                    count += 1;
                }
            }
        }

        if(count > 0)
        {
            m_bIsGrounded = true;
        }
        else
        {
            m_bIsGrounded = false;
        }

        for (int i = 0; i < m_nVerticalRayCount; i++)
        {
            if (i == 0) { } // Skips first rays
            else
            {
                Vector3 startPos;
                if(!m_bCrouching)
                {
                    startPos = this.transform.position - new Vector3(0, (m_fControllerHeight * 0.5f) - (i * (m_fControllerHeight / m_nVerticalRayCount)), 0);
                    startPos -= transform.right * m_fRayLength;
                }
                else
                {
                    startPos = this.transform.position - new Vector3(0, (m_fControllerHeight * 0.5f) - (i * ((m_fControllerHeight * 0.5f) / m_nVerticalRayCount)), 0);
                    startPos -= transform.right * m_fRayLength;
                }

                for (int j = 0; j <= m_nHorizontalRayCount; j++)
                {
                    Debug.DrawRay(startPos + (transform.right * (j * ((m_fRayLength * 2) / m_nHorizontalRayCount))), transform.forward, Color.red);
                    if (Physics.Raycast(startPos + (transform.right * (j * ((m_fRayLength * 2) / m_nHorizontalRayCount))), transform.forward * m_fMovementInput, m_fRayLength, m_CollisionMask))
                    {
                        m_fHorizontalVelocity = 0;
                    }
                }
            }
        }
    }

    public void Gravity()
    {
        if (m_bIsGrounded) // If grounded
        {
            m_fVerticalVelocity = 0; // Applies no gravity to vertical velocity
        }
        else
        {
            m_fVerticalVelocity -= m_fGravity * Time.deltaTime; // If not grounded applies constant downward velocity
        }
    }

    public void Jump()
    {
        if (m_bIsGrounded) // If grounded
        {
            m_fVerticalVelocity = m_fJumpForce; // Apply jump force to current vertical velocity
        }
        else
        {

        }
    }

    public void Move()
    {
        transform.Translate(new Vector3(0, m_fVerticalVelocity * Time.deltaTime, m_fHorizontalVelocity * Time.deltaTime)); // Applies velocity based on input, move speed and vertical velocity
        transform.Rotate(Vector3.up, m_fCameraInput * m_fCameraSpeed * Time.deltaTime); // Updates rotation based on horizontal input
    }
}
