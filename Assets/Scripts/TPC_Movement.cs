using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPC_Movement : MonoBehaviour
{

    public float m_fMovementSpeed = 5.0f; // Base movement speed
    private float m_fMovementInput; // Vertical input
    private float m_fCameraInput = 0; // Horizontal input
    public LayerMask m_LayerMask; // Layer mask for collision checks
    private CapsuleCollider m_cCapsuleCollider; // Reference to capsule collider
    public bool m_bIsGrounded = false; // Checks if player is grounded
    public float m_fJumpForce = 20.0f; // Jump velocity
    public float m_fGravity = 5.0f; // Gravity velocity
    public float m_fVerticalVelocity = 0; // Current vertical velocity
    public float m_fCameraSpeed = 2f; // Speed of camera rotation
    public int m_nDirY; // Direction of vertical movement
    public int m_nDirZ; // Direction of horizontal movement
    public int m_nHorizontalRayCount = 5;
    public int m_nVerticalRayCount = 10;

    private void Start()
    {
        m_cCapsuleCollider = GetComponent<CapsuleCollider>();

    }

    void Update()
    {
        CheckCollisions(); // Checks collisions based on velocity and direction
        MovementInput(); // Gets current movement input and direction
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
        m_fCameraInput = Input.GetAxisRaw("Horizontal"); // Horizontal movement    
        if (m_fVerticalVelocity > 0) // If vertical velocity is greater then 0
        {
            m_nDirY = 1; // Direction is 1 or forward/positive
        }
        else
        {
            m_nDirY = -1; // Direction is negative
        }

    }

    public void CheckCollisions()
    {
        if (Physics.Raycast(this.transform.position, Vector3.up * m_nDirY, m_cCapsuleCollider.height * 0.5f, m_LayerMask) // Casts 5 rays around player for vertical movement based on direction
            || Physics.Raycast(this.transform.position + new Vector3(m_cCapsuleCollider.radius, 0, m_cCapsuleCollider.radius), Vector3.up * m_nDirY, m_cCapsuleCollider.height * 0.5f, m_LayerMask)
            || Physics.Raycast(this.transform.position + new Vector3(-m_cCapsuleCollider.radius, 0, m_cCapsuleCollider.radius), Vector3.up * m_nDirY, m_cCapsuleCollider.height * 0.5f, m_LayerMask)
            || Physics.Raycast(this.transform.position + new Vector3(m_cCapsuleCollider.radius, 0, -m_cCapsuleCollider.radius), Vector3.up * m_nDirY, m_cCapsuleCollider.height * 0.5f, m_LayerMask)
            || Physics.Raycast(this.transform.position + new Vector3(-m_cCapsuleCollider.radius, 0, -m_cCapsuleCollider.radius), Vector3.up * m_nDirY, m_cCapsuleCollider.height * 0.5f, m_LayerMask)
            )
        {
            m_bIsGrounded = true; // If rays hit something is grounded
        }
        else
        {
            m_bIsGrounded = false;
        }
        for (int i = 0; i < m_nVerticalRayCount; i++)
        {
            Vector3 startPos = this.transform.position - new Vector3(0, (m_cCapsuleCollider.height * 0.45f) - (i * (m_cCapsuleCollider.height / m_nVerticalRayCount)), 0);
            startPos -= transform.right * m_cCapsuleCollider.radius;
            for (int j = 0; j <= m_nHorizontalRayCount; j++)
            {
                Debug.DrawRay(startPos + new Vector3(transform.right.x * j * ((m_cCapsuleCollider.radius * 2) / m_nHorizontalRayCount), 0, 0), transform.forward, Color.red);
                if(Physics.Raycast(startPos + new Vector3(j * ((m_cCapsuleCollider.radius * 2) / m_nHorizontalRayCount), 0, 0), transform.forward, m_cCapsuleCollider.radius,m_LayerMask))
                {
                    //Debug.Log("hit");
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
        transform.Translate(new Vector3( 0, m_fVerticalVelocity * Time.deltaTime, m_fMovementInput * m_fMovementSpeed * Time.deltaTime)); // Applies velocity based on input, move speed and vertical velocity
        transform.Rotate(Vector3.up, m_fCameraInput * m_fCameraSpeed * Time.deltaTime); // Updates rotation based on horizontal input
    }
}
