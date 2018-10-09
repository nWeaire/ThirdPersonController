using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPC_Movement : MonoBehaviour
{

    public float m_fMovementSpeed = 5.0f;
    private float m_fVerticalInput = 0;
    private float m_fHorizontalInput = 0;
    public LayerMask m_LayerMask;
    public bool m_bIsGrounded = false;
    public float m_fJumpForce = 20.0f;
    public float m_fGravity = 5.0f;
    public float m_fVerticalVelocity = 0;

    // Update is called once per frame
    void Update()
    {
        CheckIfGrounded();
        MovementInput();
        CameraRotation();
        Gravity();
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        Move();
    }

    public void MovementInput()
    {
        m_fVerticalInput = Input.GetAxisRaw("Vertical");
    }

    public void CheckIfGrounded()
    {
        Debug.DrawRay(this.transform.position, Vector3.down, Color.cyan);
        if (Physics.Raycast(this.transform.position + new Vector3(0, 0.1f, 0), Vector3.down, 0.1f, m_LayerMask))
        {
            m_bIsGrounded = true;
        }
        else
        {
            m_bIsGrounded = false;
        }
    }

    public void CameraRotation()
    {
        m_fHorizontalInput = Input.GetAxisRaw("Horizontal");
        if (m_fVerticalInput >= 0)
        {
            this.transform.Rotate(Vector3.up, m_fHorizontalInput);
        }
        else
        {
            this.transform.Rotate(Vector3.up, -m_fHorizontalInput);
        }

    }

    public void Gravity()
    {
        if (m_bIsGrounded)
        {
            m_fVerticalVelocity = 0;
        }
        else
        {
            m_fVerticalVelocity -= m_fGravity * Time.deltaTime;
        }
    }

    public void Jump()
    {
        if (m_bIsGrounded)
        {
            m_fVerticalVelocity = m_fJumpForce;
        }
        else
        {

        }
    }

    public void Move()
    {
        transform.Translate(new Vector3(0, m_fVerticalVelocity * Time.deltaTime, m_fVerticalInput * m_fMovementSpeed * Time.deltaTime));
    }
}
